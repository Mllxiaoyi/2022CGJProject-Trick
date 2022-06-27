using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("攻击/受击位移属性")]
    public float hitBackSpeed=6;
    public float AttackMoveSpeed=6;
    public float FarAttackMoveSpeed = 10;

    public bool isUnInterrupt;

    private CharacterController2D controller;
    void Start()
    {
        GetComponent<CombatData>().OnUnPerfectBlocked.AddListener(() => StartCoroutine(HitedBack()));
        //GetComponent<CombatData>().OnUnPerfectBlocked.AddListener(()=>StartCoroutine(HitedBack()));
        controller = GetComponent<CharacterController2D>();
    }

    //受击位移
    private IEnumerator HitedBack()
    {
        float time = 0.2f;
        while (time > 0)
        {
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
            controller.MoveBackward(hitBackSpeed);
        }
        controller.Stop();
    }

    public void AnimAttackTrigger() 
    {
        StartCoroutine(AttackMove(AttackMoveSpeed));
        //动画开始攻击了就UnInterrupt，UnInterrupt==false由攻击范围的disable调用
        isUnInterrupt = true;
    }

    public void AnimFarAttackTrigger()
    {
        Debug.Log("FarAttack");
        StartCoroutine(AttackMove(FarAttackMoveSpeed));
        //动画开始攻击了就UnInterrupt，UnInterrupt==false由攻击范围的disable调用
        isUnInterrupt = true;
    }

    //攻击位移
    private IEnumerator AttackMove(float moveSpeed)
    {
        float time = 0.2f;
        yield return new WaitForFixedUpdate();
        while (time > 0)
        {
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
            controller.MoveForward(moveSpeed);
        }
        controller.Stop();
    }

}
