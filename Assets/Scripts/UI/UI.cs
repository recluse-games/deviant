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
    private EncounterState encounterStateRef = default;
    private GameObject activeEntityObject = default;
    private Camera cam;

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
        }
    }
}