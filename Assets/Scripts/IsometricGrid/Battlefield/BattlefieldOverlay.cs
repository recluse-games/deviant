﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattlefieldOverlay : MonoBehaviour
{
    private EncounterState encounterStateRef = default;

    // Start is called before the first frame update
    void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    // Update is called once per frame
    void Update()
    {
        Tile currentTileAsset = Resources.Load<Tile>("Art/Tiles/select_0002");
        Tilemap tilemap = this.GetComponent<Tilemap>();
        var entities = GameObject.FindGameObjectsWithTag("entity_friendly");
        Vector3Int tilemapOrigin = tilemap.origin;
        Vector3Int tilemapSize = tilemap.size;

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