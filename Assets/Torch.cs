using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private bool isTorchOn = false; 
    public Light torchLight; 
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTorchOn)
        {
            isTorchOn = true; 
            torchLight.enabled = true; 
        }
        else if (Input.GetMouseButtonDown(0) && isTorchOn)
        {
            isTorchOn = false; 
            torchLight.enabled = false; 
        }
    }
}
