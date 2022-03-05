using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 
using UnityEngine.Events;

public class ActiveRagdoll : MonoBehaviour
{
    [Header("Assignables")]
    public JointHandler jointHandler; 
    public ActiveRagdollPresets activeRagdollObject; 
    public AIStateManager aIStateManager; 
    public GameObject theOverallRig; 
    public GameObject theOverallAnimatedRig; 
    public GameObject physicsRig; 
    public GameObject animatedRig;
    public GameObject feetPoint; 
    public NavMeshAgent navMeshAgent; 
    public Collider visionCone; 
    public PlayerData playerData; 
    public Animator animator;

    [Header("Variables")]
    public float lookRadius = 10f;
    public int currentRagdollHealth; 

    [Header("Info (Do not touch)")]
    public Transform target; 
    public float distance;  
    public bool isGettingUp; 
    public RaycastHit hit; 
    public Rigidbody ragdollRigidbody; 
    public bool isAlive; 
    public UnityEvent waveModeEvent; 
    
    // Start is called before the first frame update
    void Start()
    {
        ragdollRigidbody = physicsRig.GetComponent<Rigidbody>(); 
        target = SceneMaster.instance.player.transform; 
        currentRagdollHealth = activeRagdollObject.maxRagdollHealth; 
        isAlive = true; 
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public bool HandleFalling()
    {
        if (Vector3.Distance(physicsRig.transform.position, animatedRig.transform.position) > activeRagdollObject.falloverDistance && !isGettingUp)
        { 
            //jointHandler.SetJointSettings(true); 
            //jointHandler.SetJointBones();
            //print("Distancebiggg"); 
            return true;   
        }
        else
        {
            return false; 
        }
      
    }

    public bool LookForPlayer()
    {
        distance = Vector3.Distance(target.position, animatedRig.transform.position); 
        if (distance <= lookRadius)
        {
            return true; 
        }
        else 
        {
            return false; 
        }
    }

    public bool GroundCheck()
    {
        Physics.Raycast(feetPoint.transform.position, Vector3.down, out hit, 0.5f);
        
        if (hit.collider != null)
        {
            aIStateManager.isGrounded = true;
            return true; 
        } 
        else 
        {
            aIStateManager.isGrounded = false; 
            print("Not Grounded"); 
            return false; 
        }
    }

    public void RagdollDeath()
    {
        aIStateManager.enabled = false; 
        jointHandler.SetJointSettings(false); 
        animatedRig.SetActive(false); 
        isAlive = false; 
        waveModeEvent.Invoke(); 
        this.enabled = false; 
    }

    public void RagdollRevive()
    {
        aIStateManager.enabled = true; 
        jointHandler.SetJointSettings(true); 
        animatedRig.SetActive(true); 
        isAlive = true; 
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, lookRadius); 
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activeRagdollObject.attackDistance);
    }

}
