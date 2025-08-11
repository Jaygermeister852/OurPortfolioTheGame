using UnityEngine;

public class ResetIdleCounterSMB : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)           // This will run whenever we leave ANY state in the idle sub-state machine
    {
        var controller = animator.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.ResetIdleLoopCount();
        }
    }
}
