using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class ActiveRagdoll : MonoBehaviour
{
    [Header("Assignables")]
    public JointHandler jointHandler; 
    public ActiveRagdollPresets activeRagdollObject; 
    public GameObject physicsRig; 
    public GameObject animatedRig;
    public NavMeshAgent navMeshAgent; 
    public Collider visionCone; 
    public PlayerData playerData; 
    public Animator animator;

    [Header("Variables")]
    public float lookRadius = 10f;

    [Header("Info (Do not touch)")]
    public Transform target; 
    public float distance; 
    public bool fallen; 
    

    // Start is called before the first frame update
    void Start()
    {
        target = SceneMaster.instance.player.transform; 
    }

    // Update is called once per frame
    void Update()
    {
        //fallover conditional, sets the joints to have no drive if the physics rig is a big enough distance from the animated one
        if (Vector3.Distance(physicsRig.transform.position, animatedRig.transform.position) > activeRagdollObject.falloverDistance)
        {
            HandleFalling(true);
        } 
    }

    public void HandleFalling(bool hasFallen)
    {
        if (hasFallen)
        {
            jointHandler.SetJointSettings(true);
            jointHandler.SetJointBones(); 
            fallen = true; 
        }
        else 
        {
            fallen = false; 
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

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, lookRadius); 
    }

}
