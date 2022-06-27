using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
    public override E_PlayerStates StateID => E_PlayerStates.Block;
    public override bool CanTranslateRepeatly => true;

    //对于这个状态,需要做些修改
    public override bool DoReason()
    {
        return Input.GetMouseButton(1) || InputManager.Instance.ReadCachedKey(KeyCode.Mouse0);
    }
    public override void OnEnter()
    {
        fsm.animator.Play("PerfectBlock");
    }

    public override void OnUpdate()
    {
        if (Input.GetMouseButtonUp(1))
        {
            fsm.animator.SetTrigger("BlockRelease");
            EndState();
        }

        //在取消格挡的过程中仍能控制开始格挡
        //fsm.TryEnterTargetState(E_PlayerStates.Block);
    }
}
