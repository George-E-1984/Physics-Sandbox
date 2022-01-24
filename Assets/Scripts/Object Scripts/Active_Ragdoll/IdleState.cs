using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AIBaseState
{
    public ChaseState chaseState; 
    public bool canSeeThePlayer; 
    public override AIBaseState RunCurrentState()
    {
        if (canSeeThePlayer)
        { 
            animator.SetBool("isMoving", true); 
            return chaseState; 
        }  
        else
        {
            return this; 
        }
    }

    public Animator animator; 
    public AnimationClip idleAnimation; 
  
}
