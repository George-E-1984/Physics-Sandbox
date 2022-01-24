using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdoll : MonoBehaviour
{
    public JointHandler jointHandler; 
    public ActiveRagdollPresets activeRagdollObject; 
    public GameObject physicsRig; 
    public GameObject animatedRig;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(physicsRig.transform.position, animatedRig.transform.position) > activeRagdollObject.falloverDistance)
        {
            jointHandler.SetJointSettings(true);
            jointHandler.SetJointBones();
        }
    }
}
