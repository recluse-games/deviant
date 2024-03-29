﻿using Deviant;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityAsync;
using UnityEngine.XR.WSA;
using System.Linq;
using AStarSharp;

public class Entity : MonoBehaviour
{
    public string id = default;
    public string state = default;
    public bool selected = default;

    GameObject selectionArrow;
    private EncounterState _encounterStateComponentReference = default;

    public void Start()
    {
        Material material = new Material(Shader.Find("Shader Graphs/PixelOutline"));
        material.SetColor("Color_C6F9B478", Color.blue); // CHANGE  Color_C6F9B478 with your Color prop ID
        gameObject.GetComponent<Renderer>().material = material;

        _encounterStateComponentReference = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    public void SetSelected(bool selectionState)
    {
        this.selected = selectionState;
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

    async public Task<bool> SetRotating()
    {
        // Update the Server With New Entity Animation State
        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.EntityStateAction = new Deviant.EntityStateAction();
        encounterRequest.EntityStateAction.Id = this.id;
        encounterRequest.EntityStateAction.State = Deviant.EntityStateNames.Rotating;
        await _encounterStateComponentReference.UpdateEncounterAsync(encounterRequest);

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

                    GetComponent<Animator>().runtimeAnimatorController = Resources.Load($"Art/Animations/Entity/{alignment.ToString()}/{entityClass.ToString()}/{entityClass.ToString()}") as RuntimeAnimatorController;
                }
            }
        }
    }

    public void UpdateOutline(Deviant.Encounter encounter)
    {
        var material = GetComponent<Renderer>().material;

        for (int y = 0; y < encounter.Board.Entities.Entities_.Count; y++)
        {
            for (int x = 0; x < encounter.Board.Entities.Entities_[y].Entities.Count; x++)
            {
                //If the box is close to the ground
                if (this.id == encounter.Board.Entities.Entities_[y].Entities[x].Id)
                {
                    var alignment = encounter.Board.Entities.Entities_[y].Entities[x].Alignment;

                    if (alignment == Deviant.Alignment.Friendly)
                    {
                        material.SetColor("_OutlineColor", Color.blue);
                    }
                    else if (alignment == Deviant.Alignment.Unfriendly)
                    {
                        material.SetColor("_OutlineColor", Color.red);
                    } else
                    {
                        Material newMaterial = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));
                        gameObject.GetComponent<Renderer>().material = newMaterial;
                    }
                }
            }
        }
    }

    // Function for 4 connected Pixels 
    public void boundaryFill4(int startx, int starty, bool firstIteration, int x, int y, string filledId, string blockedId, int limit, List<List<Deviant.Tile>> tiles)
    {
        if (tiles[x][y].Id != blockedId &&
            tiles[x][y].Id != filledId)
        {
            int apCostX;
            int apCostY;

            if (startx > x )
            {
                apCostX = startx - x;
            } else if (startx < x) {
                apCostX = x - startx;
            } else
            {
                apCostX = 0;
            }

            if (starty > y)
            {
                apCostY = starty - y;
            } else if (starty < y)
            {
                apCostY = y - starty;
            } else
            {
                apCostY = 0;
            }

            tiles[x][y].Id = filledId;

            if(limit - apCostX - apCostY > 0)
            {
                if (x + 1 <= 8)
                {
                    boundaryFill4(startx, starty, false, x + 1, y, filledId, blockedId, limit, tiles);
                }

                if (y + 1 <= 7)
                {
                    boundaryFill4(startx, starty, false, x, y + 1, filledId, blockedId, limit, tiles);
                }

                if (x - 1 >= 0)
                {
                    boundaryFill4(startx, starty, false, x - 1, y, filledId, blockedId, limit, tiles);
                }

                if (y - 1 >= 0)
                {
                    boundaryFill4(startx, starty, false, x, y - 1, filledId, blockedId, limit, tiles);
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

        List<List<Deviant.Tile>> moveTargetTiles = new List<List<Deviant.Tile>>();

        for (int y = 0; y < entitiesBoard.Entities_.Count; y++)
        {
            List<Deviant.Tile> newRow = new List<Deviant.Tile>();

            for(int x = 0; x < entitiesBoard.Entities_[y].Entities.Count; x++)
            {
                Deviant.Tile newTile = new Deviant.Tile();
                newTile.X = y;
                newTile.Y = x;

                if (entitiesBoard.Entities_[y].Entities[x].Id != "")
                {
                    if(y != cellLocation.x || x != cellLocation.y)
                    {
                        newTile.Id = "select_0002";
                    }
                }

                newRow.Add(newTile);
            }

            moveTargetTiles.Add(newRow);
        }

        this.boundaryFill4(cellLocation.x, cellLocation.y, true, cellLocation.x, cellLocation.y, "select_0001", "select_0002", avaliableAp, moveTargetTiles);

        Debug.Log("Targetting Tiles: " + moveTargetTiles);

        var result = moveTargetTiles.SelectMany(i => i).ToList();
        List<Deviant.Tile> filteredResult = result.Where(tile => tile.Id != "select_0002").ToList();

        return filteredResult;
    }

    public List<Deviant.Tile> GeneratePermissableRotations(Vector3Int currentLocation)
    {
        List<Deviant.Tile> tiles = new List<Deviant.Tile>();

        var up = new Deviant.Tile();
        up.X = currentLocation.x + 1;
        up.Y = currentLocation.y;
        up.Id = "select_0001";
        var down = new Deviant.Tile();
        down.X = currentLocation.x - 1;
        down.Y = currentLocation.y;
        down.Id = "select_0001";
        var left = new Deviant.Tile();
        left.X = currentLocation.x;
        left.Y = currentLocation.y + 1;
        left.Id = "select_0001";
        var right = new Deviant.Tile();
        right.X = currentLocation.x;
        right.Y = currentLocation.y - 1;
        right.Id = "select_0001";

        tiles.Add(up);
        tiles.Add(down);
        tiles.Add(left);
        tiles.Add(right);

        return tiles;
    }

    async public Task<bool> activeRotateAction ()
    {
        Deviant.Encounter encounter = _encounterStateComponentReference.GetEncounter();

        // Retrieve the Current Encounter From Shared State.
        Deviant.Encounter encounterState = _encounterStateComponentReference.GetEncounter();

        if (encounterState.ActiveEntity.OwnerId == _encounterStateComponentReference.GetPlayerId() && encounterState.ActiveEntity.Id == this.id)
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
                        selectedTiles = GeneratePermissableRotations(new Vector3Int(y, x, 0));

                        foreach (var tile in selectedTiles)
                        {
                            encounterOverlayTilesRequest.EntityTargetAction.Tiles.Add(tile);
                        }
                    }
                }
            }

            await _encounterStateComponentReference.UpdateEncounterAsync(encounterOverlayTilesRequest);
            await SetRotating();
        }

        return true;
    }

    async public Task<bool> activateMoveAction()
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

        return true;
    }

    async public Task<bool> activatePlayerMenu()
    {
        if(this.selected == true)
        {
            this.selected = false;
            return true;
        } else
        {
            Deviant.Encounter encounterState = _encounterStateComponentReference.GetEncounter();

            GameObject actionMenu = GameObject.Find("/UI/ActionMenu");

            if (encounterState.ActiveEntity.Id == this.id)
            {
                actionMenu.GetComponent<ActionMenu>().Show();

                Vector3 point = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z));

                actionMenu.transform.position = point;
            }

            this.selected = true;

            return true;
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
            selectionArrowObj.transform.position += new Vector3(0, 0, 10);

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
