using UnityEngine;
using System.Collections;


namespace ObserverPattern
{
    //Events
    public abstract class EncounterEvents
    {
        public abstract Deviant.Encounter GetEncounterState(Deviant.Encounter encounter);
    }


    public class GetEncounter : EncounterEvents
    {
        public override Deviant.Encounter GetEncounterState(Deviant.Encounter encounter)
        {
            return encounter;
        }
    }
}