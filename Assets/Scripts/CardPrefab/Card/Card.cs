using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool mouse_over = false;
    
     void Update()
     {
         if (mouse_over)
         {
             Debug.Log("Mouse Over");
         }
     }
}
