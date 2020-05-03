using UnityEngine;
using System.Linq;

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
            GameObject existingHandCardGameObject = GameObject.Find("hand_" + card.Id);
            GameObject existingDeckCardGameObject = GameObject.Find("deck_" + card.Id);

            if(existingHandCardGameObject != null && existingDeckCardGameObject != null) {
                Destroy(existingDeckCardGameObject);
                x += 70;
                continue;
            }

            if(existingHandCardGameObject != null) {
                x += 70;
                continue;
            }

            CardPrefab newCard = Instantiate(cardPrefab);
            newCard.transform.localPosition = new Vector3Int(x, y, 0);
            newCard.transform.SetParent(transform, false);
            newCard.transform.gameObject.name = "hand_" + card.Id;
            newCard.SetVisability(true);
            x += 70;
        }

    }

    private void CreateDeck(Deviant.Entity activeEntity)
    {
        int deckx = -350;
        int decky = -163;

        Debug.Log(activeEntity.Deck.Cards);

        foreach (var card in activeEntity.Deck.Cards)
        {
            GameObject existingDeckCardGameObject = GameObject.Find("deck_" + card.Id);

            if(existingDeckCardGameObject == null) {
                CardPrefab newDeckCardGameObject = Instantiate(cardPrefab);
                newDeckCardGameObject.setSprite();
                newDeckCardGameObject.transform.localPosition = new Vector3Int(deckx, decky, 0);
                newDeckCardGameObject.transform.SetParent(transform, false);
                newDeckCardGameObject.transform.gameObject.name = "deck_" + card.Id;
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

        CreateDeck(activeEntity);
        CreateHand(activeEntity);
    }
}
