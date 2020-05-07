using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    private string value = default;

    // Update is called once per frame
    public void UpdateValue(string value)
    {
       GetComponent<Text>().text = value;
    }
}
