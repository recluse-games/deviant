using UnityEngine;
using System.Collections;

namespace ObserverPattern
{
    public class EntityObserver : MonoBehaviour
    {
        public GameObject entityObj;
        public GetEncounter encounterEvent;

        public void SetEntity(GameObject entity)
        {
            this.entityObj = entity;
        }

        public void SetEncounterEvents(GetEncounter encounterEvent)
        {
            this.encounterEvent = encounterEvent;
        }

        //What the box will do if the event fits it (will always fit but you will probably change that on your own)
        public void OnNotify(Deviant.Encounter encounter)
        {
            UpdateEntityData(encounter);
        }

        //The box will always jump in this case
        public void UpdateEntityData(Deviant.Encounter encounter)
        {
            entityObj.GetComponentInChildren<Entity>().UpdateAnimationController(encounter);
            entityObj.GetComponentInChildren<Entity>().UpdateAnimation(encounter);
            entityObj.GetComponentInChildren<Entity>().UpdateOutline(encounter);
        }
    }
}
