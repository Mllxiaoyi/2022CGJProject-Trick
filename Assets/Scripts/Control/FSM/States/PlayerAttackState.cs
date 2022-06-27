using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
public class PlayerAttackState : PlayerBaseState
{
    public float attackMoveSpeed;
    [Tooltip("攻击时位移的时间,最长不超过攻击动画的时间")]
    public float attackMoveDuration = 100;

    public AudioSO attackSFX;
    public AudioSO heavyAttackSFX;

    private float _curDuration;
    public override E_PlayerStates StateID => E_PlayerStates.Attack;

    public override bool DoReason()
    {
        return Input.GetMouseButtonDown(0) || InputManager.Instance.ReadCachedKey(KeyCode.Mouse0);
    }

    public override void OnEnter()
    {
        if (fsm.combatData.energy >= 2)
        {
            fsm.combatData.energy = 0;
            fsm.animator.Play("HeavyAttack");
            AudioManager.Instance.PlayAudio(heavyAttackSFX);
        }
        else
        {
            fsm.animator.Play("Attack");
            AudioManager.Instance.PlayAudio(attackSFX);
        }
        _curDuration = 0;
        fsm.controller.FaceTo(1);                   //朝右
    }

    public override void OnUpdate()
    {
        if (fsm.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            EndState();
        }
        _curDuration += Time.deltaTime;
        if (_curDuration <= attackMoveDuration)
        {
            fsm.controller.MoveForward(attackMoveSpeed);
        }
        else
        {
            fsm.controller.Stop();
        }
    }
}
