using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseName : MonoBehaviour
{
    public string placeHolderText = default;
    public EncounterState encounterStateRef = default;

    // Start is called before the first frame update
    void Start()
    {
        // Find Operations Are Expensive So the Less We Do The Better.
		encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    // Update is called once per frame
    void Update()
    {
        Deviant.Encounter encounterState = encounterStateRef.GetEncounter();
        placeHolderText = encounterState.Turn.Phase.ToString();
        gameObject.GetComponent<Text>().text = placeHolderText;
    }
}
