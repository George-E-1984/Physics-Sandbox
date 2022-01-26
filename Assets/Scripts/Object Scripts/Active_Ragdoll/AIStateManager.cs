using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateManager : MonoBehaviour
{
    [Header("Assignables")]
    public ActiveRagdoll activeRagdoll; 

    [Header("States")]
    public AIStates aiStates; 
    public enum AIStates{Idle, Agro, Wander, Fallen}; 

    [Header("State Settings")]
    public float wanderRadius; 
    

    // Update is called once per frame
    void Update()
    {
        HandleStates(); 
        
    }

    void Start() 
    {
        
    }

    public void HandleStates()
    {
        //setting states 
        if (activeRagdoll.LookForPlayer() && !activeRagdoll.fallen)
        {
            aiStates = AIStates.Agro; 
        }
        else if (activeRagdoll.LookForPlayer())
        {
            aiStates = AIStates.Fallen; 
        }
        else if (activeRagdoll.fallen == false)
        {
            aiStates = AIStates.Idle;
        }

        //state methods 
        switch(aiStates)
        {
            case AIStates.Agro: 
            AgroState();
            break; 
            case AIStates.Idle: 
            IdleState();
            break;
            case AIStates.Wander: 
            WanderState();
            break;
            case AIStates.Fallen: 
            FallenState();
            break;
           
            default:
            IdleState();  
            break; 
        }
      
    }

    public void IdleState()
    {
        activeRagdoll.animator.SetBool("isMoving", false);

    }
    
    public void WanderState()
    {
        print ("Wander");

    }
    

    public void FallenState()
    {

       

    }   

    public void AgroState()
    {
        activeRagdoll.animator.SetBool("isMoving", true); 
        activeRagdoll.navMeshAgent.SetDestination(activeRagdoll.target.position); 
    }
}
