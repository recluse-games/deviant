using UnityEngine;
using System.Linq;
using Deviant;
using System.Collections;

public class UI : MonoBehaviour
{
    [SerializeField]
    float deckX, deckY, deckZ;

    [SerializeField]
	string playerId = default;

    [SerializeField]
    CardPrefab cardPrefab = default;

	private EncounterState encounterStateRef = default;

    private void CreateHand(Deviant.Entity activeEntity)
    {
        int x = -250;
        int y = -170;

        foreach (var card in activeEntity.Hand.Cards)
        {
            GameObject existingHandCardGameObject = GameObject.Find("hand_" + card.InstanceId);

            if(existingHandCardGameObject != null) {
                x += 70;
                continue;
            }

            CardPrefab newCard = Instantiate(cardPrefab);
            newCard.transform.localPosition = new Vector3Int(x, y, 0);
            newCard.transform.SetParent(transform, false);
            newCard.setSprite(card.Id, card.BackId);
            newCard.transform.gameObject.tag = "hand";
            newCard.transform.gameObject.name = "hand_" + card.InstanceId;
            newCard.GetComponentInChildren<Card>().SetId(card.InstanceId);
            newCard.SetVisability(true);
            x += 70;
        }

        foreach (var deckCardObject in GameObject.FindGameObjectsWithTag("deck"))
        {
            foreach (var handCardObject in GameObject.FindGameObjectsWithTag("hand"))
            {
                if (deckCardObject.GetComponentInChildren<Card>().GetId() == handCardObject.GetComponentInChildren<Card>().GetId())
                {
                    Destroy(deckCardObject);
                }
            }
        }
    }

    private void CreateDeck(Deviant.Entity activeEntity)
    {
        int deckx = -350;
        int decky = -170;

        foreach (var card in activeEntity.Deck.Cards)
        {
            GameObject existingDeckCardGameObject = GameObject.Find("deck_" + card.InstanceId);

            if(existingDeckCardGameObject == null) {
                CardPrefab newDeckCardGameObject = Instantiate(cardPrefab);
                newDeckCardGameObject.setSprite(card.Id, card.BackId);
                newDeckCardGameObject.transform.localPosition = new Vector3Int(deckx, decky, 0);
                newDeckCardGameObject.transform.SetParent(transform, false);
                newDeckCardGameObject.transform.gameObject.tag = "deck";
                newDeckCardGameObject.transform.gameObject.name = "deck_" + card.InstanceId;
                newDeckCardGameObject.GetComponentInChildren<Card>().SetId(card.InstanceId);
            }
        }
    }
     
    // Start is called before the first frame update
    void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();  
    }

    void Update()
    {
        // Retrieve the Current Encounter From Shared State.
		Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
        Deviant.Entity activeEntity = encounterState.ActiveEntity;

        // 0000 should be replaced with the current players ID
        if(activeEntity.OwnerId == "0000")
        {
            CreateDeck(activeEntity);
            CreateHand(activeEntity);
        }
    }
}
