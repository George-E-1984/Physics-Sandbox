using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{
    public ColliderEffects colliderEffects; 
    [Header("Rigidbody Objects settings")]
    public Rigidbody objectRigidbody; 
    public GameObject centreOfMassOverride; 
    public Sprite icon;
    public bool isThisSlottable; 
    public MonoBehaviour[] scriptsToEnableOnGrab; 

    private void Start() 
    {
        if (centreOfMassOverride)
        {
            objectRigidbody.centerOfMass = centreOfMassOverride.transform.position; 
        }

        if (gameObject.tag == "Tool")
        {
            isThisSlottable = true; 
        }
    }



}
