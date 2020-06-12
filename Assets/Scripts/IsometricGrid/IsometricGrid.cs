using Deviant;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityAsync;
using UnityEngine.EventSystems;
using System.Runtime.Remoting.Lifetime;
using AStarSharp;
using TMPro;

public class IsometricGrid : MonoBehaviour
{
    public EncounterState encounterStateRef = default;
    public EventSystem eventSystem = default;

    public void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
        eventSystem = GameObject.Find("/EventSystem").GetComponent<EventSystem>();
    }

    private bool validateEntityActive(Entity selectedEntity, Deviant.Entity activeEntity)
    {
        if(selectedEntity.GetId() == activeEntity.Id)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
        Tilemap battlefield = this.transform.Find("LocalBattlefieldOverlay").GetComponent<Tilemap>();
        Deviant.Entity activeEntity = encounterStateRef.encounter.ActiveEntity;
        Entity[] entityObjects = FindObjectsOfType<Entity>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        
        foreach (Entity entity in entityObjects)
        {
            if (validateEntityActive(entity, activeEntity) && activeEntity.State == Deviant.EntityStateNames.Moving)
            {
                // Draws a blue line from this transform to the target
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(entity.transform.position, worldPoint);
            }
        }
    }

    private void ProcessMouseHover()
    {
        GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
        Tilemap battlefield = this.transform.Find("LocalBattlefieldOverlay").GetComponent<Tilemap>();
        Deviant.Entity activeEntity = encounterStateRef.encounter.ActiveEntity;
        Entity[] entityObjects = FindObjectsOfType<Entity>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = gridLayout.WorldToCell(worldPoint);
        UnityEngine.Tilemaps.Tile newTileAsset = Resources.Load<UnityEngine.Tilemaps.Tile>($"Art/Tiles/select_0000");

        //TestAStar(new Vector2(position.x, position.y));

        battlefield.ClearAllTiles();
        battlefield.SetTile(position, newTileAsset);
        battlefield.RefreshTile(position);
    }
    
    private async Task<bool> ProcessRotation()
    {
        GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
        Tilemap battlefield = this.transform.Find("Battlefield").GetComponent<Tilemap>();
        Deviant.Entity activeEntity = encounterStateRef.encounter.ActiveEntity;
        Entity[] entityObjects = FindObjectsOfType<Entity>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = gridLayout.WorldToCell(worldPoint);

        foreach (Entity entity in entityObjects)
        {
            if (validateEntityActive(entity, activeEntity) && activeEntity.State == Deviant.EntityStateNames.Rotating)
            {
                Deviant.EntityRotationNames rotation;

                Vector3 startingPos = gridLayout.WorldToCell(entity.transform.parent.position);

                if(startingPos.x > position.x)
                {
                    rotation = Deviant.EntityRotationNames.South;

                } else if (startingPos.x < position.x)
                {
                    rotation = Deviant.EntityRotationNames.North;
                }
                else if (startingPos.y > position.y)
                {
                    rotation = Deviant.EntityRotationNames.West;
                }
                else if (startingPos.y < position.y)
                {
                    rotation = Deviant.EntityRotationNames.East;
                } else
                {
                    rotation = activeEntity.Rotation;
                }

                await updatePlayerRotation(rotation);
             
                break;
            };
        }

        // Remove all highlighted tiles.
        Deviant.EncounterRequest encounterOverlayTilesRequest = new Deviant.EncounterRequest();
        encounterOverlayTilesRequest.EntityTargetAction = new Deviant.EntityTargetAction();
        encounterOverlayTilesRequest.EntityTargetAction.Id = activeEntity.Id;
        encounterOverlayTilesRequest.EntityTargetAction.Tiles.Clear();
        await encounterStateRef.UpdateEncounterAsync(encounterOverlayTilesRequest);

        return true;
    }

    private async Task<bool> ProcessMove()
    {
        GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
        Tilemap battlefield = this.transform.Find("Battlefield").GetComponent<Tilemap>();
        Deviant.Entity activeEntity = encounterStateRef.encounter.ActiveEntity;
        Entity[] entityObjects = FindObjectsOfType<Entity>();
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = gridLayout.WorldToCell(worldPoint);

        foreach (Entity entity in entityObjects)
        {
            if (validateEntityActive(entity, activeEntity) && activeEntity.State == Deviant.EntityStateNames.Moving)
            {
                Vector3 startingPos = entity.transform.parent.position;
                await updatePlayerPosition(battlefield.WorldToCell(startingPos).x, battlefield.WorldToCell(startingPos).y, position.x, position.y);
                break;
            };
        }

        return true;
    }

    private async Task<bool> ProcessEntitySelection()
    {
        GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
        Tilemap battlefield = this.transform.Find("LocalBattlefieldOverlay").GetComponent<Tilemap>();
        Deviant.Entity activeEntity = encounterStateRef.encounter.ActiveEntity;
        Entity[] entityObjects = FindObjectsOfType<Entity>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = gridLayout.WorldToCell(worldPoint);

        var encounter = encounterStateRef.GetEncounter();

        for(var y = 0; y < encounter.Board.Entities.Entities_.Count; y ++)
        {
            for (var x = 0; x < encounter.Board.Entities.Entities_[y].Entities.Count; x++)
            {
                if(encounter.ActiveEntity.Id == encounter.Board.Entities.Entities_[y].Entities[x].Id && position.y == x && position.x == y)
                {
                    foreach (Entity entity in entityObjects)
                    {
                        if (entity.GetId() == encounter.Board.Entities.Entities_[y].Entities[x].Id)
                        {
                            await entity.activatePlayerMenu();
                            break;
                        };
                    }
                }
            }
        }

        return true;
    }

    async public void Update()
    {
        // Block input to this component while we're on a GUI element.
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            ProcessMouseHover();

            if (encounterStateRef.GetEncounter() != null)
            {
                if (encounterStateRef.GetEncounter().ActiveEntity.OwnerId == encounterStateRef.GetPlayerId())
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        await ProcessEntitySelection();

                        if (encounterStateRef.GetEncounter().ActiveEntity.State == Deviant.EntityStateNames.Rotating)
                        {
                            await ProcessRotation();
                        }
                        else
                        {
                            await ProcessMove();
                        }
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        CardPrefab selectedCard = UpdateSelectedCard();

                        var activeEntity = encounterStateRef.GetEncounter().ActiveEntity;
                        var entityObjects = FindObjectsOfType<Entity>();

                        var actionMenu = FindObjectOfType<ActionMenu>();
                        actionMenu.Hide();

                        if (selectedCard)
                        {
                            selectedCard.SetSelected(false);
                        }

                        foreach (Entity entity in entityObjects)
                        {
                            if (validateEntityActive(entity, activeEntity))
                            {
                                await entity.SetIdle();
                                break;
                            };
                        }

                        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
                        encounterRequest.EntityTargetAction = new EntityTargetAction();
                        encounterRequest.EntityTargetAction.Tiles.Clear();

                        await encounterStateRef.UpdateEncounterAsync(encounterRequest);

                        // Remove all highlighted tiles.
                        Deviant.EncounterRequest encounterOverlayTilesRequest = new Deviant.EncounterRequest();
                        encounterOverlayTilesRequest.EntityTargetAction = new Deviant.EntityTargetAction();
                        encounterOverlayTilesRequest.EntityTargetAction.Id = activeEntity.Id;
                        encounterOverlayTilesRequest.EntityTargetAction.Tiles.Clear();
                        await encounterStateRef.UpdateEncounterAsync(encounterOverlayTilesRequest);
                    }
                }
            }
        }
    }

    private CardPrefab UpdateSelectedCard()
    {
        CardPrefab selectedCard = null;

        var currentCards = GameObject.FindGameObjectsWithTag("hand");

        foreach (var currentCard in currentCards)
        {
            if (currentCard.GetComponent<CardPrefab>().GetSelected())
            {
                return currentCard.GetComponent<CardPrefab>();
            }
        }

        return selectedCard;
    }

    async public Task<bool> updatePlayerPosition(int startx, int starty, int endx, int endy)
    {
        Deviant.EntityMoveAction entityMoveAction = new Deviant.EntityMoveAction();
        entityMoveAction.StartXPosition = startx;
        entityMoveAction.StartYPosition = starty;
        entityMoveAction.FinalXPosition = endx;
        entityMoveAction.FinalYPosition = endy;

        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.PlayerId = encounterStateRef.GetPlayerId();
        encounterRequest.Encounter = encounterStateRef.encounter;
        encounterRequest.EntityActionName = Deviant.EntityActionNames.Move;
        encounterRequest.EntityMoveAction = entityMoveAction;


        await encounterStateRef.UpdateEncounterAsync(encounterRequest);

        return true;
    }

    async public Task<bool> updatePlayerRotation(Deviant.EntityRotationNames rotation)
    {
        Deviant.EntityRotateAction entityRotateAction = new Deviant.EntityRotateAction();
        entityRotateAction.Rotation = rotation;

        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.EntityActionName = Deviant.EntityActionNames.Rotate;
        encounterRequest.EntityRotateAction = entityRotateAction;


        Debug.Log("Rotating");
        await encounterStateRef.UpdateEncounterAsync(encounterRequest);

        return true;
    }

    /**
    private List<List<Node>> ConstructAStarGrid()
    {
        List<List<Node>> temp = new List<List<Node>>();
        bool walkable = false;

        Deviant.Encounter encounter = encounterStateRef.GetEncounter();

        for (int y = 0; y < encounter.Board.Entities.Entities_.Count; y++)
        {
            temp.Add(new List<Node>());

            for (int x = 0; x < encounter.Board.Entities.Entities_[y].Entities.Count; x++)
            {
                if (encounter.Board.Entities.Entities_[y].Entities[x].Id != "" && encounter.Board.Entities.Entities_[y].Entities[x].Id != encounter.ActiveEntity.Id)
                {
                    walkable = false;
                }
                else
                {
                    walkable = true;
                }

                temp[y].Add(new Node(new Vector2(y, x), walkable));
            }
        }

        return temp;
    }

    public void TestAStar(Vector2 mousePosition)
    {
        Deviant.Encounter encounter = encounterStateRef.GetEncounter();

        GameObject overLayGrid = GameObject.Find("IsometricGrid");
        Vector2 pathfindOrigin = new Vector2();

        for (int y = 0; y < encounter.Board.Entities.Entities_.Count; y++)
        {
            for (int x = 0; x < encounter.Board.Entities.Entities_[y].Entities.Count; x++)
            {
                if (encounter.Board.Entities.Entities_[y].Entities[x].Id == encounter.ActiveEntity.Id)
                {
                    pathfindOrigin = new Vector2(y, x);
                }

            }
        }

        var A = new Astar(ConstructAStarGrid());
        var path = A.FindPath(pathfindOrigin, mousePosition, encounter.ActiveEntity.Ap);
        Tilemap battlefield = this.transform.Find("LocalBattlefieldOverlay").GetComponent<Tilemap>();
        UnityEngine.Tilemaps.Tile newTileAsset = Resources.Load<UnityEngine.Tilemaps.Tile>($"Art/Tiles/select_0001");

        battlefield.ClearAllTiles();
        while (path.Count > 0)
        {
            var input = path.Pop();
            Debug.Log("DrawMe" + input.Position);
            battlefield.SetTile(new Vector3Int((int)input.Position.x, (int)input.Position.y, 0), newTileAsset);
            battlefield.RefreshTile(new Vector3Int((int)input.Position.x, (int)input.Position.y, 0));
        }
    }
    **/
}
