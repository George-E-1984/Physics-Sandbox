using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : AIBaseState
{
    public AttackState attackState; 
    public IdleState idleState; 
    public bool isInAttackRange;
    public bool rest; 
    public Animator animator; 
  public override AIBaseState RunCurrentState()
  {
    if (isInAttackRange)
    {
      return attackState; 
    }
    else if (rest)
    {
      animator.SetBool("isMoving", false); 
      return idleState; 
    }
    else 
    {
      return this; 
    }
    
  }

}
