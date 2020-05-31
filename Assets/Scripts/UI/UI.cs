using UnityEngine;
using System.Linq;
using Deviant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Net;

public class UI : MonoBehaviour
{
    [SerializeField]
    float deckX, deckY, deckZ;

    [SerializeField]
    string playerId = default;

    [SerializeField]
    CardPrefab cardPrefab = default;

    private Vector3Int previousEntityLocation = default;
    private string previousRotation = "down";
    private EncounterState encounterStateRef = default;
    private GameObject activeEntityObject = default;
    private Camera cam;

    public void ResetRotation()
    {
        previousRotation = "down";
    }

    public string GetRotation()
    {
        return this.previousRotation;
    }

    async void UpdateThings()
    {
        if (activeEntityObject != default)
        {
            var activeEntityLocationWorld = activeEntityObject.transform.position;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePosWorld = ray.GetPoint(-ray.origin.z / ray.direction.z);
            GridLayout gridLayout = this.transform.GetComponent<GridLayout>();
            Tilemap overlay = GameObject.Find("BattlefieldOverlay").GetComponent<Tilemap>();
            Vector3Int mousePos = overlay.WorldToCell(mousePosWorld);
            Vector3Int activeEntityLocation = overlay.WorldToCell(activeEntityLocationWorld);

            if (previousRotation != "up")
            {
                if (mousePos.y > activeEntityLocation.y && mousePos.x > activeEntityLocation.x)
                {
                    var currentCards = GameObject.FindGameObjectsWithTag("hand");

                    foreach (var currentCard in currentCards)
                    {
                        if (currentCard.GetComponent<CardPrefab>().GetSelected() == true)
                        {
                            await currentCard.GetComponent<CardPrefab>().UpdateSelectedTiles("up", previousRotation, activeEntityLocation, previousEntityLocation);
                            previousRotation = "up";
                            previousEntityLocation = activeEntityLocation;
                        }
                    }

                }
            }
            if (previousRotation != "down")
            {
                if (mousePos.y < activeEntityLocation.y && mousePos.x < activeEntityLocation.x)
                {
                    var currentCards = GameObject.FindGameObjectsWithTag("hand");

                    foreach (var currentCard in currentCards)
                    {
                        if (currentCard.GetComponent<CardPrefab>().GetSelected() == true)
                        {
                            await currentCard.GetComponent<CardPrefab>().UpdateSelectedTiles("down", previousRotation, activeEntityLocation, previousEntityLocation);
                            previousRotation = "down";
                            previousEntityLocation = activeEntityLocation;
                        }
                    }


                }
            }

            if (previousRotation != "left")
            {
                if (mousePos.y > activeEntityLocation.y && mousePos.x < activeEntityLocation.x)
                {
                    var currentCards = GameObject.FindGameObjectsWithTag("hand");

                    foreach (var currentCard in currentCards)
                    {
                        if (currentCard.GetComponent<CardPrefab>().GetSelected() == true)
                        {
                            await currentCard.GetComponent<CardPrefab>().UpdateSelectedTiles("left", previousRotation, activeEntityLocation, previousEntityLocation);
                            previousRotation = "left";
                            previousEntityLocation = activeEntityLocation;
                        }
                    }

                }
            }

            if (previousRotation != "right")
            {
                if (mousePos.y < activeEntityLocation.y && mousePos.x > activeEntityLocation.x)
                {
                    var currentCards = GameObject.FindGameObjectsWithTag("hand");

                    foreach (var currentCard in currentCards)
                    {
                        if (currentCard.GetComponent<CardPrefab>().GetSelected() == true)
                        {
                            await currentCard.GetComponent<CardPrefab>().UpdateSelectedTiles("right", previousRotation, activeEntityLocation, previousEntityLocation);
                            previousRotation = "right";
                            previousEntityLocation = activeEntityLocation;
                        }
                    }
                }
            }
        }
    }

