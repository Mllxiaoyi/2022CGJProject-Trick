using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class AnimatorUtility
{
    public static bool IsAnimFinished(this Animator animator, string animName, int stateIndex = 0)
    {
        AnimatorStateInfo animatorInfo;
        animatorInfo = animator.GetCurrentAnimatorStateInfo(stateIndex);

        if (animatorInfo.IsName(animName) && animatorInfo.normalizedTime >= 1)
            return true;        
        else
            return false;
    }


    public static bool CheckIsState(this Animator animator, string animName, int stateIndex = 0)
    {
        return animator.GetCurrentAnimatorStateInfo(stateIndex).IsName(animName);
    }
}


