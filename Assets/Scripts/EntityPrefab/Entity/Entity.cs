using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Entity : MonoBehaviour
{
	public EncounterState encounterStateRef = default;

    public bool moveSelection = false;

	public List<Vector3Int> validTiles = new List<Vector3Int>();

	private Vector3Int previousCell = default;

	public void Start() {
		encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
	}

	public void cleanTiles() {
		encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();

		GameObject tilemapGameObject = GameObject.Find("BattlefieldOverlay");
		Tilemap tilemap = tilemapGameObject.GetComponent<Tilemap>();

		foreach(Vector3Int location in this.validTiles) {
			tilemap.SetTile(location, null);
		}

		tilemap.SetTile(previousCell, null);

		this.transform.gameObject.GetComponentInChildren<Animator>().Play("Warrior-Idle");

		this.validTiles = new List<Vector3Int>();
	}

  	public void OnMouseDown()
    {
		// Retrieve the Current Encounter From Shared State.
		Deviant.Encounter encounterState = encounterStateRef.GetEncounter();

		if(encounterState.ActiveEntity.OwnerId == "0001")
		{
			Deviant.Board board = encounterState.Board;

			this.moveSelection = true;
			Deviant.Entity activeEntity = encounterState.ActiveEntity;

			this.transform.gameObject.GetComponentInChildren<Animator>().Play("Warrior-Walk");

			GameObject overLayGrid = GameObject.Find("IsometricGrid");
			GridLayout gridLayout = overLayGrid.transform.GetComponent<GridLayout>();

			Vector3Int cellLocation = gridLayout.WorldToCell(this.transform.position);

			Tile myTile = Resources.Load<Tile>("Art/Tiles/select_0001");
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
			previousCell = cellLocation;
			validTiles.Add(upThing);
			validTiles.Add(downThing);
			validTiles.Add(leftThing);
			validTiles.Add(rightThing);
		}
	}
}
