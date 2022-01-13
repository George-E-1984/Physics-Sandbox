using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 


public class PlayerGrab : MonoBehaviour

{
    [Header("COnfig Joint variables")]
    public ConfigurableJoint grabHolderConfig;
    public GrabSettings grabSettings; 
    public float grabForce;
    public float grabDamper; 
    private Rigidbody grabbedObjectRb;    
    [Header("Bool variables")]
    public bool isGrabbing = false; 
    public bool isGrabbingTool = false; 
    public Collider playerCollider;
    public bool foundObject = false; 
     
    [Header("Assignables")]
    public Transform TargetObjectPosition; 
    public LayerMask grabbableMask;
    public Transform CamPos;
    public float throwForce;
    public Image grabInd;
    public GameObject thePlayer;
    public GameObject grabbedTool;
    [Header("Assign Scripts")]
    public ToolbarManager toolbarManager;
    public PlayerMovement PlayerMovement;
    public AudioClip grabSound; 
    [Header("Info")]
    public AudioSource grabAudioSource;
    public RaycastHit hitGrab; 
    public GameObject grabbedObject; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //player direction

        if (Input.GetMouseButtonDown(1))
        {
            hitGrab = checkForObject();
            //tool grab
            if (hitGrab.collider != null && hitGrab.collider.gameObject.tag == "Tool" && !isGrabbing && hitGrab.collider.gameObject != toolbarManager.Tools[toolbarManager.currentlySelected])
            {
                if (toolbarManager.Tools[toolbarManager.currentlySelected] != null && toolbarManager.currentToolScript.isReloading == true)
                {
                    StopCoroutine(toolbarManager.currentToolScript.Reload());
                    toolbarManager.currentToolScript.isReloading = false;
                    toolbarManager.currentToolScript.reloadIcon.SetActive(false);
                }
                hitGrab.collider.gameObject.transform.rotation = PlayerMovement.playerCamera.rotation;
                GrabObject(hitGrab.collider.gameObject);
                hitGrab.collider.gameObject.GetComponent<Tool>().enabled = true;
                print("Grabbed Tool");
                isGrabbingTool = true;
                grabbedTool = hitGrab.collider.gameObject;
                toolbarManager.AddTool(grabbedTool);

            }
            //object grab
            else if(hitGrab.collider != null && !isGrabbing && !isGrabbingTool & hitGrab.collider != PlayerMovement.hit.collider)
            {
                GrabObject(hitGrab.collider.gameObject);
                isGrabbing = true;
                print("Grabbed Object");
            }           
        }

        else if (isGrabbing && Input.GetMouseButtonUp(1) && !isGrabbingTool)
        {
            ReleaseObject(false);
        }
        else if (isGrabbing && Input.GetMouseButtonDown(0) && !isGrabbingTool)
        {
            //throwing 
            ReleaseObject(true); 
        }
        else if (grabbedObject && PlayerMovement.hit.collider.gameObject)
        {
            print("Cooky stinks"); 
            if (grabbedObject == PlayerMovement.hit.collider.gameObject)
            {
                print("Cooky pooop"); 
                ReleaseObject(false);  
            }
        }
    }

    private RaycastHit checkForObject()
    {
        Physics.Raycast(CamPos.position, CamPos.forward, out hitGrab, 10f, grabbableMask);
        return hitGrab;      
    }

    public void GrabObject(GameObject objectToGrab)
    {
        grabbedObject = objectToGrab; 
        //grab sound effect
        grabAudioSource = objectToGrab.AddComponent<AudioSource>(); 
        grabAudioSource.PlayOneShot(grabSound);  
        //important variables setting!     
         grabbedObjectRb = objectToGrab.transform.gameObject.GetComponent<Rigidbody>();
         grabSettings = objectToGrab.transform.gameObject.GetComponent<GrabSettings>();
        //object doesn't collide with player
        for(int i = 0; i < grabSettings.grabbedObjectColliders.Length; i++ )
        {
            Physics.IgnoreCollision(grabSettings.grabbedObjectColliders[i], playerCollider, true);           
        }
        //changing values of config to the grab settings ones
        //position offset
        grabHolderConfig.anchor = grabSettings.positionOffset;
        //rotation offset
        grabHolderConfig.targetRotation = grabSettings.rotationOffset; 
        //sets the grabbed object as the connected body of the grab holder's joint
        grabHolderConfig.connectedBody = grabbedObjectRb;
        //sets the joint limits (limited, locked, free)
        grabHolderConfig.xMotion = grabSettings.jointXMotion;
        grabHolderConfig.yMotion = grabSettings.jointYMotion;
        grabHolderConfig.zMotion = grabSettings.jointZMotion;
        grabHolderConfig.angularXMotion = grabSettings.jointXAngMotion;
        grabHolderConfig.angularYMotion = grabSettings.jointYAngMotion;
        grabHolderConfig.angularZMotion = grabSettings.jointZAngMotion;
        //set the joint drives
        grabHolderConfig.xDrive = grabSettings.xJointDrive;
        grabHolderConfig.yDrive = grabSettings.yJointDrive;
        grabHolderConfig.zDrive = grabSettings.zJointDrive;
        grabHolderConfig.angularXDrive = grabSettings.xAngDrive;
        grabHolderConfig.angularYZDrive = grabSettings.yzAngDrive; 
    }

    public void ReleaseObject(bool isThrowing = false)
    {
        if(grabAudioSource != null)
        {
            Destroy(grabAudioSource);
        } 
        for (int i = 0; i < grabSettings.grabbedObjectColliders.Length; i++)
        {
            Physics.IgnoreCollision(grabSettings.grabbedObjectColliders[i], playerCollider, false);
        }
        grabHolderConfig.connectedBody = null;
        grabbedObjectRb.AddForce((System.Convert.ToUInt16(isThrowing)) * CamPos.transform.forward * throwForce, ForceMode.Impulse);
        grabbedObjectRb = null;
        isGrabbing = false; 
        grabbedObject = null; 
    }
}


