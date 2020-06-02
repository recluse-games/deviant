﻿using Deviant;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityAsync;

public class IsometricGrid : MonoBehaviour
{
    public EncounterState encounterStateRef = default;

    private CardPrefab selectedCard = default;
    
    public void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }


    private bool validateMovementLocation(Vector3Int newLocation, Deviant.Alignment entityAlignment)
    {
        switch (entityAlignment) {
            case Deviant.Alignment.Friendly:
                if(newLocation.x <= 3 && newLocation.x >= 0 && newLocation.y <= 8 && newLocation.y >= 0)
                {
                    return true;
                } else
                {
                    return false;
                }
            case Deviant.Alignment.Unfriendly:
                if (newLocation.x <= 8 && newLocation.x >= 4 && newLocation.y <= 8 && newLocation.y >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        return false;
    }

    private bool validateEntityActive(Entity selectedEntity, Deviant.Entity activeEntity)
    {
        if(selectedEntity.GetId() == activeEntity.Id)
        {
            return true;
        } else
        {
            return false;
        }
    }
    
    private async Task<bool> ProcessMove()
    {
        GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
        Tilemap battlefield = this.transform.Find("Battlefield").GetComponent<Tilemap>();
        Deviant.Entity activeEntity = encounterStateRef.encounter.ActiveEntity;

        var entityObjects = FindObjectsOfType<Entity>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = gridLayout.WorldToCell(worldPoint);

        foreach (Entity entity in entityObjects)
        {
            if (validateEntityActive(entity, activeEntity) && activeEntity.State == Deviant.EntityStateNames.Moving)
            {
                Vector3 startingPos = entity.transform.parent.position;
                await updatePlayerPosition(battlefield.WorldToCell(startingPos).x, battlefield.WorldToCell(startingPos).y, position.x, position.y);
                break;
            };
        }

        return true;
    }

    private async Task<bool> ProcessAttack()
    {
        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.EntityActionName = Deviant.EntityActionNames.Play;
        encounterRequest.EntityPlayAction = new Deviant.EntityPlayAction();
        encounterRequest.EntityPlayAction.CardId = selectedCard.GetInstanceId();

        foreach (var action in selectedCard.GetSelectedTilePositions())
        {
            foreach (var pattern in selectedCard.GetSelectedTilePositions()[action.Key])
            {
                foreach (var tileLocation in selectedCard.GetSelectedTilePositions()[action.Key][pattern.Key])
                {
                    Tilemap test = GameObject.Find($"/IsometricGrid/BattlefieldOverlay").GetComponent<BattlefieldOverlay>().GetComponent<Tilemap>();

                    Deviant.Play newPlay = new Deviant.Play();
                    newPlay.X = tileLocation.x;
                    newPlay.Y = tileLocation.y;

                    encounterRequest.EntityPlayAction.Plays.Add(newPlay);
                }
            }
        }

        selectedCard.ClearSelectedTiles();
        GameObject.Find($"/UI").GetComponent<UI>().ResetRotation();
        this.selectedCard.SetSelected(false);
        await encounterStateRef.UpdateEncounterAsync(encounterRequest);

        var activeEntity = encounterStateRef.GetEncounter().ActiveEntity;

        // Remove all highlighted tiles.
        Deviant.EncounterRequest encounterOverlayTilesRequest = new Deviant.EncounterRequest();
        encounterOverlayTilesRequest.EntityTargetAction = new Deviant.EntityTargetAction();
        encounterOverlayTilesRequest.EntityTargetAction.Id = activeEntity.Id;
        encounterOverlayTilesRequest.EntityTargetAction.Tiles.Clear();
        await encounterStateRef.UpdateEncounterAsync(encounterOverlayTilesRequest);

        selectedCard = null;
        return true;
    }

    async public void Update()
    {
        UpdateSelectedCard();

        if (encounterStateRef.GetEncounter() != null)
        {
            if (encounterStateRef.GetEncounter().ActiveEntity.OwnerId == encounterStateRef.GetPlayerId())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (selectedCard)
                    {
                        if (selectedCard.GetSelected())
                        {
                            if (selectedCard.GetSelectedTilePositions().Count > 0)
                            {
                                await ProcessAttack();
                            }
                        }
                    }
                    else
                    {
                        await ProcessMove();
                    }
                } else if (Input.GetMouseButtonDown(1))
                {
                    var activeEntity = encounterStateRef.GetEncounter().ActiveEntity;

                    if (selectedCard)
                    {
                        selectedCard.SetSelected(false);
                        var selectedPatternTilePositions = selectedCard.GetSelectedTilePositions();

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

                        selectedCard = null;
                    } else {
                        var entityObjects = FindObjectsOfType<Entity>();

                        foreach (Entity entity in entityObjects)
                        {
                            if (validateEntityActive(entity, activeEntity))
                            {
                                entity.SetIdle();
                                break;
                            };
                        }
                    }

                    Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
                    encounterRequest.EntityTargetAction = new EntityTargetAction();
                    encounterRequest.EntityTargetAction.Tiles.Clear();

                    await encounterStateRef.UpdateEncounterAsync(encounterRequest);
                    GameObject.Find($"/UI").GetComponent<UI>().ResetRotation();

                    // Remove all highlighted tiles.
                    Deviant.EncounterRequest encounterOverlayTilesRequest = new Deviant.EncounterRequest();
                    encounterOverlayTilesRequest.EntityTargetAction = new Deviant.EntityTargetAction();
                    encounterOverlayTilesRequest.EntityTargetAction.Id = activeEntity.Id;
                    encounterOverlayTilesRequest.EntityTargetAction.Tiles.Clear();
                    await encounterStateRef.UpdateEncounterAsync(encounterOverlayTilesRequest);
                }
            }
        }
    }

    private void UpdateSelectedCard()
    {
        var currentCards = GameObject.FindGameObjectsWithTag("hand");

        foreach (var currentCard in currentCards)
        {
            if (currentCard.GetComponent<CardPrefab>().GetSelected())
            {
                this.selectedCard = currentCard.GetComponent<CardPrefab>();
            }
        }
    }



    async public Task<bool> updatePlayerPosition(int startx, int starty, int endx, int endy)
    {
        Deviant.EntityMoveAction entityMoveAction = new Deviant.EntityMoveAction();
        entityMoveAction.StartXPosition = startx;
        entityMoveAction.StartYPosition = starty;
        entityMoveAction.FinalXPosition = endx;
        entityMoveAction.FinalYPosition = endy;

        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.PlayerId = encounterStateRef.GetPlayerId();
        encounterRequest.Encounter = encounterStateRef.encounter;
        encounterRequest.EntityActionName = Deviant.EntityActionNames.Move;
        encounterRequest.EntityMoveAction = entityMoveAction;


        await encounterStateRef.UpdateEncounterAsync(encounterRequest);

        return true;
    }
}
