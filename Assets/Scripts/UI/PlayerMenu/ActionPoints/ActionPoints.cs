using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ActionPoints : MonoBehaviour
{
	[SerializeField]
    APPrefab APPrefab = default;

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
        Deviant.Entity activeEntity = encounterStateRef.encounter.ActiveEntity;
        var actionPoints = GameObject.FindGameObjectsWithTag("ui_ap");
        
        if(actionPoints.Length < activeEntity.Ap)
        {
            var numberOfApToAdd =  activeEntity.Ap - actionPoints.Length;

            for (var i = 0; i < numberOfApToAdd; i++)
            {
                APPrefab ap = Instantiate(APPrefab);
                ap.transform.SetParent(transform, false);
                ap.transform.gameObject.tag = "ui_ap";
                ap.transform.SetParent(ap.GetComponentInParent<VerticalLayoutGroup>().transform, true);
                ap.transform.localPosition = new Vector3(0, 0, 0);
            }
        }

        if (actionPoints.Length > activeEntity.Ap)
        {
            var numberOfApToRemove = actionPoints.Length - activeEntity.Ap;
            for (var i = 0; i < numberOfApToRemove; i++)
            {
                Destroy(actionPoints[i]);
            }
        }
    }
}