    private void CreateHand(Deviant.Entity activeEntity)
    {
        int x = -250;
        int y = -170;

        foreach (var card in activeEntity.Hand.Cards)
        {
            GameObject existingHandCardGameObject = GameObject.Find("hand_" + card.InstanceId);

            if (existingHandCardGameObject != null)
            {
                x += 70;
                continue;
            }

            CardPrefab newCard = Instantiate(cardPrefab);
            newCard.transform.localPosition = new Vector3Int(x, y, 0);
            newCard.transform.SetParent(transform, false);
            newCard.setSprite(card.Id, "Front");
            newCard.transform.gameObject.tag = "hand";
            newCard.transform.gameObject.name = "hand_" + card.InstanceId;
            newCard.GetComponentInChildren<Card>().SetId(card.InstanceId);
            newCard.SetId(card.Id);
            newCard.SetInstanceId(card.InstanceId);
            newCard.SetVisability(true);
            x += 70;
        }

        foreach (var deckCardObject in GameObject.FindGameObjectsWithTag("deck"))
        {
            foreach (var handCardObject in GameObject.FindGameObjectsWithTag("hand"))
            {
                if (deckCardObject.GetComponent<CardPrefab>().GetInstanceId() == handCardObject.GetComponent<CardPrefab>().GetInstanceId())
                {
                    Destroy(deckCardObject);
                }
            }
        }

        // Clean up any removed cards
        var allExistingCards = GameObject.FindObjectsOfType<CardPrefab>();
        List<string> convertCardsToIds = activeEntity.Hand.Cards.Select(card => card.InstanceId).ToList();
        List<string> convertGameObjectsToIds = allExistingCards.Select(cardPrefab => cardPrefab.GetInstanceId()).ToList();
        var cardsIdsToDestroy = convertGameObjectsToIds.Except(convertCardsToIds);

        foreach (var id in cardsIdsToDestroy)
        {
            Debug.Log("Time to destory: " + "hand_" + id);
            GameObject existingDeckCardGameObject = GameObject.Find("hand_" + id);
            Destroy(existingDeckCardGameObject);
        }
    }

    private void CreateDeck(Deviant.Entity activeEntity)
    {
        int deckx = -350;
        int decky = -170;

        foreach (var card in activeEntity.Deck.Cards)
        {
            GameObject existingDeckCardGameObject = GameObject.Find("deck_" + card.InstanceId);

            if (existingDeckCardGameObject == null)
            {
                CardPrefab newDeckCardGameObject = Instantiate(cardPrefab);
                newDeckCardGameObject.setSprite(card.BackId, "Back");
                newDeckCardGameObject.transform.localPosition = new Vector3Int(deckx, decky, 0);
                newDeckCardGameObject.transform.SetParent(transform, false);
                newDeckCardGameObject.transform.gameObject.tag = "deck";
                newDeckCardGameObject.transform.gameObject.name = "deck_" + card.InstanceId;
                newDeckCardGameObject.SetId(card.Id);
                newDeckCardGameObject.SetInstanceId(card.InstanceId);
                newDeckCardGameObject.GetComponentInChildren<Card>().SetId(card.InstanceId);
            }
        }

        // Clean up any removed cards
        var allExistingCards = GameObject.FindObjectsOfType<CardPrefab>();
        List<string> convertCardsToIds = activeEntity.Deck.Cards.Select(card => card.InstanceId).ToList();
        List<string> convertGameObjectsToIds = allExistingCards.Select(cardPrefab => cardPrefab.GetInstanceId()).ToList();
        var cardsIdsToDestroy = convertGameObjectsToIds.Except(convertCardsToIds);

        foreach (var id in cardsIdsToDestroy)
        {
            GameObject existingDeckCardGameObject = GameObject.Find("deck_" + id);
            Destroy(existingDeckCardGameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    void Update()
    {
        // Retrieve the Current Encounter From Shared State.
        Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
        var activeEntity = encounterState.ActiveEntity;
        activeEntityObject = GameObject.Find($"/entity_{activeEntity.Id}");

        // 0000 should be replaced with the current players ID
        if (encounterStateRef.GetPlayerId() == activeEntity.OwnerId)
        {
            CreateDeck(activeEntity);
            CreateHand(activeEntity);
        }

        if (encounterStateRef.GetEncounter().Board.OverlayTiles != null)
        {
            UpdateThings();
        }
    }
}