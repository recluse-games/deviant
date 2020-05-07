using Deviant;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CardPrefab  : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    Transform entity = default;

    private bool mouse_over = false;
    private bool selected = false;
    private bool visable = false;
    public EncounterState encounterStateRef = default;

    private Dictionary<string, Dictionary<string, List<Vector3Int>>> selectedPatternTilePositions = new Dictionary<string, Dictionary< string, List<Vector3Int>>>();

    public void SetVisability(bool visability) {
        this.visable = visability;
    }

    public bool GetSelected()
    {
        return this.selected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.visable == true) {
            this.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            mouse_over = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(this.visable == true) {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            mouse_over = false;
        }
    }

    public void ClearSelectedTiles()
    {
        this.selectedPatternTilePositions = null;
    }

    public void switchUp(Dictionary<string, Dictionary<string, List<Vector3Int>>> dictionary, string actionKey, string direction, List<Vector3Int> up, List<Vector3Int> down, List<Vector3Int> left, List<Vector3Int> right)
    {
        switch (direction)
        {
            case "up":
                dictionary[actionKey]["up"] = up;
                dictionary[actionKey]["down"] = down;
                dictionary[actionKey]["left"] = left;
                dictionary[actionKey]["right"] = right;
                break;
            case "down":
                dictionary[actionKey]["down"] = up;
                dictionary[actionKey]["up"] = down;
                dictionary[actionKey]["left"] = left;
                dictionary[actionKey]["right"] = right;
                break;
            case "left":
                dictionary[actionKey]["up"] = right;
                dictionary[actionKey]["left"] = up;
                dictionary[actionKey]["down"] = left;
                dictionary[actionKey]["right"] = down;
                break;
            case "right":
                dictionary[actionKey]["up"] = left;
                dictionary[actionKey]["left"] = down;
                dictionary[actionKey]["down"] = right;
                dictionary[actionKey]["right"] = up;
                break;
        }

        this.selectedPatternTilePositions = dictionary;
    }
    public void switchDown(Dictionary<string, Dictionary<string, List<Vector3Int>>> dictionary, string actionKey, string direction, List<Vector3Int> up, List<Vector3Int> down, List<Vector3Int> left, List<Vector3Int> right)
    {
        switch (direction)
        {
            case "up":
                dictionary[actionKey]["up"] = down;
                dictionary[actionKey]["down"] = up;
                dictionary[actionKey]["left"] = left;
                dictionary[actionKey]["right"] = right;
                break;
            case "down":
                dictionary[actionKey]["up"] = up;
                dictionary[actionKey]["left"] = left;
                dictionary[actionKey]["right"] = right;
                dictionary[actionKey]["down"] = down;
                break;
            case "left":
                dictionary[actionKey]["up"] = left;
                dictionary[actionKey]["left"] = down;
                dictionary[actionKey]["down"] = right;
                dictionary[actionKey]["right"] = up;
                break;
            case "right":
                dictionary[actionKey]["up"] = right;
                dictionary[actionKey]["left"] = up;
                dictionary[actionKey]["down"] = left;
                dictionary[actionKey]["right"] = down;
                break;
        }

        Debug.Log(direction);
        Debug.Log("down" + selectedPatternTilePositions[actionKey]["down"].Count());
        Debug.Log("right" + selectedPatternTilePositions[actionKey]["right"].Count());
        this.selectedPatternTilePositions = dictionary;

    }

    public void switchLeft(Dictionary<string, Dictionary<string, List<Vector3Int>>> dictionary, string actionKey, string direction, List<Vector3Int> up, List<Vector3Int> down, List<Vector3Int> left, List<Vector3Int> right)
    {
        switch (direction)
        {
            case "up":
                dictionary[actionKey]["up"] = left;
                dictionary[actionKey]["left"] = down;
                dictionary[actionKey]["down"] = right;
                dictionary[actionKey]["right"] = up;
                break;
            case "down":
                dictionary[actionKey]["up"] = right;
                dictionary[actionKey]["left"] = up;
                dictionary[actionKey]["down"] = left;
                dictionary[actionKey]["right"] = down;
                break;
            case "left":
                dictionary[actionKey]["up"] = up;
                dictionary[actionKey]["left"] = left;
                dictionary[actionKey]["right"] = right;
                dictionary[actionKey]["down"] = down;
                break;
            case "right":
                dictionary[actionKey]["left"] = right;
                dictionary[actionKey]["right"] = left;
                break;
        }

        this.selectedPatternTilePositions = dictionary;

    }
    public void switchRight(Dictionary<string, Dictionary<string, List<Vector3Int>>> dictionary, string actionKey, string direction, List<Vector3Int> up, List<Vector3Int> down, List<Vector3Int> left, List<Vector3Int> right)
    {
        switch (direction)
        {
            case "up":
                dictionary[actionKey]["up"] = right;
                dictionary[actionKey]["left"] = up;
                dictionary[actionKey]["down"] = left;
                dictionary[actionKey]["right"] = down;
                break;
            case "down":
                dictionary[actionKey]["up"] = left;
                dictionary[actionKey]["left"] = down;
                dictionary[actionKey]["down"] = right;
                dictionary[actionKey]["right"] = up;
                break;
            case "left":
                dictionary[actionKey]["left"] = right;
                dictionary[actionKey]["right"] = left;
                break;
            case "right":
                dictionary[actionKey]["up"] = up;
                dictionary[actionKey]["left"] = left;
                dictionary[actionKey]["right"] = right;
                dictionary[actionKey]["down"] = down;
                break;
        }

        this.selectedPatternTilePositions = dictionary;
    }

    public void UpdateSelectedTiles(string direction, string previousDirection, Vector3Int entityTile, Vector3Int previousEntityTile)
    {
        GameObject battleFieldOverlay = GameObject.Find("BattlefieldOverlay");
        Tilemap battleFieldOverlayTilemap = battleFieldOverlay.GetComponent<Tilemap>();
        UnityEngine.Tilemaps.Tile selectedTile = Resources.Load<UnityEngine.Tilemaps.Tile>("Art/Tiles/select_0000");

        // Delete Old Tiles + Update Location 
        foreach (var action in this.selectedPatternTilePositions)
        {
            foreach (var directionalPattern in this.selectedPatternTilePositions[action.Key])
            {
                var newPatterns = new List<Vector3Int>();
                var originalPatterns = new List<Vector3Int>();

                for (var i = 0; i < this.selectedPatternTilePositions[action.Key][directionalPattern.Key].Count; i++)
                {
                    var pattern = this.selectedPatternTilePositions[action.Key][directionalPattern.Key][i];

                    Vector3Int newLocation = default;

                    int x = pattern.x;
                    int y = pattern.y;

                    Debug.Log("Original Tile Rotation Locaiton" + pattern);

                    if (directionalPattern.Key == "up")
                    {
                        switch (direction)
                        {
                            case "up":
                                newLocation.x = x;
                                newLocation.y = y;
                                break;
                            case "down":
                                newLocation.x = entityTile.x - (System.Math.Abs(x) - entityTile.x);
                                newLocation.y = y;
                                break;
                            case "left":
                                newLocation.x = y + entityTile.x - entityTile.y;
                                newLocation.y = (System.Math.Abs(x - entityTile.x) * (1)) + entityTile.y;
                                break;
                            case "right":
                                newLocation.x = y + entityTile.x - entityTile.y;
                                newLocation.y = (System.Math.Abs(entityTile.x - x) * (-1)) + entityTile.y;

                                break;
                        }
                    }
                    else if (directionalPattern.Key == "down")
                    {
                        switch (direction)
                        {
                            case "up":
                                newLocation.x = entityTile.x + (entityTile.x - x);
                                newLocation.y = y;
                                break;
                            case "down":
                                newLocation.x = x;
                                newLocation.y = y;
                                break;
                            case "left":
                                newLocation.x = y + entityTile.x - entityTile.y;
                                newLocation.y = (System.Math.Abs(entityTile.x - x) * (1)) + entityTile.y;

                                break;
                            case "right":
                                newLocation.x = y + entityTile.x - entityTile.y;
                                newLocation.y = (System.Math.Abs(x - entityTile.x) * (-1)) + entityTile.y;

                                break;
                        }
                    }
                    else if (directionalPattern.Key == "left")
                    {
                        switch (direction)
                        {
                            case "up":
                                newLocation.x = (System.Math.Abs(y - entityTile.y) * (1)) + entityTile.x;
                                newLocation.y = (entityTile.x - x) + entityTile.y;
                                break;
                            case "down":
                                newLocation.x = (System.Math.Abs(y - entityTile.y) * (-1)) + entityTile.x;
                                newLocation.y = (x - entityTile.x) + entityTile.y;
                                break;
                            case "left":
                                newLocation.x = x;
                                newLocation.y = y;

                                break;
                            case "right":
                                newLocation.x = x;
                                newLocation.y = (System.Math.Abs(y) * (-1)) + (entityTile.y * 2);

                                break;
                        }
                    }
                    else if (directionalPattern.Key == "right")
                    {
                        switch (direction)
                        {
                            case "up":
                                newLocation.x = (System.Math.Abs(y - entityTile.y) * (1)) + entityTile.x;
                                newLocation.y = (entityTile.x - x) + entityTile.y;
                                break;
                            case "down":
                                newLocation.x = (System.Math.Abs(y - entityTile.y) * (-1)) + entityTile.x;
                                newLocation.y = (x - entityTile.x) + entityTile.y;

                                break;
                            case "left":
                                newLocation.x = x;
                                newLocation.y = y * (-1) + (entityTile.y * 2);

                                break;
                            case "right":
                                newLocation.x = x;
                                newLocation.y = y;

                                break;
                        }
                    }

                    Debug.Log("New Tile Rotation Locaiton" + newLocation);

                    originalPatterns.Add(new Vector3Int(pattern.x, pattern.y, pattern.z));
                    newPatterns.Add(new Vector3Int(newLocation.x, newLocation.y, newLocation.z));

                    this.selectedPatternTilePositions[action.Key][directionalPattern.Key][i] = newLocation;
                }

                foreach(var pattern in newPatterns)
                {
                   var matched = originalPatterns.Where(oldPattern => oldPattern.x != pattern.x && oldPattern.y != pattern.y).ToList();
                    foreach(var matchedPattern in matched)
                    {
                        battleFieldOverlayTilemap.SetTile(matchedPattern, null);
                    }
                }
            }
        }

        // Create a new empty dictionary 
        Dictionary<string, Dictionary<string, List<Vector3Int>>> newSelectedTilePositions = new Dictionary<string, Dictionary<string, List<Vector3Int>>>();
        List<string> actionKeys = new List<string>(this.selectedPatternTilePositions.Keys);

        foreach (var actionKey in actionKeys)
        {
            newSelectedTilePositions.Add(actionKey, new Dictionary<string, List<Vector3Int>> { });
            newSelectedTilePositions[actionKey].Add("up", new List<Vector3Int>());
            newSelectedTilePositions[actionKey].Add("down", new List<Vector3Int>());
            newSelectedTilePositions[actionKey].Add("left", new List<Vector3Int>());
            newSelectedTilePositions[actionKey].Add("right", new List<Vector3Int>());

            List<Vector3Int> up = new List<Vector3Int>();
            List<Vector3Int> down = new List<Vector3Int>();
            List<Vector3Int> left = new List<Vector3Int>();
            List<Vector3Int> right = new List<Vector3Int>();

            foreach(var vector in this.selectedPatternTilePositions[actionKey]["up"])
            {
                Vector3Int newVector = new Vector3Int(vector.x, vector.y, vector.z);
                up.Add(newVector);
            }

            foreach (var vector in this.selectedPatternTilePositions[actionKey]["down"])
            {
                Vector3Int newVector = new Vector3Int(vector.x, vector.y, vector.z);
                down.Add(newVector);

            }

            foreach (var vector in this.selectedPatternTilePositions[actionKey]["left"])
            {
                Vector3Int newVector = new Vector3Int(vector.x, vector.y, vector.z);
                left.Add(newVector);

            }

            foreach (var vector in this.selectedPatternTilePositions[actionKey]["right"])
            {
                Vector3Int newVector = new Vector3Int(vector.x, vector.y, vector.z);
                right.Add(newVector);

            }

            switch (previousDirection)
            {
                case "up":
                    switchUp(newSelectedTilePositions, actionKey, direction, up, down, left, right);
                    break;
                case "down":
                    switchDown(newSelectedTilePositions, actionKey, direction, up, down, left, right);
                    break;
                case "left":
                    switchLeft(newSelectedTilePositions, actionKey, direction, up, down, left, right);
                    break;
                case "right":
                    switchRight(newSelectedTilePositions, actionKey, direction, up, down, left, right);
                    break;
            }
        }

        // Draw New Tiles
        foreach (var actionKey in newSelectedTilePositions)
        {
            foreach (var directionKey in newSelectedTilePositions[actionKey.Key])
            {
                foreach (var vector in newSelectedTilePositions[actionKey.Key][directionKey.Key])
                {
                    battleFieldOverlayTilemap.SetTile(vector, selectedTile);
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Retrieve the Current Encounter From Shared State.
		Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
		Deviant.Board board = encounterState.Board;
        Deviant.Entity activeEntity = encounterState.ActiveEntity;
        
        if (this.visable == true) {
            GameObject entity = GameObject.Find("entity_" + activeEntity.Id);;
        
            GameObject isometricGameGrid = GameObject.Find("IsometricGrid");
            GridLayout isometricGameGridLayout = isometricGameGrid.transform.GetComponent<GridLayout>();
            Vector3Int entityCellLocation = isometricGameGridLayout.WorldToCell(entity.transform.position);

            UnityEngine.Tilemaps.Tile selectedTile = Resources.Load<UnityEngine.Tilemaps.Tile>("Art/Tiles/select_0000");
            GameObject battleFieldOverlay = GameObject.Find("BattlefieldOverlay");
            Tilemap battleFieldOverlayTilemap = battleFieldOverlay.GetComponent<Tilemap>();

            Vector3Int up = new Vector3Int(1, 0, 0);
            Vector3Int down = new Vector3Int(-1, 0, 0);
            Vector3Int left = new Vector3Int(0, 1, 0);
            Vector3Int right = new Vector3Int(0, -1, 0);

            this.selectedPatternTilePositions = new Dictionary<string, Dictionary<string, List<Vector3Int>>>();

            foreach (var card in activeEntity.Hand.Cards)
            {
                if (card.InstanceId ==  gameObject.GetComponentInChildren<Card>().GetId()) {
                    foreach(var entry in card.Action.Pattern)
                    {
                        if(!selectedPatternTilePositions.ContainsKey(card.Action.Id))
                        {
                            selectedPatternTilePositions.Add(card.Action.Id, new Dictionary<string, List<Vector3Int>> { });
                        }
                        if (!selectedPatternTilePositions[card.Action.Id].ContainsKey("up"))
                        {
                            selectedPatternTilePositions[card.Action.Id].Add("up", new List<Vector3Int>());
                        }
                        if (!selectedPatternTilePositions[card.Action.Id].ContainsKey("down"))
                        {
                            selectedPatternTilePositions[card.Action.Id].Add("down", new List<Vector3Int>());
                        }
                        if (!selectedPatternTilePositions[card.Action.Id].ContainsKey("left"))
                        {
                            selectedPatternTilePositions[card.Action.Id].Add("left", new List<Vector3Int>());
                        }
                        if (!selectedPatternTilePositions[card.Action.Id].ContainsKey("right"))
                        {
                            selectedPatternTilePositions[card.Action.Id].Add("right", new List<Vector3Int>());
                        }

                        Vector3Int offsetVector = new Vector3Int(0, 0, 0);
                        
                        if(entry.Offset != null) {
                            foreach(var offset in entry.Offset) {
                                switch(offset.Direction) {
                                case Deviant.Direction.Up:
                                    for (int i = 0; i < offset.Distance; i++)
                                    {
                                        offsetVector = offsetVector + up;
                                    }
                                    break; 
                                case Deviant.Direction.Down:
                                    for (int i = 0; i < offset.Distance; i++)
                                    {
                                        offsetVector = offsetVector + down;
                                    }
                                    break;
                                case Deviant.Direction.Left:
                                    for (int i = 0; i < offset.Distance; i++)
                                    {
                                        offsetVector = offsetVector + left;
                                    }
                                    break;
                                case Deviant.Direction.Right:
                                    for (int i = 0; i < offset.Distance; i++)
                                    {
                                        offsetVector = offsetVector + right;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            }
                        }

                        Vector3Int offsetStart = offsetVector;

                        switch (entry.Direction) {
                            case Deviant.Direction.Up:
                                for (int i = 0; i < entry.Distance; i++)
                                {
                                    offsetStart += up;

                                    if (this.selected == true) {
                                        battleFieldOverlayTilemap.SetTile(offsetStart + entityCellLocation, null);
                                    } else {
                                        battleFieldOverlayTilemap.SetTile(offsetStart + entityCellLocation, selectedTile);
                                        selectedPatternTilePositions[card.Action.Id]["up"].Add(offsetStart + entityCellLocation);
                                    }

                                    offsetStart += up;
                                }
                                break; 
                            case Deviant.Direction.Down:
                                for (int i = 0; i < entry.Distance; i++)
                                {
                                    if(this.selected == true) {
                                        battleFieldOverlayTilemap.SetTile(offsetStart + entityCellLocation, null);
                                    } else {
                                        battleFieldOverlayTilemap.SetTile(offsetStart + entityCellLocation, selectedTile);
                                        selectedPatternTilePositions[card.Action.Id]["down"].Add(offsetStart + entityCellLocation);
                                    }

                                    offsetStart += down;
                                }
                                break;
                            case Deviant.Direction.Left:
                                for (int i = 0; i < entry.Distance; i++)
                                {
                                    if (this.selected == true) {
                                        battleFieldOverlayTilemap.SetTile(offsetStart + entityCellLocation, null);
                                    } else {
                                        battleFieldOverlayTilemap.SetTile(offsetStart + entityCellLocation, selectedTile);
                                        selectedPatternTilePositions[card.Action.Id]["left"].Add(offsetStart + entityCellLocation);
                                    }
                                    offsetStart += left;

                                }
                                break;
                            case Deviant.Direction.Right:
                                for (int i = 0; i < entry.Distance; i++)
                                {
                                    if (this.selected == true) {
                                        battleFieldOverlayTilemap.SetTile(offsetStart + entityCellLocation, null);
                                    } else {
                                        battleFieldOverlayTilemap.SetTile(offsetStart + entityCellLocation, selectedTile);
                                        selectedPatternTilePositions[card.Action.Id]["right"].Add(offsetStart + entityCellLocation);
                                    }

                                    offsetStart += right;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if(this.selected == true) {
                var animation = entity.transform.gameObject.GetComponentInChildren<Animator>();
                entity.transform.gameObject.GetComponentInChildren<Animator>().Play("Warrior-StopAttack");
                this.selected = false;

                foreach(var action in this.selectedPatternTilePositions)
                {
                    foreach(var pattern in this.selectedPatternTilePositions[action.Key])
                    {
                        foreach(var tileLocation in this.selectedPatternTilePositions[action.Key][pattern.Key])
                        {
                            battleFieldOverlayTilemap.SetTile(tileLocation, null);
                        }
                    }
                }
            } else {
                var animation = entity.transform.gameObject.GetComponentInChildren<Animator>();
                entity.transform.gameObject.GetComponentInChildren<Animator>().Play("Warrior-Attack");

                this.selected = true;
            }
        }       
    }

	public void setSprite(string assetId, string cardType)
	{
        Sprite newSprite = Resources.Load($"Art/Sprites/Deck/Card/{cardType}/{assetId}", typeof(Sprite)) as Sprite;
		entity.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = newSprite;
	}

    void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }
}
