using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ActionPoints : MonoBehaviour
{
	public EncounterState encounterStateRef = default;

    public GameObject ParentPanel;

    // Start is called before the first frame update
    void Start()
    {
		encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    // Update is called once per frame
    void Update()
    {	
    }
}
