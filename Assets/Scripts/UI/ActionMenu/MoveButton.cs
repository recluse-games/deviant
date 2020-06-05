using Deviant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Scripting.Pipeline;

public class MoveButton : MonoBehaviour
{
    public Button button;
    public EncounterState encounterStateRef = default;

    // Start is called before the first frame update
    void Start()
    {
        encounterStateRef = GameObject.Find("/EncounterState").GetComponent<EncounterState>();

        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    async void TaskOnClick()
    {
        Entity[] entityObjects = FindObjectsOfType<Entity>();

        foreach (Entity entity in entityObjects)
        {
            if(entity.GetId() == encounterStateRef.encounter.ActiveEntity.Id)
            {
                await entity.activateMoveAction();
                GameObject actionMenu = GameObject.Find("/UI/ActionMenu");
                actionMenu.GetComponent<ActionMenu>().Hide();
                entity.SetCollider(true);
            }
        }
    }
}
