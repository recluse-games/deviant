using Deviant;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityAsync;


public class Entity : MonoBehaviour
{
    public string id = default;
    public string state = default;

    GameObject selectionArrow;
    private EncounterState _encounterStateComponentReference = default;

    public void Start()
    {
        _encounterStateComponentReference = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    public string GetId()
    {
        return this.id;
    }

    async public Task<bool> SetIdle()
    {
        // Update the Server With New Entity Animation State
        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.EntityStateAction = new Deviant.EntityStateAction();
        encounterRequest.EntityStateAction.Id = this.id;
        encounterRequest.EntityStateAction.State = Deviant.EntityStateNames.Idle;
        await _encounterStateComponentReference.UpdateEncounterAsync(encounterRequest);

        // Remove all highlighted tiles.
        Deviant.EncounterRequest encounterOverlayTilesRequest = new Deviant.EncounterRequest();
        encounterOverlayTilesRequest.EntityTargetAction = new Deviant.EntityTargetAction();
        encounterOverlayTilesRequest.EntityTargetAction.Id = this.id;
        encounterOverlayTilesRequest.EntityTargetAction.Tiles.Clear();
        await _encounterStateComponentReference.UpdateEncounterAsync(encounterOverlayTilesRequest);

        return true;
    }

    public void UpdateAnimation(Deviant.Encounter encounter)
    {
        for (int y = 0; y < encounter.Board.Entities.Entities_.Count; y++)
        {
            for (int x = 0; x < encounter.Board.Entities.Entities_[y].Entities.Count; x++)
            {
                //If the box is close to the ground
                if (this.id == encounter.Board.Entities.Entities_[y].Entities[x].Id)
                {
                    var currentState = encounter.Board.Entities.Entities_[y].Entities[x].State;

                    Debug.Log("Current state is: " + currentState);
                    // Update Animation State Machine Triggers
                    GetComponent<Animator>().SetTrigger(currentState.ToString().ToUpper());
                }
            }
        }
    }

    public void UpdateAnimationController(Deviant.Encounter encounter)
    {
        for (int y = 0; y < encounter.Board.Entities.Entities_.Count; y++)
        {
            for (int x = 0; x < encounter.Board.Entities.Entities_[y].Entities.Count; x++)
            {
                //If the box is close to the ground
                if (this.id == encounter.Board.Entities.Entities_[y].Entities[x].Id)
                {
                    var alignment = encounter.Board.Entities.Entities_[y].Entities[x].Alignment;
                    var entityClass = encounter.Board.Entities.Entities_[y].Entities[x].Class;

                    if (alignment == Deviant.Alignment.Friendly || alignment == Deviant.Alignment.Unfriendly)
                    {
                        alignment = Deviant.Alignment.Friendly;

                    }

                    if (GetComponent<Animator>().runtimeAnimatorController.ToString() != entityClass.ToString())
                    {
                        GetComponent<Animator>().runtimeAnimatorController = Resources.Load($"Art/Animations/Entity/{alignment.ToString()}/{entityClass.ToString()}/{entityClass.ToString()}") as RuntimeAnimatorController;

                    }
                }
            }
        }
    }

    async public void OnMouseDown()
    {
        // Retrieve the Current Encounter From Shared State.
        Deviant.Encounter encounterState = _encounterStateComponentReference.GetEncounter();

        if (encounterState.ActiveEntity.OwnerId == _encounterStateComponentReference.GetPlayerId() && encounterState.ActiveEntity.Id == this.id && encounterState.ActiveEntity.Ap > 0)
        {
            // Update the Server With New Entity Animation State
            Deviant.EncounterRequest encounterEntityStateRequest = new Deviant.EncounterRequest();
            encounterEntityStateRequest.EntityStateAction = new Deviant.EntityStateAction();
            encounterEntityStateRequest.EntityStateAction.Id = this.id;
            encounterEntityStateRequest.EntityStateAction.State = Deviant.EntityStateNames.Moving;
            await _encounterStateComponentReference.UpdateEncounterAsync(encounterEntityStateRequest);

            GameObject overLayGrid = GameObject.Find("IsometricGrid");
            GridLayout gridLayout = overLayGrid.transform.GetComponent<GridLayout>();

            Vector3Int cellLocation = gridLayout.WorldToCell(this.transform.position);

            List<Deviant.Tile> moveTargetTiles = new List<Deviant.Tile>();

            Vector3Int upOffset = new Vector3Int(1, 0, 0);
            Vector3Int downOffset = new Vector3Int(-1, 0, 0);
            Vector3Int leftOffset = new Vector3Int(0, 1, 0);
            Vector3Int rightOffset = new Vector3Int(0, -1, 0);

            Vector3Int tileMapUp = cellLocation + upOffset;
            Vector3Int tileMapDown = cellLocation + downOffset;
            Vector3Int tileMapLeft = cellLocation + leftOffset;
            Vector3Int tileMapRight = cellLocation + rightOffset;

            Deviant.Tile up = new Deviant.Tile();
            Deviant.Tile down = new Deviant.Tile();
            Deviant.Tile left = new Deviant.Tile();
            Deviant.Tile right = new Deviant.Tile();

            up.X = tileMapUp.x;
            up.Y = tileMapUp.y;

            down.X = tileMapDown.x;
            down.Y = tileMapDown.y;

            left.X = tileMapLeft.x;
            left.Y = tileMapLeft.y;

            right.X = tileMapRight.x;
            right.Y = tileMapRight.y;

            up.Id = "select_0001";
            down.Id = "select_0001";
            left.Id = "select_0001";
            right.Id = "select_0001";


            moveTargetTiles.Add(up);
            moveTargetTiles.Add(up);
            moveTargetTiles.Add(up);
            moveTargetTiles.Add(up);


            // Update the Server With Newly Highlighted Overlay Tiles
            Deviant.EncounterRequest encounterOverlayTilesRequest = new Deviant.EncounterRequest();
            encounterOverlayTilesRequest.EntityTargetAction = new Deviant.EntityTargetAction();
            encounterOverlayTilesRequest.EntityTargetAction.Id = this.id;
            encounterOverlayTilesRequest.EntityTargetAction.Tiles.Add(up);
            encounterOverlayTilesRequest.EntityTargetAction.Tiles.Add(down);
            encounterOverlayTilesRequest.EntityTargetAction.Tiles.Add(left);
            encounterOverlayTilesRequest.EntityTargetAction.Tiles.Add(right);

            await _encounterStateComponentReference.UpdateEncounterAsync(encounterOverlayTilesRequest);
        }
    }

    public void Update()
    {
        // Retrieve the Current Encounter From Shared State.
        Deviant.Encounter encounterState = _encounterStateComponentReference.GetEncounter();

        if (encounterState.ActiveEntity.Id == this.id && !this.selectionArrow)
        {
            Deviant.Entity activeEntity = encounterState.ActiveEntity;
            Sprite selectionArrow = Resources.Load<Sprite>("Art/Sprites/Entity/Friendly/active_entity_arrow_0000");
            var selectionArrowObj = new GameObject("SelectionArrow");
            selectionArrowObj.transform.parent = this.transform;

            var bounds = GetComponent<Entity>().GetComponent<SpriteRenderer>().bounds;

            var topOfSprite = new Vector3(bounds.extents.x / 2 - (float).1, bounds.extents.y * (float)2.3, 0.0f);
            selectionArrowObj.transform.localPosition = topOfSprite;

            SpriteRenderer rend = selectionArrowObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            rend.sprite = selectionArrow;
            rend.sortingOrder = 1;
            var newYellowColor = new Color(255f / 255f, 230f / 255f, 88f / 255f);
            rend.color = newYellowColor;
            selectionArrowObj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            this.selectionArrow = selectionArrowObj;
        }
        else if (this.selectionArrow)
        {
            var bounds = GetComponent<Entity>().GetComponent<SpriteRenderer>().bounds;

            var topOfSprite = new Vector3(bounds.extents.x / 2 - (float).1, bounds.extents.y * (float)2.3, 0.0f);

            this.selectionArrow.transform.localPosition = topOfSprite;
        }

        if (this.selectionArrow && encounterState.ActiveEntity.Id != this.id)
        {
            Destroy(this.selectionArrow);
        }
    }

    void OnMouseOver()
    {
        var mouseOverPanel = GameObject.FindGameObjectWithTag("ui_selected");

        for (int y = 0; y < encounter.Board.Entities.Entities_.Count; y++)
        {
            for (int x = 0; x < encounter.Board.Entities.Entities_[y].Entities.Count; x++)
            {
                if (this.id == encounter.Board.Entities.Entities_[y].Entities[x].Id)
                {
                    mouseOverPanel.GetComponentInChildren<Name>().UpdateValue( encounter.Board.Entities.Entities_[y].Entities[x].Title);
                    mouseOverPanel.GetComponentInChildren<Hp>().UpdateValue("");
                }
            }
        }

        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }
}
