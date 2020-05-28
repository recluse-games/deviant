using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTriggersOnExitBehavior : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(parameter.name);
        }
    }
}
