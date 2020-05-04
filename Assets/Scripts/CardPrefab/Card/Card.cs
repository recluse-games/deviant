using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string id = default;

    public void SetId(string id)
    {
        this.id = id;
    }

    public string GetId()
    {
        return id;
    }
}
