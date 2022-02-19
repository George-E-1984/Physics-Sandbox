using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private bool isTorchOn = false; 
    public Light torchLight; 
    [Header("Torch SFX")] 
    public AudioSource audioSource; 
    public AudioClip torchSwitchSfx; 
    void Start()
    {
        isTorchOn = torchLight.isActiveAndEnabled; 
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTorchOn)
        {
            HandleTorch(true); 
        }
        else if (Input.GetMouseButtonDown(0) && isTorchOn)
        {
            HandleTorch(false); 
        }

        void HandleTorch(bool isTorchActive)
        {
            isTorchOn = isTorchActive; 
            audioSource.PlayOneShot(torchSwitchSfx);
            torchLight.enabled = isTorchActive; 
        }
    }
}
