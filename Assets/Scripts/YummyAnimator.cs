using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YummyAnimatorExtension
{
    public static float GetAnimDuration(this Animator animator, string animName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
