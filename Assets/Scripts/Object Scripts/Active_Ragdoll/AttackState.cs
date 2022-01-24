using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AIBaseState
{
    public AttackState attackState; 
    public bool isInAttackRange; 
    public override AIBaseState RunCurrentState()
    {
        Debug.Log("I have attacked"); 
        return this; 
    }
    
}
