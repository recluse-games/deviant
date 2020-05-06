using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Deviant;

public class NextButton : MonoBehaviour
{
    public EncounterState encounterStateRef = default;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    // Update is called once per frame
    void Update()
    {
        Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
    }

    async void TaskOnClick()
    {
        // Retrieve the Current Encounter From Shared State.
        Deviant.Encounter encounterState = encounterStateRef.GetEncounter();

        Deviant.EncounterRequest encounterRequest = new Deviant.EncounterRequest();
        encounterRequest.PlayerId = "0001";
        encounterRequest.Encounter = encounterState;
        encounterRequest.EntityActionName = Deviant.EntityActionNames.ChangePhase;

        await encounterStateRef.UpdateEncounterAsync(encounterRequest);
    }
}
