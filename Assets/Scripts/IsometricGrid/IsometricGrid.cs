using Deviant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityScript.Scripting.Pipeline;

public class IsometricGrid : MonoBehaviour
{
    public EncounterState encounterStateRef = default;

    // Movement speed in units per second.
    private float speed = 10.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private CardPrefab selectedCard = default;

    private Vector3Int previousTransform = new Vector3Int(1, 1, 1);

    private GameObject activeEntityObject = default;
    
    private string previousCursorLocation = default;

    public void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    public void Update()
    {
        UpdateSelectedCard();

        if (encounterStateRef.GetEncounter().ActiveEntity.OwnerId == "0001")
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(selectedCard.GetSelectedTilePositions().Count > 0)
                {
                    Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
                    encounterRequest.PlayerId = "0001";
                    encounterRequest.Encounter = encounterStateRef.encounter;
                    encounterRequest.EntityActionName = Deviant.EntityActionNames.Move;

                    foreach (var action in selectedCard.GetSelectedTilePositions())
                    {
                        foreach (var pattern in selectedCard.GetSelectedTilePositions()[action.Key])
                        {
                            foreach (var tileLocation in selectedCard.GetSelectedTilePositions()[action.Key][pattern.Key])
                            {
                                Tilemap test = GameObject.Find($"/IsometricGrid/BattlefieldOverlay").GetComponent<BattlefieldOverlay>().GetComponent<Tilemap>();
                                test.SetTile(tileLocation, null);
                            }
                        }
                    }
                }

                GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
                Tilemap overlay = this.transform.Find("BattlefieldOverlay").GetComponent<Tilemap>();

                var entityObjects = FindObjectsOfType<Entity>();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
                Vector3Int position = gridLayout.WorldToCell(worldPoint);

                foreach (Entity entity in entityObjects)
                {
                    foreach (Vector3Int location in entity.validTiles)
                    {
                        if (position.Equals(location) == true)
                        {
                            Vector3 startingPos = entity.transform.parent.position;
                            this.updatePlayerPosition(overlay.WorldToCell(startingPos).y, overlay.WorldToCell(startingPos).x, position.x, position.y);

                            startTime = Time.time;

                            journeyLength = Vector3.Distance(entity.transform.parent.position, overlay.GetCellCenterWorld(position));

                            // Distance moved equals elapsed time times speed..
                            float distCovered = (Time.time - startTime) * speed;

                            // Fraction of journey completed equals current distance divided by total distance.
                            float fractionOfJourney = distCovered / journeyLength;

                            startTime += Time.deltaTime * 100f;
                            // Set our position as a fraction of the distance between the markers.
                            entity.transform.parent.position = Vector3.Lerp(startingPos, overlay.GetCellCenterWorld(position), startTime);

                            entity.cleanTiles();
                            break;
                        };
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                var activeEntity = encounterStateRef.GetEncounter().ActiveEntity;
                activeEntityObject = GameObject.Find($"/entity_{activeEntity.Id}");

                var animation = activeEntityObject.transform.gameObject.GetComponentInChildren<Animator>();
                activeEntityObject.transform.gameObject.GetComponentInChildren<Animator>().Play("Warrior-StopAttack");

                var currentCards = GameObject.FindGameObjectsWithTag("hand");

                foreach (var currentCard in currentCards)
                {
                    if (currentCard.GetComponent<CardPrefab>().GetSelected() == true)
                    {
                        currentCard.GetComponent<CardPrefab>().SetSelected(false);
                        var selectedPatternTilePositions = currentCard.GetComponent<CardPrefab>().GetSelectedTilePositions();


                        foreach (var action in selectedPatternTilePositions)
                        {
                            foreach (var pattern in selectedPatternTilePositions[action.Key])
                            {
                                foreach (var tileLocation in selectedPatternTilePositions[action.Key][pattern.Key])
                                {
                                    Tilemap test = GameObject.Find($"/IsometricGrid/BattlefieldOverlay").GetComponent<BattlefieldOverlay>().GetComponent<Tilemap>();
                                    test.SetTile(tileLocation, null);
                                }
                            }
                        }
                    }
                }

                GameObject.Find($"/UI").GetComponent<UI>().ResetRotation();

            }
        }
    }

    private void UpdateSelectedCard()
    {
        var activeEntity = encounterStateRef.GetEncounter().ActiveEntity;
        activeEntityObject = GameObject.Find($"/entity_{activeEntity.Id}");

        var animation = activeEntityObject.transform.gameObject.GetComponentInChildren<Animator>();
        activeEntityObject.transform.gameObject.GetComponentInChildren<Animator>().Play("Warrior-StopAttack");

        var currentCards = GameObject.FindGameObjectsWithTag("hand");

        foreach (var currentCard in currentCards)
        {
            if (currentCard.GetComponent<CardPrefab>().GetSelected() == true)
            {
                this.selectedCard = currentCard.GetComponent<CardPrefab>();
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
        encounterRequest.PlayerId = "0001";
        encounterRequest.Encounter = encounterStateRef.encounter;
        encounterRequest.EntityActionName = Deviant.EntityActionNames.Move;
        encounterRequest.EntityMoveAction = entityMoveAction;


        await encounterStateRef.UpdateEncounterAsync(encounterRequest);
    }
}
