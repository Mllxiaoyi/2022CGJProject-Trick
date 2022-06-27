using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("����/�ܻ�λ������")]
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

    //�ܻ�λ��
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
        //������ʼ�����˾�UnInterrupt��UnInterrupt==false�ɹ�����Χ��disable����
        isUnInterrupt = true;
    }

    public void AnimFarAttackTrigger()
    {
        Debug.Log("FarAttack");
        StartCoroutine(AttackMove(FarAttackMoveSpeed));
        //������ʼ�����˾�UnInterrupt��UnInterrupt==false�ɹ�����Χ��disable����
        isUnInterrupt = true;
    }

    //����λ��
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
