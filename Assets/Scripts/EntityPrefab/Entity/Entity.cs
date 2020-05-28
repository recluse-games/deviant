using Deviant;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityAsync;


public class Entity : MonoBehaviour
{
	GameObject selectionArrow;

	public string id = default;
	public bool moveSelection = false;

	public List<Vector3Int> validTiles = new List<Vector3Int>();

	private Vector3Int _previousEntityCellLocation = default;
	private EncounterState _encounterStateComponentReference = default;

	public void Start() {
		_encounterStateComponentReference = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
	}

	public string GetId()
	{
		return this.id;
	}

	async public Task<bool> cleanTiles() {
		GameObject tilemapGameObject = GameObject.Find("BattlefieldOverlay");
		Tilemap tilemap = tilemapGameObject.GetComponent<Tilemap>();

		foreach(Vector3Int location in this.validTiles) {
			tilemap.SetTile(location, null);
		}

		tilemap.SetTile(_previousEntityCellLocation, null);

		// Update the Server With New Entity Animation State
		Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
		encounterRequest.EntityStateAction = new Deviant.EntityStateAction();
		encounterRequest.EntityStateAction.Id = this.id;
		encounterRequest.EntityStateAction.State = Deviant.EntityStateNames.Idle;
		await _encounterStateComponentReference.UpdateEncounterAsync(encounterRequest);

		this.validTiles = new List<Vector3Int>();
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

	async public void OnMouseDown()
    {
		// Retrieve the Current Encounter From Shared State.
		Deviant.Encounter encounterState = _encounterStateComponentReference.GetEncounter();

		if(encounterState.ActiveEntity.OwnerId == _encounterStateComponentReference.GetPlayerId() && encounterState.ActiveEntity.Id == this.id && encounterState.ActiveEntity.Ap > 0)
		{
			Deviant.Board board = encounterState.Board;

			this.moveSelection = true;
			Deviant.Entity activeEntity = encounterState.ActiveEntity;

			// Update the Server With New Entity Animation State
			Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
			encounterRequest.EntityStateAction = new Deviant.EntityStateAction();
			encounterRequest.EntityStateAction.Id = this.id;
			encounterRequest.EntityStateAction.State = Deviant.EntityStateNames.Moving;
			await _encounterStateComponentReference.UpdateEncounterAsync(encounterRequest);

			GameObject overLayGrid = GameObject.Find("IsometricGrid");
			GridLayout gridLayout = overLayGrid.transform.GetComponent<GridLayout>();

			Vector3Int cellLocation = gridLayout.WorldToCell(this.transform.position);

			UnityEngine.Tilemaps.Tile myTile = Resources.Load<UnityEngine.Tilemaps.Tile>("Art/Tiles/select_0001");
			GameObject tilemapGameObject = GameObject.Find("BattlefieldOverlay");
			Tilemap tilemap = tilemapGameObject.GetComponent<Tilemap>();

			Vector3Int up = new Vector3Int(1, 0, 0);
			Vector3Int down = new Vector3Int(-1, 0, 0);
			Vector3Int left = new Vector3Int(0, 1, 0);
			Vector3Int right = new Vector3Int(0, -1, 0);

			tilemap.SetTile(cellLocation, myTile);
			tilemap.SetTile(cellLocation + up, myTile);
			tilemap.SetTile(cellLocation + down, myTile);
			tilemap.SetTile(cellLocation + left, myTile);
			tilemap.SetTile(cellLocation + right, myTile);

			Vector3Int upThing = cellLocation + up;
			Vector3Int downThing = cellLocation + down;
			Vector3Int leftThing = cellLocation + left;
			Vector3Int rightThing = cellLocation + right;

			// Load All Tiles Into Entity State for Future Cleaning
			_previousEntityCellLocation = cellLocation;
			validTiles.Add(upThing);
			validTiles.Add(downThing);
			validTiles.Add(leftThing);
			validTiles.Add(rightThing);
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

			var topOfSprite = new Vector3(bounds.extents.x/2 - (float).1, bounds.extents.y * (float)2.3, 0.0f);
			selectionArrowObj.transform.localPosition = topOfSprite;

			SpriteRenderer rend = selectionArrowObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
			rend.sprite = selectionArrow;
			rend.sortingOrder = 1;
			var newYellowColor = new Color(255f / 255f, 230f / 255f, 88f / 255f);
			rend.color = newYellowColor;
			selectionArrowObj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

			this.selectionArrow = selectionArrowObj;
		} else if (this.selectionArrow)
		{
			var bounds = GetComponent<Entity>().GetComponent<SpriteRenderer>().bounds;

			var topOfSprite = new Vector3(bounds.extents.x/2 - (float).1, bounds.extents.y * (float)2.3, 0.0f);

			this.selectionArrow.transform.localPosition = topOfSprite;
		}

		if (this.selectionArrow && encounterState.ActiveEntity.Id != this.id)
		{
			Destroy(this.selectionArrow);
		}
	}
}
