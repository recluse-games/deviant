using UnityEngine;
using System.Linq;
using Deviant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Net;
using UnityEngine.UI;

namespace Hand
{

    public class Hand : MonoBehaviour
    {
        [SerializeField]
        CardPrefab cardPrefab = default;

        private EncounterState encounterStateRef = default;
        private GameObject activeEntityObject = default;

        public GameObject ParentPanel;

        private void CreateHand(Deviant.Entity activeEntity)
        {
            foreach (var card in activeEntity.Hand.Cards)
            {
                GameObject existingHandCardGameObject = GameObject.Find("hand_" + card.InstanceId);

                if (existingHandCardGameObject != null)
                {
                    continue;
                }

                CardPrefab newCard = Instantiate(cardPrefab);
                newCard.transform.localPosition = new Vector3Int(0, 0, 0);
                newCard.transform.SetParent(transform, false);
                newCard.transform.gameObject.tag = "hand";
                newCard.transform.gameObject.name = "hand_" + card.InstanceId;
                newCard.GetComponentInChildren<Card>().SetId(card.InstanceId);
                newCard.GetComponentInChildren<Card>().transform.localPosition = new Vector3Int(0, 0, 0);
                newCard.SetId(card.Id);
                newCard.SetInstanceId(card.InstanceId);
                newCard.SetVisability(true);
                newCard.GetComponentInChildren<Name>().UpdateValue(card.Title);
                newCard.GetComponentInChildren<Description>().UpdateValue(card.Description);
                newCard.GetComponentInChildren<AP>().UpdateValue(card.Cost.ToString());
                newCard.GetComponentInChildren<Damage>().UpdateValue(card.Damage.ToString());
                newCard.transform.SetParent(newCard.GetComponentInParent<HorizontalLayoutGroup>().transform, true);
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

        // Start is called before the first frame update
        void Start()
        {
            encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
        }

        // Update is called once per frame
        void Update()
        {

            // Retrieve the Current Encounter From Shared State.
            Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
            Deviant.Entity activeEntity = encounterState.ActiveEntity;
            activeEntityObject = GameObject.Find($"/entity_{activeEntity.Id}");

            // 0000 should be replaced with the current players ID
            if (encounterStateRef.GetPlayerId() == activeEntity.OwnerId)
            {
                CreateHand(activeEntity);
            }
        }
    }
}