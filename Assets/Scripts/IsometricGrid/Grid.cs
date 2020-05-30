using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    // Movement speed in units per second.
    public float speed = 10.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
            Tilemap overlay = this.transform.Find("Overlay").GetComponent<Tilemap>();

            var entityObjects = FindObjectsOfType<Entity>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int position = gridLayout.WorldToCell(worldPoint);

            foreach (Entity entity in entityObjects)
            {
                Vector3 startingPos = entity.transform.parent.position;
                startTime = Time.time;

                journeyLength = Vector3.Distance(entity.transform.parent.position, overlay.GetCellCenterWorld(position));

                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - startTime) * speed;

                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourney = distCovered / journeyLength;

                startTime += Time.deltaTime * 100f;
                // Set our position as a fraction of the distance between the markers.
                entity.transform.parent.position = Vector3.Lerp(startingPos, overlay.GetCellCenterWorld(position), startTime);

                break;
            };
        }
    }
}
