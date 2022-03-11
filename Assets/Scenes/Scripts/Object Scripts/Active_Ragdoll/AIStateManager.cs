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
    [SerializeField]private bool foundPlayer; 
    

    [Header("Info")]
    public bool isGrounded; 
    public bool isInFallenState; 

    

    // Update is called once per frame
    void Update()
    {
        hasFallen = activeRagdoll.HandleFalling();
        foundPlayer = activeRagdoll.LookForPlayer(); 
        isGrounded = activeRagdoll.GroundCheck(); 
        HandleStates();  
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
        if (foundPlayer && !isInFallenState && isGrounded && !activeRagdoll.isGettingUp && !hasFallen)
        {
            aiStates = AIStates.Agro; 
        }
        else if (hasFallen && activeRagdoll.ragdollRigidbody.velocity.magnitude <= 1 && isGrounded)
        {
            aiStates = AIStates.GettingUp; 
        }
        else if (hasFallen || !isGrounded)
        {
            aiStates = AIStates.Fallen; 
        }
        else if (!isInFallenState && isGrounded && !activeRagdoll.isGettingUp && !hasFallen)
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
        hasFallen = true;
        isInFallenState = true; 
        activeRagdoll.theOverallAnimatedRig.transform.position = activeRagdoll.rootPhysicsObj.transform.position;
        activeRagdoll.animator.SetBool(isIdleHash, false); 
        activeRagdoll.animator.SetBool(isWalkingHash, false);  
        activeRagdoll.animator.SetBool(isGettingUpHash, false); 
        activeRagdoll.jointHandler.SetJointSettings(true);
        activeRagdoll.jointHandler.SetJointBones(); 
        activeRagdoll.animator.enabled = true;
        print("Fallen");
    }   

    public void AgroState()
    {
        //activeRagdoll.animator.SetBool("isWalking", true); 
        //activeRagdoll.jointHandler.SetJointSettings(true); 
        activeRagdoll.navMeshAgent.isStopped = false;
        activeRagdoll.animator.SetBool(isIdleHash, false); 
        activeRagdoll.animator.SetBool(isGettingUpHash, false);
        activeRagdoll.animator.SetBool(isWalkingHash, true); 
        activeRagdoll.navMeshAgent.SetDestination(activeRagdoll.target.position);
        float distance = Vector3.Distance(activeRagdoll.target.position, activeRagdoll.rootAnimatedObj.transform.position); 
        if (distance <= activeRagdoll.activeRagdollObject.attackDistance)
        {
            activeRagdoll.animator.SetLayerWeight(1, 1); 
        }
        else 
        {
            activeRagdoll.animator.SetLayerWeight(1, 0);
        }
    }

    public void GettingUpState()
    {
        print("Gettingup!");    
        //activeRagdoll.animatedRig.transform.localPosition = new Vector3(0, 0, 0); 
        //activeRagdoll.theOverallRig.transform.position = activeRagdoll.physicsRig.transform.position; 
        //activeRagdoll.theOverallAnimatedRig.transform.position = activeRagdoll.physicsRig.transform.position;
        hasFallen = false;
        isInFallenState = false;
        activeRagdoll.isGettingUp = true;
        activeRagdoll.animator.enabled = true;  
        activeRagdoll.jointHandler.SetJointSettings(false); 
        activeRagdoll.jointHandler.SetJointBones();
        activeRagdoll.animator.SetBool(isIdleHash, false); 
        activeRagdoll.animator.SetBool(isWalkingHash, false);
        activeRagdoll.animator.SetBool(isGettingUpHash, true);  
        if (activeRagdoll.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            activeRagdoll.isGettingUp = false; 
        }
    }
}
