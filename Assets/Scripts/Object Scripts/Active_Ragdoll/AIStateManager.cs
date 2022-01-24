using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateManager : MonoBehaviour
{
    public AIBaseState currentState;
    // Update is called once per frame
    void Update()
    {
        RunStateMachine(); 
    }

    private void RunStateMachine()
    {
        AIBaseState nextState = currentState?.RunCurrentState(); 

        if (nextState != null)
        {
            SwitchToTheNextState(nextState); 
        }
    }

    private void SwitchToTheNextState(AIBaseState nextState)
    {
        currentState = nextState; 
    }
}
