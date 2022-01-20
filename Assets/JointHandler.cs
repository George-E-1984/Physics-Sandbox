using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointHandler : MonoBehaviour
{
    public ConfigurableJoint[] armJoints; 
    public JointDrive armJointDrive; 
    public float armJointSpring;
    public float armJointDamper; 
    public float armMaxForce = 200f; 
    public ConfigurableJoint[] legJoints;
    public JointDrive legJointDrive; 
    public float legJointSpring; 
    public float legJointDamper;  
    public float legMaxForce = 200f;
    public ConfigurableJoint[] spineJoints;
    public JointDrive spineJointDrive;
    public float spineJointSpring; 
    public float spineJointDamper;  
    public float spineMaxForce = 200f;
    public ConfigurableJoint[] neckAndHeadJoints; 
    public JointDrive neckAndHeadJointDrive;
    public float neckAndHeadJointSpring; 
    public float neckAndHeadJointDamper; 
    public float neckAndHeadMaxForce = 200f;


    void Start() 
    {
       SetJointSettings(false); 
       SetJointBones();
    }

    public void SetJointSettings(bool hasFallen)
    {
        if (hasFallen)
        {
         //arm joint settings
         armJointDrive.positionSpring = 0f; 
         armJointDrive.positionDamper = 0f; 
         armJointDrive.maximumForce = 0f; 

         //Leg joint settings 
         legJointDrive.positionSpring = 0f; 
         legJointDrive.positionDamper = 0f; 
         legJointDrive.maximumForce = 0f; 

         //Spine joint settings 
         spineJointDrive.positionSpring = 0f;
         spineJointDrive.positionDamper = 0f;
         spineJointDrive.maximumForce = 0f; 

         //neck and Head joint settings
         neckAndHeadJointDrive.positionSpring = 0f; 
         neckAndHeadJointDrive.positionDamper = 0f;
         neckAndHeadJointDrive.maximumForce = 0f;
        }
        else
        {
         //arm joint settings
         armJointDrive.positionSpring = armJointSpring; 
         armJointDrive.positionDamper = armJointDamper; 
         armJointDrive.maximumForce = armMaxForce; 

         //Leg joint settings 
         legJointDrive.positionSpring = legJointSpring; 
         legJointDrive.positionDamper = legJointDamper; 
         legJointDrive.maximumForce = legMaxForce; 

         //Spine joint settings 
         spineJointDrive.positionSpring = spineJointSpring;
         spineJointDrive.positionDamper = spineJointDamper;
         spineJointDrive.maximumForce = spineMaxForce; 

         //neck and Head joint settings
         neckAndHeadJointDrive.positionSpring = neckAndHeadJointSpring; 
         neckAndHeadJointDrive.positionDamper = neckAndHeadJointDamper;
         neckAndHeadJointDrive.maximumForce = neckAndHeadMaxForce;

        }
        
    }

    public void SetJointBones()
    {
        //applying joint settings 
        for (int i = 0; i < armJoints.Length; i++)
        {
            //sets drive settings
            armJoints[i].xDrive = armJointDrive; 
            armJoints[i].yDrive = armJointDrive;
            armJoints[i].zDrive = armJointDrive;
            //Angular 
            armJoints[i].angularXDrive = armJointDrive;
            armJoints[i].angularYZDrive = armJointDrive;
        }

        for (int i = 0; i < legJoints.Length; i++)
        {
            //sets drive settings
            legJoints[i].xDrive = legJointDrive; 
            legJoints[i].yDrive = legJointDrive;
            legJoints[i].zDrive = legJointDrive;
            //angular 
            legJoints[i].angularXDrive = legJointDrive;
            legJoints[i].angularYZDrive = legJointDrive;
        }

        for (int i = 0; i < spineJoints.Length; i++)
        {
            //sets drive settings
            spineJoints[i].xDrive = spineJointDrive; 
            spineJoints[i].yDrive = spineJointDrive;
            spineJoints[i].zDrive = spineJointDrive;
            //angular 
            spineJoints[i].angularXDrive = spineJointDrive;
            spineJoints[i].angularYZDrive = spineJointDrive;
        }

        for (int i = 0; i < neckAndHeadJoints.Length; i++)
        {
            //sets drive settings
            neckAndHeadJoints[i].xDrive = neckAndHeadJointDrive; 
            neckAndHeadJoints[i].yDrive = neckAndHeadJointDrive;
            neckAndHeadJoints[i].zDrive = neckAndHeadJointDrive;
            //angular 
            neckAndHeadJoints[i].angularXDrive = neckAndHeadJointDrive;
            neckAndHeadJoints[i].angularYZDrive = neckAndHeadJointDrive;
        }
    }


}
