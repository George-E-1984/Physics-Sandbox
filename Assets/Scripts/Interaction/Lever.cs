using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable 
{
    [Header("Assign")]
    public ConfigurableJoint handleConfigJoint; 
    public Quaternion startRotation;
    void Start()
    {
        startRotation = handleConfigJoint.transform.localRotation;
    }

    void Update()
    {
        
    }
    public override void Interact()
    {
        HandleLever(); 
    }

    public override void StopInteract()
    {
        
    }

    public void HandleLever()
    {
        if (handleConfigJoint.targetRotation.y == 0 || handleConfigJoint.targetRotation.y == 270)
        {
            //handleConfigJoint.targetRotation = new Quaternion(0, 10, 0, 0);
            handleConfigJoint.targetRotation = new Quaternion(0, 90f, 0, 0);
            print("Hi"); 
        } 
        else if (handleConfigJoint.targetRotation.y == 90)
        {
            handleConfigJoint.targetRotation = new Quaternion(0, 270, 0, 0);
        }
    }
}
