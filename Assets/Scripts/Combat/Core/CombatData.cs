using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Game;

public class CombatData : MonoBehaviour, IDamageable
{
    [Header("相应属性")]
    public int jiaShiTiao;
    public int energy;
    public bool isInvincible=> 
        attackRange.isActiveAndEnabled&& _animator.GetCurrentAnimatorStateInfo(0).IsName("HeavyAttack");      //是否无敌

    [Header("相应组件")]
    private Animator _animator;
    public TMP_Text hpText;

    public AudioSO energyUpSFX;
    public AudioSO energyMaxSFX;
    public AudioSO HitedSFX;
    public AudioSO BlockSFX;
    public AudioSO PerfectBlockSFX;

    public UnityEvent OnHited;
    public UnityEvent OnDamaged;
    public UnityEvent OnUnPerfectBlocked;
    public UnityEvent OnDied;

    private Collider2D attackRange; 
    private void Start()
    {
        _animator = GetComponent<Animator>();
        attackRange = GetComponentInChildren<Collider2D>();
    }


    private void LateUpdate()
    {
        hpText.text = jiaShiTiao.ToString();
    }

    void IDamageable.OnHited(bool isHeavy)
    {
        if (isInvincible) { return; }
        //如果是蓄能攻击
        if (isHeavy)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("PerfectBlock"))
            {
                Debug.Log("蓄能攻击完美格挡");
                AudioManager.Instance.PlayAudio(PerfectBlockSFX);
                energy++;
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Block") || _animator.GetCurrentAnimatorStateInfo(0).IsName("UnPerfectBlock"))
            {
                Debug.Log("蓄能攻击普通格挡");
                AudioManager.Instance.PlayAudio(BlockSFX);
                jiaShiTiao -= 2;
                OnUnPerfectBlocked?.Invoke();
            }
            else
            {
                AudioManager.Instance.PlayAudio(HitedSFX);
                jiaShiTiao -= 3;
                OnDamaged?.Invoke();
            }
        }
        else
        {            
            OnHited?.Invoke();

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("PerfectBlock") || _animator.CheckIsState("CD2_Prepare") || _animator.CheckIsState("CD2_Block"))
            {
                Debug.Log("完美格挡");
                AudioManager.Instance.PlayAudio(PerfectBlockSFX);
                energy++;
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Block") || _animator.GetCurrentAnimatorStateInfo(0).IsName("UnPerfectBlock"))
            {
                Debug.Log("普通格挡");
                AudioManager.Instance.PlayAudio(BlockSFX);
                jiaShiTiao--;
                OnUnPerfectBlocked?.Invoke();
            }
            else
            {
                AudioManager.Instance.PlayAudio(HitedSFX);
                jiaShiTiao -= 2;
                OnDamaged?.Invoke();
            }            
        }

        if (jiaShiTiao <= 0)
        {
            Died();
            this.GetComponent<BoxCollider2D>().isTrigger = true;
            this.GetComponent<Rigidbody2D>().gravityScale = 0;

        }

    }

    private void Died()
    {
        OnDied?.Invoke();
        this.tag = "Untagged";
        return;
    }
}