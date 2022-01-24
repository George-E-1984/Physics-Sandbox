using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingUpState : AIBaseState
{
    public IdleState idleState; 
    public bool isStanding;  
    public override AIBaseState RunCurrentState()
    {
        if (isStanding)
        {
            return idleState; 
        }  
        else
        {
            return this; 
        }
    }
  
}
