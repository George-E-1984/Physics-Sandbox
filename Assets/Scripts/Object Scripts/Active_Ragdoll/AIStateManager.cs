using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateManager : MonoBehaviour
{
    [Header("Assignables")]
    public ActiveRagdoll activeRagdoll; 

    [Header("States")]
    public AIStates aiStates; 
    public enum AIStates{Idle, Agro, Wander, Fallen, GettingUp}; 

    [Header("State Settings")]
    public float wanderRadius; 

    [Header("Info, do not touch")]
    [SerializeField]private int isWalkingHash;
    [SerializeField]private int isRunningHash;
    [SerializeField]private int isIdleHash;
    [SerializeField]private int isGettingUpHash;
    [SerializeField]private bool hasFallen; 

    

    // Update is called once per frame
    void Update()
    {
        HandleStates(); 
        hasFallen = activeRagdoll.HandleFalling();
    }

    void Start() 
    {
        isWalkingHash = Animator.StringToHash("isWalking");
        isIdleHash = Animator.StringToHash("isIdle"); 
        isRunningHash = Animator.StringToHash("isRunning"); 
        isGettingUpHash = Animator.StringToHash("isGettingUp");  
    }

    public void HandleStates()
    {
        //setting states 
        if (activeRagdoll.LookForPlayer() && !hasFallen)
        {
            aiStates = AIStates.Agro; 
        }
        else if (hasFallen && activeRagdoll.physicsRig.GetComponentInChildren<Rigidbody>().velocity.magnitude <= 2.5f && activeRagdoll.GroundCheck())
        {
            aiStates = AIStates.GettingUp; 
        }
        else if (hasFallen || !activeRagdoll.GroundCheck())
        {
            aiStates = AIStates.Fallen; 
        }
        else if (hasFallen == false)
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
            case AIStates.GettingUp:
            GettingUpState(); 
            break; 
           
            default:
            IdleState();  
            break; 
        }
      
    }

    public void IdleState()
    {
        //activeRagdoll.animator.SetBool("isWalking", false);
        activeRagdoll.isGettingUp = false; 
        activeRagdoll.navMeshAgent.isStopped = true;
        activeRagdoll.animator.SetBool(isWalkingHash, false); 
        activeRagdoll.animator.SetBool(isIdleHash, true);   
        activeRagdoll.animator.SetBool(isGettingUpHash, false);

    }
    
    public void WanderState()
    {
        activeRagdoll.isGettingUp = false; 
        
        print ("Wander");
    }
    

    public void FallenState()
    { 
        activeRagdoll.isGettingUp = false; 
        activeRagdoll.animator.SetBool(isIdleHash, false); 
        activeRagdoll.animator.SetBool(isWalkingHash, false);  
        activeRagdoll.animator.SetBool(isGettingUpHash, false);
        activeRagdoll.animator.enabled = true;
        activeRagdoll.animatedRig.transform.position = activeRagdoll.physicsRig.transform.position;
    }   

    public void AgroState()
    {
        //activeRagdoll.animator.SetBool("isWalking", true); 
        //activeRagdoll.jointHandler.SetJointSettings(true);
        activeRagdoll.isGettingUp = false; 
        if (!activeRagdoll.isGettingUp)
        {
            activeRagdoll.navMeshAgent.isStopped = false;
        }
        else 
        {
            activeRagdoll.navMeshAgent.isStopped = true;
        }
        activeRagdoll.navMeshAgent.SetDestination(activeRagdoll.target.position);
        activeRagdoll.animator.SetBool(isIdleHash, false); 
        activeRagdoll.animator.SetBool(isWalkingHash, true); 
        activeRagdoll.animator.SetBool(isGettingUpHash, false);
         
    }

    public void GettingUpState()
    {
        print("Gettingup!");  
        activeRagdoll.animator.enabled = true; 
        activeRagdoll.isGettingUp = true; 
        activeRagdoll.jointHandler.SetJointSettings(false); 
        activeRagdoll.jointHandler.SetJointBones();
        activeRagdoll.animator.SetBool(isIdleHash, false); 
        activeRagdoll.animator.SetBool(isWalkingHash, false);
        activeRagdoll.animator.SetBool(isGettingUpHash, true);  
    }
}
