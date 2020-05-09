using Deviant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrder : MonoBehaviour
{
    [SerializeField]
    TurnOrderUnitPrefab TurnOrderUnitPrefab = default;

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
        var encounter = encounterStateRef.GetEncounter();

        var turnOrderUnits = GameObject.FindGameObjectsWithTag("ui_turn_order_unit");

        if (turnOrderUnits.Length < encounter.ActiveEntityOrder.Count)
        {
            for (var i = 0; i < encounter.ActiveEntityOrder.Count; i++)
            {
                TurnOrderUnitPrefab turnOrderUnit = Instantiate(TurnOrderUnitPrefab);
                turnOrderUnit.transform.SetParent(transform, false);
                turnOrderUnit.transform.gameObject.tag = "ui_turn_order_unit";
                turnOrderUnit.transform.SetParent(turnOrderUnit.GetComponentInParent<HorizontalLayoutGroup>().transform, true);
                turnOrderUnit.SetId(encounter.ActiveEntityOrder[i]);
                turnOrderUnit.transform.localPosition = new Vector3(0, 0, 0);
            }
        }

        if (turnOrderUnits.Length > encounter.ActiveEntityOrder.Count)
        {
            foreach(var turnOrderUnit in turnOrderUnits)
            {
                string turnOrderId = turnOrderUnit.GetComponent<TurnOrderUnitPrefab>().GetId();

                if (!encounter.ActiveEntityOrder.Contains(turnOrderId))
                {
                    Destroy(turnOrderUnit);
                }
            }
        }
    }
}
