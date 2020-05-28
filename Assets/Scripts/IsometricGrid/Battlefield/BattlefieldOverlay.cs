using Deviant;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattlefieldOverlay : MonoBehaviour
{
    private EncounterState encounterStateRef = default;
    private Deviant.Tiles overlayTiles = default;

    // Start is called before the first frame update
    void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    // Update is called once per frame
    async void Update()
    {

        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.PlayerId = encounterStateRef.GetPlayerId();
        encounterRequest.GetEncounterState = true;

        await encounterStateRef.UpdateEncounterAsync(encounterRequest);

        UnityEngine.Tilemaps.Tile currentTileAsset = Resources.Load<UnityEngine.Tilemaps.Tile>("Art/Tiles/select_0002");
        Tilemap tilemap = this.GetComponent<Tilemap>();

        if (encounterStateRef.GetEncounter().Board.OverlayTiles != null)
        {
            var newTiles = encounterStateRef.GetEncounter().Board.OverlayTiles;
            tilemap.ClearAllTiles();

            foreach (var tile in newTiles)
            {
                UnityEngine.Tilemaps.Tile newTileAsset = Resources.Load<UnityEngine.Tilemaps.Tile>($"Art/Tiles/{tile.Id}");
                tilemap.SetTile(new Vector3Int(tile.X, tile.Y, 0), currentTileAsset);
                tilemap.RefreshTile(new Vector3Int(tile.X, tile.Y, 0));
            }
        }
    }
};
