using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour, IState
{
    protected PlayerFSM fsm;
    public virtual bool CanTranslateRepeatly=>false; 
 
    public void Init(PlayerFSM fsm)
    {
        this.fsm = fsm;
    }
    public abstract E_PlayerStates StateID{ get;}
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }

    public virtual bool DoReason()
    {
        return false;
    }     
    protected void EndState()
    {
        fsm.ChangeState(E_PlayerStates.Idle);
    }
}
