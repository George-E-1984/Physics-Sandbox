using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable
{
    [Header("Assign")]
    public AudioClip grabSound; 
    public AudioSource grabAudioSource; 
    [Header("Debug Vars")]
    public GameObject grabbedObject; 
    public GameObject grabbedTool; 
    public Rigidbody grabbedObjectRb; 
    public PlayerManager playerManager; 
    public PlayerInteraction playerInteraction; 
    public ToolbarManager toolbarManager; 
    public GrabSettings grabSettings; 
    public ConfigurableJoint grabHolderConfig; 
    void Start()
    {
        //Stores the player manager
        playerManager = PlayerManager.instance; 
        playerInteraction = playerManager.playerInteraction; 
        toolbarManager = playerManager.toolbarManager; 
        grabHolderConfig = playerManager.grabHolderConfig; 
        grabSettings = this.GetComponent<GrabSettings>(); 
    }
    void Update()
    {
        
    }
    public override void Interact()
    {
        print("Interacted"); 
        //tool grab
        if (playerManager.playerInteraction.interactHit.collider.gameObject.tag == "Tool" && playerManager.playerInteraction.interactHit.collider.gameObject != toolbarManager.items[toolbarManager.currentlySelected])
        {
            if (toolbarManager.items[toolbarManager.currentlySelected] != null && toolbarManager.currentToolScript && toolbarManager.currentToolScript.isReloading == true)
            {
                //toolbarManager.currentToolScript.reloadTimer.Stop(); 
                toolbarManager.currentToolScript.isReloading = false;
                toolbarManager.currentToolScript.reloadIcon.SetActive(false);
            }
            playerManager.playerInteraction.interactHit.collider.gameObject.transform.rotation = playerManager.playerMovement.playerCamera.rotation;
            StartCoroutine(GrabObject(playerManager.playerInteraction.interactHit.collider.gameObject));
            if (grabSettings.isThisSlottable)
            {
                toolbarManager.AddItem(grabbedObject);
            }
            playerManager.playerInteraction.interactHit.collider.gameObject.GetComponent<Weapons>().enabled = true;
            print("Grabbed Tool");
            playerInteraction.isGrabbingTool = true;
            grabbedTool = playerManager.playerInteraction.interactHit.collider.gameObject;
            playerManager.playerMovement.shootOrigin.transform.localPosition = new Vector3(0f, 0f, toolbarManager.currentToolScript.shootPointOffset); 

        }
        //object grab
        else if (playerManager.playerInteraction.interactHit.collider != null && !playerManager.playerInteraction.isGrabbing && !playerManager.playerInteraction.isGrabbingTool & playerManager.playerInteraction.interactHit.collider != playerManager.playerMovement.hit.collider)
        {
            playerManager.playerInteraction.interactHit.collider.gameObject.transform.rotation = playerManager.playerMovement.playerCamera.rotation;
            playerInteraction.isGrabbing = true;
            StartCoroutine(GrabObject(playerManager.playerInteraction.interactHit.collider.gameObject));
            if (grabbedObject.GetComponent<ObjectProperties>())
            {
                if(grabSettings.isThisSlottable && playerManager.playerInteraction.interactHit.collider.gameObject != toolbarManager.items[toolbarManager.currentlySelected])
                {
                    toolbarManager.AddItem(grabbedObject);
                }
            }
            print("Grabbed Object");
        }  
    }
    public override void StopInteract()
    {
        if (playerInteraction.grabbedObject == this.gameObject)
        {
            ReleaseObject(); 
        }
    }
    public IEnumerator GrabObject(GameObject objectToGrab)
    {
        print("grabbed object"); 
        //disables colliders on the object 
        for (int i = 0; i < grabSettings.grabbedObjectColliders.Length; i++)
        {
            grabSettings.grabbedObjectColliders[i].enabled = false; 
        }
        grabbedObject = objectToGrab;
        playerInteraction.grabbedObject = objectToGrab; 
        ObjectProperties objectProperties = grabbedObject.GetComponent<ObjectProperties>();  
        //grab sound effect
        grabAudioSource = objectToGrab.AddComponent<AudioSource>(); 
        grabAudioSource.PlayOneShot(grabSound);      
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
            Physics.IgnoreCollision(grabSettings.grabbedObjectColliders[i], playerManager.playerCollider, true);           
        }
    
        for(int i = 0; i < grabSettings.scriptsToEnable.Length; i++)
        {
            grabSettings.scriptsToEnable[i].enabled = true; 
        }
        
        //changing values of config to the grab settings ones
        //position offset
        grabHolderConfig.anchor = grabSettings.positionOffset;
        if (grabSettings.grabOptions == GrabSettings.GrabOptions.grabPoint && grabSettings.grabPoint)
        {
            grabHolderConfig.connectedAnchor = grabSettings.grabPoint.transform.localPosition;
        } 
        else if (grabSettings.grabOptions == GrabSettings.GrabOptions.dynamicGrabPoints && grabSettings.grabPoint)
        {
            grabSettings.grabPoint.transform.position = playerInteraction.interactHit.point;
            grabHolderConfig.connectedAnchor = grabSettings.grabPoint.transform.localPosition; 
        }
        else 
        {
            grabHolderConfig.connectedAnchor = new Vector3(0, 0, 0); 
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
        yield return new WaitForSeconds(1);
        //Enables colliders on the object 
        for (int i = 0; i < grabSettings.grabbedObjectColliders.Length; i++)
        {
            grabSettings.grabbedObjectColliders[i].enabled = true; 
        }
    }
    public void ReleaseObject()
    {
        if (playerManager.playerInteraction && !grabSettings.isThisSlottable || playerManager.playerInteraction && playerManager.playerInputActions.Player.TaskbarRelease.ReadValue<float>() == 1)
        {
            if(grabAudioSource != null)
            {
                Destroy(grabAudioSource);
            } 
            
            for (int i = 0; i < grabSettings.grabbedObjectColliders.Length; i++)
            {
                Physics.IgnoreCollision(grabSettings.grabbedObjectColliders[i], playerManager.playerCollider, false);
                grabSettings.grabbedObjectColliders[i].enabled = true; 
            }
     
            for(int i = 0; i < grabSettings.scriptsToEnable.Length; i++)
            {
                grabSettings.scriptsToEnable[i].enabled = false; 
            }
            
            grabHolderConfig.connectedBody = null;
            //grabbedObjectRb.AddForce((System.Convert.ToUInt16(isThrowing)) * camPos.transform.forward * throwForce, ForceMode.Impulse);
            grabbedObjectRb = null;
            playerInteraction.isGrabbing = false; 
            grabbedObject = null; 

        }
        
    }
}
