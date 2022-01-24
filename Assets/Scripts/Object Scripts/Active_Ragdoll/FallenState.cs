using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenState : AIBaseState
{
    public GettingUpState gettingUpState; 
    public bool isAbleToStand; 
    public override AIBaseState RunCurrentState()
    {
        if (isAbleToStand)
        {
            return gettingUpState; 
        }  
        else
        {
            return this; 
        }
    }
 
}
