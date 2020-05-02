using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using SimpleJSON;

public class CardPrefab: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	Transform entity = default;
    private bool mouse_over = false;

    private bool selected = false;
    private bool visable = false;
    public EncounterState encounterStateRef = default;

    public void SetVisability(bool visability) {
        this.visable = visability;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.visable == true) {
            this.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            this.gameObject.transform.position = this.gameObject.transform.position + new Vector3(0, 0, 5);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        // Retrieve the Current Encounter From Shared State.
		Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
		Deviant.Board board = encounterState.Board;
        Deviant.Entity activeEntity = encounterState.ActiveEntity;

        if(this.visable == true) {
            GameObject entity = GameObject.Find("entity_" + activeEntity.Id);;
        
            GameObject isometricGameGrid = GameObject.Find("IsometricGrid");
            GridLayout isometricGameGridLayout = isometricGameGrid.transform.GetComponent<GridLayout>();
            Vector3Int entityCellLocation = isometricGameGridLayout.WorldToCell(entity.transform.position);

            Tile selectedTile = Resources.Load<Tile>("Art/Tiles/select_0000");
            GameObject battleFieldOverlay = GameObject.Find("BattlefieldOverlay");
            Tilemap battleFieldOverlayTilemap = battleFieldOverlay.GetComponent<Tilemap>();

            Vector3Int up = new Vector3Int(1, 0, 0);
            Vector3Int down = new Vector3Int(-1, 0, 0);
            Vector3Int left = new Vector3Int(0, 1, 0);
            Vector3Int right = new Vector3Int(0, -1, 0);

            foreach (var card in activeEntity.Hand.Cards)
            {
                if(card.Id ==  this.name) {
                    foreach(var entry in card.Action.Pattern)
                    {
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
                                    Debug.Log("INVALID DIRECTION");
                                    break;
                            }
                            }
                        }

                        Vector3Int offsetStart = entityCellLocation += offsetVector;

                        switch(entry.Direction) {
                            case Deviant.Direction.Up:
                                for (int i = 0; i < entry.Distance; i++)
                                {
                                    if(this.selected == true) {
                                        battleFieldOverlayTilemap.SetTile(offsetStart += up, null);
                                    } else {
                                        battleFieldOverlayTilemap.SetTile(offsetStart += up, selectedTile);
                                    }
                                }
                                break; 
                            case Deviant.Direction.Down:
                                for (int i = 0; i < entry.Distance; i++)
                                {
                                    if(this.selected == true) {
                                        battleFieldOverlayTilemap.SetTile(offsetStart += down, null);
                                    } else {
                                        battleFieldOverlayTilemap.SetTile(offsetStart += down, selectedTile);
                                    }
                                }
                                break;
                            case Deviant.Direction.Left:
                                for (int i = 0; i < entry.Distance; i++)
                                {
                                    if(this.selected == true) {
                                        battleFieldOverlayTilemap.SetTile(offsetStart += left, null);
                                    } else {
                                        battleFieldOverlayTilemap.SetTile(offsetStart += left, selectedTile);
                                    }
                                }
                                break;
                            case Deviant.Direction.Right:
                                for (int i = 0; i < entry.Distance; i++)
                                {
                                    if(this.selected == true) {
                                        battleFieldOverlayTilemap.SetTile(offsetStart += right, null);
                                    } else {
                                        battleFieldOverlayTilemap.SetTile(offsetStart += right, selectedTile);
                                    }
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
        } else {
            var animation = entity.transform.gameObject.GetComponentInChildren<Animator>();
            entity.transform.gameObject.GetComponentInChildren<Animator>().Play("Warrior-Attack");
            this.selected = true;
        }
        }       
    }

	public void setSprite()
	{
        Sprite newSprite = Resources.Load("Art/Sprites/Deck/Card/Back/back_0000", typeof(Sprite)) as Sprite;
		entity.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = newSprite;
	}
}
