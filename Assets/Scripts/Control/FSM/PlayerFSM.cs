using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
public class PlayerFSM : MonoBehaviour
{
    [Header("属性")]
    

    [Header("相关组件")]
    [HideInInspector]public Animator animator;
    [HideInInspector] public CharacterController2D controller;
    [HideInInspector] public CombatData combatData;

    [Header("状态机相关")]
    public Transform statesRoot;
    public Dictionary<E_PlayerStates, PlayerBaseState> statesDic
        =new Dictionary<E_PlayerStates, PlayerBaseState>();

    [ShowInInspector,DisplayAsString]
    public E_PlayerStates CurrentState { get; private set; }
    [ShowInInspector, DisplayAsString]
    public E_PlayerStates LastState { get; private set;}

    public bool IsAttack => CurrentState == E_PlayerStates.Attack;
    public bool IsDash=> CurrentState == E_PlayerStates.Dash;
    void Start()
    {
        GetRelatedCompoents();
        RegistAllAbilities();
        CurrentState = E_PlayerStates.Idle;

        combatData.OnHited.AddListener(ForceChangeToHitedState);
        combatData.OnDied.AddListener(() => ChangeState(E_PlayerStates.Executed));
        combatData.OnDied.AddListener(() => Invoke("BackToStartMenu",4));
    }

    private void RegistAllAbilities()
    {
        if (statesRoot == null) { Debug.LogError("请设置玩家状态的载体;"); }
        PlayerBaseState[] states = statesRoot.GetComponents<PlayerBaseState>();
        foreach (var item in states)
        {
            if (statesDic.ContainsKey(item.StateID))
            {
                Debug.LogError("添加了相同的状态！"); return;
            }
            statesDic.Add(item.StateID, item);
            item.Init(this);
        }
    }

    private void GetRelatedCompoents()
    {
        if (!animator) { animator = GetComponent<Animator>(); }
        if (!controller) { controller = GetComponent<CharacterController2D>(); }
        combatData = GetComponent<CombatData>();
    }

    private void ForceChangeToHitedState()
    {
        if (CurrentState!=E_PlayerStates.Block)
        {
            ChangeState(E_PlayerStates.Hited);
            StartCoroutine(HitedBack());
        }
        else
        {
            StartCoroutine(HitedBack());
        }
    }

    private IEnumerator HitedBack()
    {
        float time = 0.2f;
        while (time>0)
        {
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
            controller.MoveBackward(6);
        }
        controller.Stop();
    }

    void Update()
    {
        statesDic[CurrentState]?.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToStartMenu();
        }
    }

    private void BackToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void TryEnterTargetState(E_PlayerStates targetState)
    {
        if (statesDic[targetState].DoReason())
        {
            ChangeState(targetState);
        }
    }
    public void ChangeState(E_PlayerStates nextState)
    {
        if (CurrentState == nextState&& !statesDic[CurrentState].CanTranslateRepeatly) return;
        statesDic[CurrentState].OnExit();
        LastState = CurrentState;
        CurrentState = nextState;
        statesDic[CurrentState].OnEnter();
    }

    public PlayerBaseState GetState(E_PlayerStates state)
    {
        if (statesDic.ContainsKey(state))
        {
            return statesDic[state];
        }
        Debug.LogError("尝试获取不存在的状态");
        return null;
    }
}
