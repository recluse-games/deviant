using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var currentText = GetComponent<InputField>().text;
        GameObject.Find("Input").GetComponent<UserInput>().SetPlayer(currentText);
    }
}
