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
    public GameObject physicsRig; 
    public GameObject animatedRig;
    public GameObject feetPoint; 
    public NavMeshAgent navMeshAgent; 
    public Collider visionCone; 
    public PlayerData playerData; 
    public Animator animator;

    [Header("Variables")]
    public float lookRadius = 10f;
    public bool isGrounded; 

    [Header("Info (Do not touch)")]
    public Transform target; 
    public float distance;  
    public bool isGettingUp; 
    public RaycastHit hit; 
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public bool HandleFalling()
    {
        if (Vector3.Distance(physicsRig.transform.position, animatedRig.transform.position) > activeRagdollObject.falloverDistance && !isGettingUp)
        {
            jointHandler.SetJointSettings(true);
            jointHandler.SetJointBones();
            print("fallen"); 
            return true;  
        }
        else
        {
            print("Notfallen"); 
            return false; 
        }
      
    }

    public bool LookForPlayer()
    {
        distance = Vector3.Distance(target.position, transform.position); 
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
        Physics.Raycast(feetPoint.transform.position, Vector3.down, out hit, 0.1f);
        
        if (hit.collider != null)
        {
            isGrounded = true;
            return true; 
        } 
        else 
        {
            isGrounded = false; 
            return false; 
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, lookRadius); 
    }

}
