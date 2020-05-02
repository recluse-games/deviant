using UnityEngine;
using UnityEngine.Tilemaps;

public class Battlefield : MonoBehaviour {
	public EncounterState encounterStateRef = default;

	public void Start() {
		encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
	}

	public void Update() {
		// Retrieve the Current Encounter From Shared State.
		Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
		Deviant.Board board = encounterState.Board;

		for (int y = 0; y < board.Tiles.Tiles_.Count; y++) {
			for (int x = 0; x < board.Tiles.Tiles_[y].Tiles.Count; x++) {
				Tile currentTileAsset = Resources.Load<Tile>("Art/Tiles/" + board.Tiles.Tiles_[y].Tiles[x].Id);
				Tilemap tilemap = this.GetComponent<Tilemap>();

				tilemap.SetTile(new Vector3Int(x, y, 0), currentTileAsset);
			}
		}
	}
}