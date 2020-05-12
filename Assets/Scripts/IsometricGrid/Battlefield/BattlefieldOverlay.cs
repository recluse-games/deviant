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
        Deviant.Tiles updatedOverlayTiles = new Deviant.Tiles();

        UnityEngine.Tilemaps.Tile currentTileAsset = Resources.Load<UnityEngine.Tilemaps.Tile>("Art/Tiles/select_0002");
        Tilemap tilemap = this.GetComponent<Tilemap>();
        var entities = GameObject.FindGameObjectsWithTag("entity_friendly");
        Vector3Int tilemapOrigin = tilemap.origin;
        Vector3Int tilemapSize = tilemap.size;

        for (int x = tilemap.origin.x; x < tilemap.size.x; x++)
        {
            Deviant.TilesRow newTilesRow = new Deviant.TilesRow();

            for (int y = tilemap.origin.y; y < tilemap.size.y; y++)
            {
                Deviant.Tile newTile = new Deviant.Tile();

                if (x < 0 || x > 8 || y < 0 || y > 8)
                {
                    if (tilemap.GetTile(new Vector3Int(x, y, 0)))
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), currentTileAsset);
                        tilemap.RefreshTile(new Vector3Int(x, y, 0));
                    }
                }

                // Need to support positive spells as well as attacks too.
                foreach(var entity in entities)
                {
                    if(tilemap.WorldToCell(entity.transform.position).x == x && tilemap.WorldToCell(entity.transform.position).y == y)
                    {
                        if (tilemap.GetTile(new Vector3Int(x, y, 0)))
                        {
                            for (int rowTest = x; rowTest < tilemap.size.x; rowTest++)
                            {
                                if (tilemap.GetTile(new Vector3Int(rowTest, y, 0)))
                                {
                                    tilemap.SetTile(new Vector3Int(rowTest, y, 0), currentTileAsset);
                                    tilemap.RefreshTile(new Vector3Int(rowTest, y, 0));
                                    newTile.Id = "select_0002";
                                }
                            }

                            for (int columnTest = y; columnTest < tilemap.size.y; columnTest++)
                            {
                                if (tilemap.GetTile(new Vector3Int(x, columnTest, 0)))
                                {
                                    tilemap.SetTile(new Vector3Int(x, columnTest, 0), currentTileAsset);
                                    tilemap.RefreshTile(new Vector3Int(x, columnTest, 0));
                                    newTile.Id = "select_0002";
                                }
                            }
                        }
                    }
                }

                newTilesRow.Tiles.Add(newTile);
            }
        }

        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.EntityTargetAction = new EntityTargetAction();
        encounterRequest.EntityTargetAction.Tiles = updatedOverlayTiles;

        await encounterStateRef.UpdateEncounterAsync(encounterRequest);
    }
};
