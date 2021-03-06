using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReanimateCannon : Weapons
{
    [Header("Variables - Reanimate Cannon")]
    public float forceTome; 
    public float forceSpeed;
    [Header("Assign")]
    public GameObject forceShotPrefab; 
    [Header("Info")]
    private Rigidbody toolRB; 
    private GameObject forceShot; 
    private Vector3 firstRot;
    private Transform shootOrigin;
    void Start() 
    {
        playerGrab = PlayerManager.instance.playerGrab; 
        objectPooler = ObjectPooler.Instance; 
        toolRB = gameObject.GetComponent<Rigidbody>();
        ammoLeftBank = maxAmmoInBank; 
        ammoLeftClip = maxAmmoInClip;
        waitFireRate = new WaitForSeconds(fireRate); 
        if (reloadAnimator)
        {
            waitReloadTime = new WaitForSeconds(reloadAnimation.length);
        }
        else
        {
            waitReloadTime = new WaitForSeconds(reloadTime);
        }
        //Finds the player ui script
        reloadIcon = GameObject.Find("PlayerUI").GetComponent<UiData>().reloadIcon.gameObject;
        //Finds the force shot
        forceShot = Instantiate(forceShotPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)); 
        forceShot.SetActive(false); 
        shootOrigin = playerGrab.playerMovement.shootOrigin.transform;
    }
    void Update() 
    {
        
    }
    //Override Methods 
    public override IEnumerator Shoot()
    {
        forceShot.SetActive(true); 
        isShooting = true; 
        firstRot = playerGrab.camPos.forward;
        forceShot.transform.position = shootOrigin.position;
        forceShot.transform.rotation = shootOrigin.rotation;
        //force to add to the weapon when you shoot; 
        toolRB.AddForceAtPosition(recoilAmount * (-shootPoint.transform.forward), shootPoint.transform.position, ForceMode.Impulse);
        //muzzleFlash.Play();
        shootAudioSource.PlayOneShot(shootSFX[Random.Range(0, shootSFX.Length - 1)]);
        ammoLeftClip--;
        for (int i = 0; i < forceTome * 60; i++)
        {
            forceShot.transform.Translate(firstRot * forceSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        forceShot.SetActive(false);  
        yield return waitFireRate; 
        isShooting = false; 
    }
    public override IEnumerator Reload()
    {
        isReloading = true; 
        reloadIcon.SetActive(true);
        shootAudioSource.PlayOneShot(reloadSFX[Random.Range(0, reloadSFX.Length - 1)]); 
        if (reloadAnimator)
        {
            reloadAnimator.SetTrigger("Reload"); 
        }
        yield return waitReloadTime; 
        reloadIcon.SetActive(false);
        isReloading = false; 
        if ((maxAmmoInClip - ammoLeftClip) >= ammoLeftBank)
        {
            ammoLeftClip = ammoLeftClip + ammoLeftBank;
            ammoLeftBank = 0; 
        }
        else if ((maxAmmoInClip - ammoLeftClip) < ammoLeftBank)
        {
            ammoLeftBank = ammoLeftBank - (maxAmmoInClip - ammoLeftClip);
            ammoLeftClip = ammoLeftClip + (maxAmmoInClip - ammoLeftClip);
         
        }
    }
    public override void GunEffects()
    {
        
    }
    void OnDisable() 
    {
        if (forceShot.activeSelf == true)
        {
           forceShot.SetActive(false); 
        }
    }
}
