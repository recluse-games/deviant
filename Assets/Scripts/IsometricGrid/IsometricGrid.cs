using Deviant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IsometricGrid : MonoBehaviour
{
    public EncounterState encounterStateRef = default;

    // Movement speed in units per second.
    private float speed = 10.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    async public void Update() {
		if (Input.GetMouseButtonDown(0))
        {
            GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
            Tilemap overlay = this.transform.Find("BattlefieldOverlay").GetComponent<Tilemap>();

            var entityObjects = FindObjectsOfType<Entity>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = gridLayout.WorldToCell(worldPoint);

             foreach (Entity entity in entityObjects)
             {
                foreach (Vector3Int location in entity.validTiles) {
                    if(position.Equals(location) == true) {
                        Vector3 startingPos = entity.transform.parent.position;
                        this.updatePlayerPosition(overlay.WorldToCell(startingPos).x, overlay.WorldToCell(startingPos).y, location.x, location.y);

                        startTime = Time.time;
                        
                        journeyLength = Vector3.Distance(entity.transform.parent.position, overlay.GetCellCenterWorld(position));

                        // Distance moved equals elapsed time times speed..
                        float distCovered = (Time.time - startTime) * speed;

                        // Fraction of journey completed equals current distance divided by total distance.
                        float fractionOfJourney = distCovered / journeyLength;

                        startTime += Time.deltaTime*100f;
                        // Set our position as a fraction of the distance between the markers.
                        entity.transform.parent.position = Vector3.Lerp(startingPos, overlay.GetCellCenterWorld(position), startTime);

                        entity.cleanTiles();
                        break;
                    };
                }
             }
             
        }
	}

    async public void updatePlayerPosition(int startx, int starty, int endx, int endy)
    {
        Deviant.EntityMoveAction entityMoveAction = new Deviant.EntityMoveAction();
        entityMoveAction.StartXPosition = startx;
        entityMoveAction.StartYPosition = starty;
        entityMoveAction.FinalXPosition = endx;
        entityMoveAction.FinalYPosition = endy;

        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.PlayerId = "0000";
        encounterRequest.Encounter = encounterStateRef.encounter;
        encounterRequest.EntityActionName = Deviant.EntityActionNames.Move;
        encounterRequest.EntityMoveAction = entityMoveAction;


        await encounterStateRef.UpdateEncounterAsync(encounterRequest);
    }
}
