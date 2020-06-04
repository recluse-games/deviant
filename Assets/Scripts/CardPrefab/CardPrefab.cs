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
using System.Threading.Tasks;


public class CardPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    Transform entity = default;

    private bool mouse_over = false;
    private bool selected = false;
    private bool visable = false;
    public string id = default;
    public string instanceId = default;
    private Deviant.CardType cardType = default;
    public EncounterState encounterStateRef = default;

    private Dictionary<string, Dictionary<string, List<Vector3Int>>> selectedPatternTilePositions = new Dictionary<string, Dictionary< string, List<Vector3Int>>>();

    public string getTileTypeFromCardType(Deviant.CardType type)
    {
        switch (type)
        {
            case Deviant.CardType.Attack:
                return "select_0002";
            case Deviant.CardType.Heal:
                return "select_0001";
            default:
                Debug.Log("Error specified card type does not have assigned tile type.");
                return "select_0000";
        }
    }

    public void SetSelected(bool selected)
    {
        this.selected = selected;
    }

    public void SetId(string id)
    {
        this.id = id;
    }

    public string GetId()
    {
        return this.id;
    }

    public void SetInstanceId(string id)
    {
        this.instanceId = id;
    }
    
    public string GetInstanceId()
    {
        return this.instanceId;
    }

    public void SetVisability(bool visability) {
        this.visable = visability;
    }

    public Dictionary<string, Dictionary<string, List<Vector3Int>>> GetSelectedTilePositions ()
    {
        return this.selectedPatternTilePositions;
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

    public void processTilePatterns(Deviant.Entity activeEntity) {
        GameObject entity = GameObject.Find("entity_" + activeEntity.Id); ;

        GameObject isometricGameGrid = GameObject.Find("IsometricGrid");
        GridLayout isometricGameGridLayout = isometricGameGrid.transform.GetComponent<GridLayout>();
        Vector3Int entityCellLocation = isometricGameGridLayout.WorldToCell(entity.transform.position);

        UnityEngine.Tilemaps.Tile selectedTile = Resources.Load<UnityEngine.Tilemaps.Tile>("Art/Tiles/select_0000");

        Vector3Int up = new Vector3Int(1, 0, 0);
        Vector3Int down = new Vector3Int(-1, 0, 0);
        Vector3Int left = new Vector3Int(0, 1, 0);
        Vector3Int right = new Vector3Int(0, -1, 0);

        this.selectedPatternTilePositions = new Dictionary<string, Dictionary<string, List<Vector3Int>>>();

        foreach (var card in activeEntity.Hand.Cards)
        {
            if (card.InstanceId == gameObject.GetComponentInChildren<Card>().GetId())
            {
                this.cardType = card.Type;

                foreach (var entry in card.Action.Pattern)
                {
                    if (!selectedPatternTilePositions.ContainsKey(card.Action.Id))
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

                    if (entry.Offset != null)
                    {
                        foreach (var offset in entry.Offset)
                        {
                            switch (offset.Direction)
                            {
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

                    switch (entry.Direction)
                    {
                        case Deviant.Direction.Up:
                            for (int i = 0; i < entry.Distance; i++)
                            {
                                selectedPatternTilePositions[card.Action.Id]["up"].Add(offsetStart + entityCellLocation);

                                offsetStart += up;
                            }
                            break;
                        case Deviant.Direction.Down:
                            for (int i = 0; i < entry.Distance; i++)
                            {

                                selectedPatternTilePositions[card.Action.Id]["down"].Add(offsetStart + entityCellLocation);

                                offsetStart += down;
                            }
                            break;
                        case Deviant.Direction.Left:
                            for (int i = 0; i < entry.Distance; i++)
                            {
                                selectedPatternTilePositions[card.Action.Id]["left"].Add(offsetStart + entityCellLocation);
                                offsetStart += left;
                            }
                            break;
                        case Deviant.Direction.Right:
                            for (int i = 0; i < entry.Distance; i++)
                            {
                                selectedPatternTilePositions[card.Action.Id]["right"].Add(offsetStart + entityCellLocation);
                                offsetStart += right;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public async void OnPointerClick(PointerEventData eventData)
    {
        if(this.selected == true) {
            this.selected = false;
        } else {
            this.selected = true;
        }

        // Retrieve the Current Encounter From Shared State.
        Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
        Deviant.Entity activeEntity = encounterState.ActiveEntity;
        GameObject battleFieldOverlay = GameObject.Find("BattlefieldOverlay");
        Tilemap battleFieldOverlayTilemap = battleFieldOverlay.GetComponent<Tilemap>();


        if (activeEntity.OwnerId == encounterStateRef.GetPlayerId())
        {
            if (this.visable == true)
            {
               this.processTilePatterns(activeEntity);

                if (this.selected == true)
                {
                    Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
                    encounterRequest.EntityTargetAction = new EntityTargetAction();

                    foreach (var action in this.selectedPatternTilePositions)
                    {
                        foreach (var pattern in this.selectedPatternTilePositions[action.Key])
                        {
                            foreach (var tileLocation in this.selectedPatternTilePositions[action.Key][pattern.Key])
                            {
                                Deviant.Tile newTile = new Deviant.Tile();
                                newTile.Id = getTileTypeFromCardType(this.cardType);
                                newTile.X = tileLocation.x;
                                newTile.Y = tileLocation.y;

                                encounterRequest.EntityTargetAction.Tiles.Add(newTile);
                            }
                        }
                    }

                    await encounterStateRef.UpdateEncounterAsync(encounterRequest);
                }
                else
                {
                    Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
                    encounterRequest.EntityTargetAction = new EntityTargetAction();
                    encounterRequest.EntityTargetAction.Tiles.Clear();
                    await encounterStateRef.UpdateEncounterAsync(encounterRequest);

                    selected = true;
                }
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
