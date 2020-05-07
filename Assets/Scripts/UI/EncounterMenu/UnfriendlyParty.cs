using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnfriendlyParty : MonoBehaviour
{
    [SerializeField]
    EntityPanel partyMenuEntryPrefab = default;

    public EncounterState encounterStateRef = default;

    void Start()
    {
        // Find Operations Are Expensive So the Less We Do The Better.
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    void Update()
    {
        Deviant.Encounter encounterState = encounterStateRef.GetEncounter();

        foreach (var row in encounterState.Board.Entities.Entities_)
        {
            foreach (var entity in row.Entities)
            {
                if (entity.Alignment == Deviant.Alignment.Unfriendly)
                {
                    var entityPartyPanelComponent = GameObject.Find($"/UI/EncounterMenu/UnfriendlyParty/unfriendly_party_ui_entity_{entity.Id}");

                    if(entityPartyPanelComponent == null)
                    {
                        EntityPanel turnOrderUnit = Instantiate(partyMenuEntryPrefab);
                        turnOrderUnit.transform.gameObject.name = "unfriendly_party_ui_entity_" + entity.Id;
                        turnOrderUnit.transform.SetParent(this.GetComponent<VerticalLayoutGroup>().transform, false);
                        turnOrderUnit.SetEntity(entity);
                    }
                }
            }
        }
    }
}