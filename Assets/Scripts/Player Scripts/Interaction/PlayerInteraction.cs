using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerInteraction : MonoBehaviour
{
    public RaycastHit interactHit;
    [Header("Assign")]
    public LayerMask interactableMask; 
    [Header("Variables to set")]
    public float interactionLength = 10f; 
    [Header("Debug")]
    public bool foundInteractable; 
    public PlayerManager playerManager; 
    public Interactable interactable; 
    //Grab stuff
    [HideInInspector] public bool isGrabbing; 
    [HideInInspector] public bool isGrabbingTool;
    void Start()
    {
        //assigning scripts
        playerManager = PlayerManager.instance; 
    }
    void Update() 
    {
        //Shoots a ray looking for objects to interact with
        foundInteractable = Physics.Raycast(playerManager.playerCam.transform.position, playerManager.playerCam.transform.forward, out interactHit, 10f, interactableMask); 
    }

    public void StartInteract(InputAction.CallbackContext context)
    {
        if (foundInteractable)
        {
            //finds the interactable script
            interactable = interactHit.collider.GetComponent<Interactable>(); 
            //if there is an interactable script, fire off the interact method attached to the object
            if (interactable)
            {
                interactable.Interact(); 
            }
            else
            {
                interactable = interactHit.collider.GetComponentInParent<Interactable>(); 
                if (interactable)
                {
                    interactable.Interact(); 
                }
            }
        }
    }
    public void StopInteract(InputAction.CallbackContext context)
    {
        if (interactable)
        {
            interactable.StopInteract(); 
        }
    }
}
