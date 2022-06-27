using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override E_PlayerStates StateID => E_PlayerStates.Idle;

    public override void OnEnter()
    {

    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fsm.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            fsm.controller.FaceTo(1);
        }
        fsm.controller.Stop();
        fsm.TryEnterTargetState(E_PlayerStates.Attack);
        //��״̬���⴦��,��Ȼ�񵲿����а����ӳ�,��Idle״̬������
        if (Input.GetMouseButton(1)) { fsm.ChangeState(E_PlayerStates.Block); }
        fsm.TryEnterTargetState(E_PlayerStates.Dash);
    }

    public override void OnExit()
    {
        base.OnExit();
        fsm.controller.FaceTo(1);
    }
}
