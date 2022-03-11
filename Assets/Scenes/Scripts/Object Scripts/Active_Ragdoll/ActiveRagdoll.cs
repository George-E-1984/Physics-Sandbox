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
    public GameObject theOverallPhysicsRig; 
    public GameObject rootPhysicsObj; 
    public GameObject rootAnimatedObj;
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
    public bool isAlive = true; 
    public UnityEvent waveModeEvent; 
    
    // Start is called before the first frame update
    void Start()
    {
        ragdollRigidbody = rootPhysicsObj.GetComponent<Rigidbody>(); 
        target = SceneMaster.instance.player.transform; 
        currentRagdollHealth = activeRagdollObject.maxRagdollHealth; 
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public bool HandleFalling()
    {
        if (Vector3.Distance(rootPhysicsObj.transform.position, rootAnimatedObj.transform.position) > activeRagdollObject.falloverDistance && !isGettingUp)
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
        distance = Vector3.Distance(target.position, rootAnimatedObj.transform.position); 
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
        isAlive = false; 
        jointHandler.SetJointSettings(true);  
        jointHandler.SetJointBones();
        waveModeEvent.Invoke(); 
        this.enabled = false; 
        print("Ded");
    }

    public void RagdollRevive()
    {
        currentRagdollHealth = activeRagdollObject.maxRagdollHealth; 
        this.enabled = true;
        aIStateManager.enabled = true;   
        isAlive = true; 
        jointHandler.SetJointSettings(false);
        jointHandler.SetJointBones();
        print("revive"); 
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, lookRadius); 
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activeRagdollObject.attackDistance);
    }

}
