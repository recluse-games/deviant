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

        UnityEngine.Tilemaps.Tile currentTileAsset = Resources.Load<UnityEngine.Tilemaps.Tile>("Art/Tiles/select_0002");
        Tilemap tilemap = this.GetComponent<Tilemap>();
        var entities = GameObject.FindGameObjectsWithTag("entity_friendly");
        Vector3Int tilemapOrigin = tilemap.origin;
        Vector3Int tilemapSize = tilemap.size;

        var newTiles = encounterStateRef.GetEncounter().Board.OverlayTiles;

        foreach(var tile in newTiles.Tiles_)
        {
            UnityEngine.Tilemaps.Tile newTileAsset = Resources.Load<UnityEngine.Tilemaps.Tile>($"Art/Tiles/{newTiles.Tiles_[y].Tiles[x].Id}");
            Debug.Log("X/Y: " + x + y + "tile: " + newTiles.Tiles_[tile.y].Tiles[tile.x].Id);
            tilemap.SetTile(new Vector3Int(x, y, 0), newTileAsset);
            tilemap.RefreshTile(new Vector3Int(x, y, 0));
        }

        for (int x = tilemap.origin.x; x < tilemap.size.x; x++)
        {
            for (int y = tilemap.origin.y; y < tilemap.size.y; y++)
            {
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
                                }
                            }

                            for (int columnTest = y; columnTest < tilemap.size.y; columnTest++)
                            {
                                if (tilemap.GetTile(new Vector3Int(x, columnTest, 0)))
                                {
                                    tilemap.SetTile(new Vector3Int(x, columnTest, 0), currentTileAsset);
                                    tilemap.RefreshTile(new Vector3Int(x, columnTest, 0));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
};
