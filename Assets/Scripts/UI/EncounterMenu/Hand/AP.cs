﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hand
{
    public class AP : MonoBehaviour
    {
        private string value = default;

        // Update is called once per frame
        public void UpdateValue(string value)
        {
            GetComponent<Text>().text = value;
        }
    }

}