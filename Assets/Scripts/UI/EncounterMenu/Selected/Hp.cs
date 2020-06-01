using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Selected
{
    public class Hp : MonoBehaviour
    {
        private string value = default;

        public void UpdateValue(string value)
        {
            GetComponent<Text>().text = value;
        }
    }

}
