using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnim : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float randomIndex = (float)Random.Range(0, 2);
        animator.SetFloat("idleVariant", randomIndex);
    }
}
