﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ObserverPattern
{
    public class Subject : MonoBehaviour
    {
        //A list with observers that are waiting for something to happen
        public List<GameObject> observers = new List<GameObject>();

        //Send notifications if something has happened
        public void Notify(Deviant.Encounter encounter)
        {
            Debug.Log(this.observers);

            foreach (GameObject gameobj in this.observers)
            {
                Debug.Log(gameobj);
                //Notify all observers even though some may not be interested in what has happened
                //Each observer should check if it is interested in this event
                gameobj.GetComponent<EntityObserver>().OnNotify(encounter);
            }
        }

        //Add observer to the list
        public void AddObserver(GameObject observer)
        {
            observers.Add(observer);
        }

        //Remove observer from the list
        public void RemoveObserver(GameObject observer)
        {
            observers.Remove(observer);
        }
    }
}