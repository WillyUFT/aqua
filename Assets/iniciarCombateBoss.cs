using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iniciarCombateBoss : StateMachineBehaviour
{
    private BossController bossController;
    private Rigidbody2D rigidBody;
    private int repeatCount = 0;
    private bool isCounting = false;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        bossController = animator.GetComponent<BossController>();
        rigidBody = bossController.rigidBody;
        bossController.MirarJugador();
        repeatCount = 0;
        isCounting = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossController.MirarJugador();
    }

}
