﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenuEntryPrefab : MonoBehaviour
{
    [SerializeField]
    Transform EntityPanel = default;

    public GameObject ParentPanel;

    public GameObject Ap;
    public GameObject Hp;
    public GameObject Ar;
    public GameObject Name;

    private Deviant.Entity entity = default;

    private void Update()
    {
        UpdateHP(entity.Hp);
        UpdateAP(entity.Ap);
        UpdateName(entity.Name);
    }

    public void SetEntity(Deviant.Entity newEntity)
    {
        this.entity = newEntity;
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