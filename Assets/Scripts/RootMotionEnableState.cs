using UnityEngine;

public class RootMotionEnableState : StateMachineBehaviour
{
//    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        animator.applyRootMotion = true;
//    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
//
//    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//    {
//        animator.applyRootMotion = false;
//    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.applyRootMotion = true;
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.applyRootMotion = false;
    }
}