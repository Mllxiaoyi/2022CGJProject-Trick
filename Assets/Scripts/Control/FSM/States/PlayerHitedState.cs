using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitedState : PlayerBaseState
{
    public override E_PlayerStates StateID => E_PlayerStates.Hited;


    public override void OnEnter()
    {
        fsm.animator.Play("Hited");

    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fsm.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            fsm.controller.FaceTo(1);
            EndState();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        fsm.controller.FaceTo(1);
    }

}
