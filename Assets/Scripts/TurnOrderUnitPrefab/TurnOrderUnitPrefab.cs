using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUnitPrefab : MonoBehaviour
{

    [SerializeField]
    Transform TurnOrderUnit = default;

    private EncounterState encounterStateRef = default;
    private string id = default;

    private void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();
    }

    public void SetId(string id)
    {
        this.id = id;
    }

    public string GetId()
    {
        return this.id;
    }

    private void Update()
    {
        Deviant.Entity activeEntity = encounterStateRef.encounter.ActiveEntity;

        if (activeEntity.Id == this.id)
        {
            this.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            this.gameObject.transform.position = this.gameObject.transform.position + new Vector3(0, 0, 5);
        }

        if (activeEntity.Id != this.id)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
