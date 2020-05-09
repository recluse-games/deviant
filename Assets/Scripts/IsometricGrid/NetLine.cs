using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetLine : MonoBehaviour
{
    private EncounterState encounterStateRef = default;
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        Sprite netLine = Resources.Load<Sprite>("Art/Sprites/IsometricGrid/NetLine/netline_0000");
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
        Deviant.Encounter encounterState = encounterStateRef.GetEncounter();

        Tilemap tilemap = GameObject.Find("/IsometricGrid/Battlefield").GetComponent<Tilemap>();

        Vector3Int tilemapOrigin = tilemap.origin;
        Vector3Int tilemapSize = tilemap.size;

        for (int x = tilemap.origin.x; x < tilemap.size.x; x++)
        {
            if(x == 3)
            {
                for (int y = 0; y < 8; y++)
                {
                    var netline = new GameObject($"netline_{y}");
                    netline.transform.parent = this.transform;

                    SpriteRenderer renderer = netline.AddComponent<SpriteRenderer>();
                    renderer.material = material;
                    renderer.sprite = netLine;

                    Vector3 place = tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    netline.transform.position = new Vector3(place.x, place.y, place.z);
                    renderer.sortingOrder = 3;
                }
            }
        }
   
    }

    // Update is called once per frame
    void Update()
    {
    }
}
