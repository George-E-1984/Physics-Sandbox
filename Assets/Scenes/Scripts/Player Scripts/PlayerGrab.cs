using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 
using UnityEngine.InputSystem; 


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
    public Camera cam; 
    public Transform camPos;
    public float throwForce;
    public Image grabInd;
    public GameObject thePlayer;
    public GameObject grabbedTool;
    [Header("Assign Scripts")]
    public ToolbarManager toolbarManager;
    public PlayerMovement playerMovement;
    public PlayerManager playerManager; 
    public AudioClip grabSound; 
    [Header("Info")]
    public AudioSource grabAudioSource;
    public RaycastHit hitGrab; 
    public GameObject grabbedObject; 
    public ObjectProperties objectProperties; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitGrab = checkForObject();
    }

    private RaycastHit checkForObject()
    {
        Physics.Raycast(camPos.position, camPos.forward, out hitGrab, 10f, grabbableMask);
        return hitGrab;      
    }

    public void GrabObject(GameObject objectToGrab)
    {
        print("grabbed object"); 
        grabbedObject = objectToGrab;
        objectProperties = grabbedObject.GetComponent<ObjectProperties>();  
        //grab sound effect
        grabAudioSource = objectToGrab.AddComponent<AudioSource>(); 
        grabAudioSource.PlayOneShot(grabSound);  
        //important variables setting!     
        grabSettings = objectToGrab.transform.gameObject.GetComponent<GrabSettings>();
        if (grabSettings.grabRigidbody)
        {
            grabbedObjectRb = grabSettings.grabRigidbody; 
        }
        else
        {
            grabbedObjectRb = objectToGrab.transform.gameObject.GetComponent<Rigidbody>();
        }
        //object doesn't collide with player
        for(int i = 0; i < grabSettings.grabbedObjectColliders.Length; i++ )
        {
            Physics.IgnoreCollision(grabSettings.grabbedObjectColliders[i], playerCollider, true);           
        }
        if (grabbedObject.GetComponent<ObjectProperties>() != null)
        {
            ObjectProperties objectProperties = grabbedObject.GetComponent<ObjectProperties>(); 
            for(int i = 0; i < objectProperties.scriptsToEnableOnGrab.Length; i++)
            {
                objectProperties.scriptsToEnableOnGrab[i].enabled = true; 
            }

        }
        //changing values of config to the grab settings ones
        //position offset
        grabHolderConfig.anchor = grabSettings.positionOffset;
        if (grabSettings.grabPoint && !objectProperties.dynamicGrabPoints)
        {
            grabHolderConfig.connectedAnchor = grabSettings.grabPoint.transform.localPosition;
        } 
        else if (objectProperties.dynamicGrabPoints && !objectProperties.isThisSlottable && grabSettings.grabPoint)
        {
            grabSettings.grabPoint.transform.position = hitGrab.point;
            grabHolderConfig.connectedAnchor = grabSettings.grabPoint.transform.localPosition; 
        }
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

    public void CheckForGrab(InputAction.CallbackContext context)
    {
        //tool grab
        if (hitGrab.collider != null && hitGrab.collider.gameObject.tag == "Tool" && hitGrab.collider.gameObject != toolbarManager.items[toolbarManager.currentlySelected])
        {
            if (toolbarManager.items[toolbarManager.currentlySelected] != null && toolbarManager.currentToolScript && toolbarManager.currentToolScript.isReloading == true)
            {
                //toolbarManager.currentToolScript.reloadTimer.Stop(); 
                toolbarManager.currentToolScript.isReloading = false;
                toolbarManager.currentToolScript.reloadIcon.SetActive(false);
            }
            hitGrab.collider.gameObject.transform.rotation = playerMovement.playerCamera.rotation;
            GrabObject(hitGrab.collider.gameObject);
            if (objectProperties != null && objectProperties.isThisSlottable)
            {
                toolbarManager.AddItem(grabbedObject);
            }
            hitGrab.collider.gameObject.GetComponent<Tool>().enabled = true;
            print("Grabbed Tool");
            isGrabbingTool = true;
            grabbedTool = hitGrab.collider.gameObject;
            playerMovement.shootOrigin.transform.localPosition = new Vector3(0f, 0f, toolbarManager.currentToolScript.gunOptions.shootPointOffset); 

        }
        //object grab
        else if (hitGrab.collider != null && !isGrabbing && !isGrabbingTool & hitGrab.collider != playerMovement.hit.collider)
        {
            hitGrab.collider.gameObject.transform.rotation = playerMovement.playerCamera.rotation;
            isGrabbing = true;
            GrabObject(hitGrab.collider.gameObject);
            if (grabbedObject.GetComponent<ObjectProperties>())
            {
                if(grabbedObject.GetComponent<ObjectProperties>().isThisSlottable && hitGrab.collider.gameObject != toolbarManager.items[toolbarManager.currentlySelected])
                {
                    toolbarManager.AddItem(grabbedObject);
                }
            }
            print("Grabbed Object");
        }   
        // else if (isGrabbing && !isGrabbingTool && grabSettings.canBeThrown)
        // {
        //     if (objectProperties == null || !objectProperties.isThisSlottable)
        //     {
        //         ReleaseObject(true); 
        //     }
        // }
        // else if (isGrabbing && playerManager.playerInputActions.Player.Grabbing.ReadValue<float>() == -1 && !isGrabbingTool)
        // {
        //     if (objectProperties == null || !objectProperties.isThisSlottable)
        //     {
        //         ReleaseObject(false);
        //     }
        // }
    }
    public void StartReleaseObject(InputAction.CallbackContext context)
    {
        ReleaseObject();
    }
    public void ReleaseObject()
    {
        if (isGrabbing && !grabbedObject.GetComponent<ObjectProperties>().isThisSlottable || isGrabbingTool && playerManager.playerInputActions.Player.TaskbarRelease.ReadValue<float>() == 1)
        {
            if(grabAudioSource != null)
            {
                Destroy(grabAudioSource);
            } 
            for (int i = 0; i < grabSettings.grabbedObjectColliders.Length; i++)
            {
                Physics.IgnoreCollision(grabSettings.grabbedObjectColliders[i], playerCollider, false);
            }
            if (grabbedObject.GetComponent<ObjectProperties>() != null)
            {
                ObjectProperties objectProperties = grabbedObject.GetComponent<ObjectProperties>(); 
                for(int i = 0; i < objectProperties.scriptsToEnableOnGrab.Length; i++)
                {
                    objectProperties.scriptsToEnableOnGrab[i].enabled = false; 
                }
            }
            grabHolderConfig.connectedBody = null;
            //grabbedObjectRb.AddForce((System.Convert.ToUInt16(isThrowing)) * camPos.transform.forward * throwForce, ForceMode.Impulse);
            grabbedObjectRb = null;
            isGrabbing = false; 
            grabbedObject = null; 

        }
    }
}


