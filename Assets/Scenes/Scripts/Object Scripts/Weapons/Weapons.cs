using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 
public abstract class Weapons : MonoBehaviour
{
    //public stuff
    [Header("Variables")]
    public ShootType shootType; 
    [Header("Assignables")]
    public ItemData gunOptions;
    public GrabSettings grabSettings; 
    public ParticleSystem muzzleFlash;
    public GameObject shootPoint; 
    public AudioSource shootAudioSource;
    public AudioClip[] shootSFX;
    public AudioClip[] reloadSFX;
     
    [Header("Info")]
    public PlayerInputActions playerInputActions; 
    public PlayerInteract playerGrab;  
    public bool isReloading = false; 
    public bool isShooting = false; 
    public bool isAiming = false; 
    public enum ShootType {SemiAuto, Auto, Burst}; 
    public int ammoLeftBank; 
    public int ammoLeftClip; 
    public WaitForSeconds waitFireRate; 
    public WaitForSeconds waitReloadTime;
    public GameObject reloadIcon;
    //abstract methods
    public abstract IEnumerator Shoot();
    public abstract IEnumerator Reload(); 
    public abstract void GunEffects(); 
    //Non Abstract Methods
    public void StartShot(InputAction.CallbackContext context)
    {
        if (!isShooting && !isReloading && ammoLeftClip > 0)
        {
            StartCoroutine(Shoot());   
        }
    }
    public void StartReload(InputAction.CallbackContext context)
    {
        if (!isReloading && !isShooting && ammoLeftClip != gunOptions.maxAmmoInClip && ammoLeftBank > 0)
        {
            StartCoroutine(Reload()); 
        }
    }
    public void AimDownSights(InputAction.CallbackContext context)
    {
        if (gunOptions.canAim && playerGrab.cam.fieldOfView == playerGrab.playerMovement.playerData.fieldOfView)
        {
           isAiming = true; 
           playerGrab.cam.fieldOfView = Mathf.Lerp(playerGrab.cam.fieldOfView, gunOptions.aimFov, gunOptions.timeToAim);
           playerGrab.grabHolderConfig.anchor = grabSettings.aimPositionOffset; 
           playerGrab.grabHolderConfig.targetRotation = new Quaternion(0,0,0,0); 
        }
    }
    public void StopAim()
    {
        isAiming = false; 
        playerGrab.cam.fieldOfView = playerGrab.playerMovement.playerData.fieldOfView; 
        playerGrab.grabHolderConfig.anchor = playerGrab.grabSettings.positionOffset; 
        playerGrab.grabHolderConfig.targetRotation = playerGrab.grabSettings.rotationOffset;
    }
    public void StopAimStart(InputAction.CallbackContext context)
    {
        StopAim(); 
    }
    public void SetInputs(bool setEnable)
    {
        if (setEnable)
        {
            playerInputActions = new PlayerInputActions();
            playerInputActions.Tool.Enable();
            //shooting
            playerInputActions.Tool.Shoot.performed += StartShot;
            //reloading
            playerInputActions.Tool.Reload.performed += StartReload; 
            //aiming
            playerInputActions.Tool.Aim.performed += AimDownSights; 
            playerInputActions.Tool.Aim.canceled += StopAimStart; 
        }
        else 
        {
            playerInputActions.Tool.Disable();
        }
    }
}


