using Deviant;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityAsync;
using UnityEngine.EventSystems;

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

        battlefield.ClearAllTiles();
        battlefield.SetTile(position, newTileAsset);
        battlefield.RefreshTile(position);
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

    private async Task<bool> ProcessAttack(CardPrefab selectedCard)
    {
        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.EntityActionName = Deviant.EntityActionNames.Play;
        encounterRequest.EntityPlayAction = new Deviant.EntityPlayAction();
        encounterRequest.EntityPlayAction.CardId = selectedCard.GetInstanceId();

        foreach (var action in selectedCard.GetSelectedTilePositions())
        {
            foreach (var pattern in selectedCard.GetSelectedTilePositions()[action.Key])
            {
                foreach (var tileLocation in selectedCard.GetSelectedTilePositions()[action.Key][pattern.Key])
                {
                    Deviant.Play newPlay = new Deviant.Play();
                    newPlay.X = tileLocation.x;
                    newPlay.Y = tileLocation.y;

                    encounterRequest.EntityPlayAction.Plays.Add(newPlay);
                }
            }
        }

        selectedCard.SetSelected(false);
        await encounterStateRef.UpdateEncounterAsync(encounterRequest);

        var activeEntity = encounterStateRef.GetEncounter().ActiveEntity;

        // Remove all highlighted tiles.
        Deviant.EncounterRequest encounterOverlayTilesRequest = new Deviant.EncounterRequest();
        encounterOverlayTilesRequest.EntityTargetAction = new Deviant.EntityTargetAction();
        encounterOverlayTilesRequest.EntityTargetAction.Id = activeEntity.Id;
        encounterOverlayTilesRequest.EntityTargetAction.Tiles.Clear();
        await encounterStateRef.UpdateEncounterAsync(encounterOverlayTilesRequest);

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
                        CardPrefab selectedCard = UpdateSelectedCard();

                        if (selectedCard)
                        {
                            if (selectedCard.GetSelected())
                            {
                                if (selectedCard.GetSelectedTilePositions().Count > 0)
                                {
                                    await ProcessAttack(selectedCard);
                                }
                            }
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

                        if(selectedCard)
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
}
