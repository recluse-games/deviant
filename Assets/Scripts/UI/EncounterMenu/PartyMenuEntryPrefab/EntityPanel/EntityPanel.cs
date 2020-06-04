using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPanel : MonoBehaviour
{
    [SerializeField]
    Transform FriendlyParty = default;

    public GameObject ParentPanel;

    public GameObject Ap;
    public GameObject Hp;
    public GameObject Ar;
    public GameObject Name;

    private Deviant.Entity entity = default;
    private void Update()
    {
        if(entity != null)
        {
            UpdateHP(entity.Hp);
            UpdateAP(entity.Ap);
            UpdateName(entity.Name);
        }
    }

    public Deviant.Entity GetEntity()
    {
        return this.entity;
    }

    public void SetEntity(Deviant.Entity newEntity)
    {
        this.entity = newEntity;
        UpdateHP(entity.Hp);
        UpdateAP(entity.Ap);
        UpdateName(entity.Name);
    }

    void UpdateHP(int value)
    {
        this.GetComponentInChildren<HP>().UpdateValue(value.ToString());
    }

    void UpdateAP(int value)
    {
        this.GetComponentInChildren<AP>().UpdateValue(value.ToString());
    }

    void UpdateName(string value)
    {
        this.GetComponentInChildren<Name>().UpdateValue(value);
    }
}
