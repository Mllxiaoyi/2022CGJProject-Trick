using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCauser : MonoBehaviour
{
    public string targetTag;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if (GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HeavyAttack"))
            {
                Debug.Log(collision.name + "因为" + this.name + "而受蓄能攻击");
                collision.GetComponent<IDamageable>().OnHited(true);
            }
            else
            {
                Debug.Log(collision.name + "因为" + this.name + "而受击");
                collision.GetComponent<IDamageable>().OnHited(false);
            }            
        }
    }

    private void OnDisable()
    {
        if (GetComponentInParent<Enemy>())
        {
            GetComponentInParent<Enemy>().isUnInterrupt = false;
        } 
    }
}
