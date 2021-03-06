using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 
using UnityEngine.InputSystem; 


public class PlayerInteract : MonoBehaviour 

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
    public GameObject grabHolder; 
    [Header("Assign Scripts")]
    public ToolbarManager toolbarManager;
    public PlayerMovement playerMovement;
    public PlayerManager playerManager; 
    public UiData uiData; 
    public AudioClip grabSound; 
    [Header("String Variables")]
    public string buttonTagName; 
    [Header("Info")]
    public AudioSource grabAudioSource;
    public RaycastHit hitGrab; 
    public RaycastHit hitButton; 
    public GameObject grabbedObject; 
    public ObjectProperties objectProperties; 
    public Button buttonScript; 
    public bool isPressingButton; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitGrab = checkForObject();
        hitButton = CheckForButton(); 
    }

    private RaycastHit checkForObject()
    {
        Physics.Raycast(camPos.position, camPos.forward, out hitGrab, 10f, grabbableMask);
        return hitGrab;      
    }
    private RaycastHit CheckForButton()
    {
        Physics.Raycast(camPos.position, camPos.forward, out hitButton, 4f);
        if (hitButton.collider && hitButton.collider.tag == buttonTagName)
        {
           uiData.crosshair.SetActive(false);
           uiData.handIcons.SetActive(true);
        }
        else if (uiData.handIcons.activeSelf)
        {
           uiData.handIcons.SetActive(false);
           uiData.crosshair.SetActive(true);
        }
        return hitButton; 
    }

    public IEnumerator GrabObject(GameObject objectToGrab)
    {
        print("grabbed object"); 
        //disables colliders on the object 
        grabSettings = objectToGrab.transform.gameObject.GetComponent<GrabSettings>();
        for (int i = 0; i < grabSettings.grabbedObjectColliders.Length; i++)
        {
            grabSettings.grabbedObjectColliders[i].enabled = false; 
        }
        grabbedObject = objectToGrab;
        objectProperties = grabbedObject.GetComponent<ObjectProperties>();  
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
            Physics.IgnoreCollision(grabSettings.grabbedObjectColliders[i], playerCollider, true);           
        }
    
        for(int i = 0; i < grabSettings.scriptsToEnable.Length; i++)
        {
            grabSettings.scriptsToEnable[i].enabled = true; 
        }
        
        //changing values of config to the grab settings ones
        //position offset
        grabHolderConfig.anchor = grabSettings.positionOffset;
        if (grabSettings.grabOptions == GrabSettings.GrabOptions.grabPoint)
        {
            grabHolderConfig.connectedAnchor = grabSettings.grabPoint.transform.localPosition;
        } 
        else if (grabSettings.grabOptions == GrabSettings.GrabOptions.dynamicGrabPoints)
        {
            grabSettings.grabPoint.transform.position = hitGrab.point;
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
            StartCoroutine(GrabObject(hitGrab.collider.gameObject));
            if (grabSettings.isThisSlottable)
            {
                toolbarManager.AddItem(grabbedObject);
            }
            hitGrab.collider.gameObject.GetComponent<Weapons>().enabled = true;
            print("Grabbed Tool");
            isGrabbingTool = true;
            grabbedTool = hitGrab.collider.gameObject;
            playerMovement.shootOrigin.transform.localPosition = new Vector3(0f, 0f, toolbarManager.currentToolScript.shootPointOffset); 

        }
        //object grab
        else if (hitGrab.collider != null && !isGrabbing && !isGrabbingTool & hitGrab.collider != playerMovement.hit.collider)
        {
            hitGrab.collider.gameObject.transform.rotation = playerMovement.playerCamera.rotation;
            isGrabbing = true;
            StartCoroutine(GrabObject(hitGrab.collider.gameObject));
            if (grabbedObject.GetComponent<ObjectProperties>())
            {
                if(grabSettings.isThisSlottable && hitGrab.collider.gameObject != toolbarManager.items[toolbarManager.currentlySelected])
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
        if (isGrabbing && !grabSettings.isThisSlottable || isGrabbingTool && playerManager.playerInputActions.Player.TaskbarRelease.ReadValue<float>() == 1)
        {
            if(grabAudioSource != null)
            {
                Destroy(grabAudioSource);
            } 
            
            for (int i = 0; i < grabSettings.grabbedObjectColliders.Length; i++)
            {
                Physics.IgnoreCollision(grabSettings.grabbedObjectColliders[i], playerCollider, false);
                grabSettings.grabbedObjectColliders[i].enabled = true; 
            }
     
            for(int i = 0; i < grabSettings.scriptsToEnable.Length; i++)
            {
                grabSettings.scriptsToEnable[i].enabled = false; 
            }
            
            grabHolderConfig.connectedBody = null;
            //grabbedObjectRb.AddForce((System.Convert.ToUInt16(isThrowing)) * camPos.transform.forward * throwForce, ForceMode.Impulse);
            grabbedObjectRb = null;
            isGrabbing = false; 
            grabbedObject = null; 

        }
        
    }
    
    public void ButtonPress(InputAction.CallbackContext context)
    { 
        if (hitButton.collider && hitButton.collider.tag == buttonTagName)
        {
            buttonScript = hitButton.collider.GetComponentInParent<Button>(); 
            buttonScript.buttonJoint.targetPosition = new Vector3(0, buttonScript.downThreshold, 0);
            isPressingButton = true;  
        }
    }
    // public void StartButtonDepress(InputAction.CallbackContext context)
    // {
    //     ButtonDepress(); 
    // }
    public void ButtonDepress(InputAction.CallbackContext context)
    { 
        if (isPressingButton)
        {
            buttonScript.buttonJoint.targetPosition = new Vector3(0, 0 ,0); 
            isPressingButton = false; 
        }
    }
}


