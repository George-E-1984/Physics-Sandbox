using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInteractionTest : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        print("Held object interact"); 
    }
    public override void StopInteract()
    {
        print("Held object stop interact");
    }
}
