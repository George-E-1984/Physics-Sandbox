using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSettings : MonoBehaviour
{
    public Vector3 positionOffset;
    public Quaternion rotationOffset;

    public ConfigurableJointMotion jointXMotion; 
    
    public ConfigurableJointMotion jointYMotion;
    
    public ConfigurableJointMotion jointZMotion;
    
    public ConfigurableJointMotion jointXAngMotion;

    public ConfigurableJointMotion jointYAngMotion;
   
    public ConfigurableJointMotion jointZAngMotion;



    public bool overrideJointDrives = false;
    
    public JointDrive xJointDrive;
    [Header("XDrive Settings")]
    public float xSpring;
    public float xDamper;
    public float xForce;

    public JointDrive yJointDrive;
    [Header("YDrive Settings")]
    public float ySpring;
    public float yDamper;
    public float yForce;
   
    public JointDrive zJointDrive;
    [Header("ZDrive Settings")]
    public float zSpring;
    public float zDamper;
    public float zForce;

    public JointDrive xAngDrive;
    [Header("XAngDrive Settings")]
    public float angXSpring;
    public float angXDamper;
    public float angXForce;

    public JointDrive yzAngDrive;
    [Header("YZAngDrive Settings")]
    public float angYZSpring;
    public float angYZDamper;
    public float angYZForce;





    //public float ConfigJointDriveXOverride;
    //public float ConfigJointDriveYOverride;
    //public float ConfigJointDriveZOverride;


    public Collider[] grabbedObjectColliders;

    private void Start()
    {
        //xDrive
        xJointDrive.positionSpring = xSpring;
        xJointDrive.positionDamper = xDamper;
        xJointDrive.maximumForce = xForce;

        //yDrive 
        yJointDrive.positionSpring = ySpring;
        yJointDrive.positionDamper = yDamper;
        yJointDrive.maximumForce = yForce;

        //zDrive 
        zJointDrive.positionSpring = zSpring;
        zJointDrive.positionDamper = zDamper;
        zJointDrive.maximumForce = zForce;

        //xAngDrive 
        xAngDrive.positionSpring = angXSpring;
        xAngDrive.positionDamper = angXDamper;
        xAngDrive.maximumForce = angXForce;

        yzAngDrive.positionSpring = angYZSpring;
        yzAngDrive.positionDamper = angYZDamper;
        yzAngDrive.maximumForce = angYZForce; 
    }
}
