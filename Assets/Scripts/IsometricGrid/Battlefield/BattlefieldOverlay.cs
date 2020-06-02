using Deviant;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattlefieldOverlay : MonoBehaviour
{
    private EncounterState encounterStateRef = default;
    private Deviant.Tiles overlayTiles = default;

    void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    void Update()
    {
        Tilemap tilemap = this.GetComponent<Tilemap>();

        tilemap.ClearAllTiles();

        if (encounterStateRef.GetEncounter().Board.OverlayTiles != null)
        {
            var newTiles = encounterStateRef.GetEncounter().Board.OverlayTiles;

            foreach (var tile in newTiles)
            {
                UnityEngine.Tilemaps.Tile newTileAsset = Resources.Load<UnityEngine.Tilemaps.Tile>($"Art/Tiles/{tile.Id}");
                tilemap.SetTile(new Vector3Int(tile.X, tile.Y, 0), newTileAsset);
                tilemap.RefreshTile(new Vector3Int(tile.X, tile.Y, 0));
            }
        }
    }
};
