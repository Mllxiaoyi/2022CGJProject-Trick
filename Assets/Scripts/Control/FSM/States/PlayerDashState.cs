using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
public class PlayerDashState : PlayerBaseState
{
    public float dashDistance;
    public float dashTime;

    public AudioSO audioSO;

    private float _curDashDuration;
    private int _dir;
    private float _cd=0.2f;
    public override E_PlayerStates StateID => E_PlayerStates.Dash;


    private void Update()
    {
        _cd -= Time.deltaTime;
    }

    public override bool DoReason()
    {
        if (_cd >= 0) { return false; }
        if (Input.GetKeyDown(KeyCode.A)|| InputManager.Instance.ReadCachedKey(KeyCode.A))
        {
            _dir = -1;
            return true;
        }
        if (Input.GetKeyDown(KeyCode.D) || InputManager.Instance.ReadCachedKey(KeyCode.D))
        {
            _dir = 1;
            return true;
        }
        return false;
    }

    public override void OnEnter()
    {
        fsm.animator.Play("DashStart");
        _curDashDuration = 0;
        AudioManager.Instance.PlayAudio(audioSO);
    }


    public override void OnUpdate()
    {
        if (_curDashDuration > dashTime)
        {
            EndState();
            return;
        }
        if (fsm.animator.IsAnimFinished("DashStart"))
        {
            fsm.animator.Play("Dash");
        }
        if (fsm.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            fsm.controller.MoveTowards(dashDistance / dashTime, _dir);
        }
        _curDashDuration += Time.deltaTime;
    }

    public override void OnExit()
    {
        fsm.controller.Stop();
        _cd = 0.2f;
    }
}
