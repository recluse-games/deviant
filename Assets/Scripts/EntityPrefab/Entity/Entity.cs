using Deviant;
using System;
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

    public List<Deviant.Tile> GeneratePermissableMoves(int avaliableAp, Deviant.Entities entitiesBoard)
    {
        // Get the current cell location of this entity on the tilemap.
        GameObject overLayGrid = GameObject.Find("IsometricGrid");
        GridLayout gridLayout = overLayGrid.transform.GetComponent<GridLayout>();
        Vector3Int cellLocation = gridLayout.WorldToCell(this.transform.position);

        List<Deviant.Tile> moveTargetTiles = new List<Deviant.Tile>();
        var rows = entitiesBoard.Entities_[0].Entities.Count;
        var cols = entitiesBoard.Entities_.Count;

        var rMax = Math.Min(cellLocation.y + avaliableAp + 1, rows);

        // Initialize Empty Increments and Maxes
        int cMax, yOff, r, c;

        // Start at the first row with a permissible move
        for (r = Math.Max(cellLocation.y - avaliableAp, 0); r < rMax; r++)
        {
            yOff = Math.Abs(r - cellLocation.y);

            // Work out where we should stop looping for this row
            cMax = Math.Min(cellLocation.x + avaliableAp - yOff + 1, cols);

            // Start at the first column with a permissible move
            for (c = Math.Max(cellLocation.x - avaliableAp + yOff, 0); c < cMax; c++)
            {
                // If it's not the current position, add it to the result
                if (cellLocation.x != c || cellLocation.y != r) {
                    Deviant.Tile newTile = new Deviant.Tile();
                    newTile.X = c;
                    newTile.Y = r;
                    newTile.Id = "select_0001";

                    moveTargetTiles.Add(newTile);
                }
            }
        }

        Debug.Log("Targetting Tiles: " + moveTargetTiles);

        return moveTargetTiles;
    }

    async public void OnMouseDown()
    {
        Deviant.Encounter encounter = _encounterStateComponentReference.GetEncounter();

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

            // Update the Server With Newly Highlighted Overlay Tiles
            Deviant.EncounterRequest encounterOverlayTilesRequest = new Deviant.EncounterRequest();
            encounterOverlayTilesRequest.EntityTargetAction = new Deviant.EntityTargetAction();
            encounterOverlayTilesRequest.EntityTargetAction.Id = this.id;

            List<Deviant.Tile> selectedTiles;

            for (int y = 0; y < encounter.Board.Entities.Entities_.Count; y++)
            {
                for (int x = 0; x < encounter.Board.Entities.Entities_[y].Entities.Count; x++)
                {
                    if (this.id == encounter.Board.Entities.Entities_[y].Entities[x].Id)
                    {
                        selectedTiles = GeneratePermissableMoves(encounter.Board.Entities.Entities_[y].Entities[x].Ap, encounter.Board.Entities);
                        foreach (var tile in selectedTiles)
                        {
                            encounterOverlayTilesRequest.EntityTargetAction.Tiles.Add(tile);
                        }
                    }
                }
            }

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
        Deviant.Encounter encounter = _encounterStateComponentReference.GetEncounter();
        var mouseOverPanel = GameObject.FindWithTag("ui_selected");

        for (int y = 0; y < encounter.Board.Entities.Entities_.Count; y++)
        {
            for (int x = 0; x < encounter.Board.Entities.Entities_[y].Entities.Count; x++)
            {
                if (this.id == encounter.Board.Entities.Entities_[y].Entities[x].Id)
                {
                    mouseOverPanel.GetComponentInChildren<Selected.Name>().UpdateValue(encounter.Board.Entities.Entities_[y].Entities[x].Name);
                    mouseOverPanel.GetComponentInChildren<Selected.Hp>().UpdateValue(encounter.Board.Entities.Entities_[y].Entities[x].Hp.ToString());
                }
            }
        }

        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        Deviant.Encounter encounter = _encounterStateComponentReference.GetEncounter();
        var mouseOverPanel = GameObject.FindGameObjectWithTag("ui_selected");
        mouseOverPanel.GetComponentInChildren<Selected.Name>().UpdateValue("");
        mouseOverPanel.GetComponentInChildren<Selected.Hp>().UpdateValue("");

        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }
}
