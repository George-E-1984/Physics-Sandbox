using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Weapons
{
    [Header("Assign")]
    [SerializeField] GameObject rocketPrefab; 
    [Header("Variables")]
    public float rocketForce; 
    [Header("Info")]
    GameObject rocket; 
    Transform shootOrigin;
    Rigidbody toolRB; 
    void Start()
    { 
        reloadIcon = GameObject.Find("PlayerUI").GetComponent<UiData>().reloadIcon.gameObject;
        playerGrab = PlayerManager.instance.playerGrab; 
        objectPooler = ObjectPooler.Instance; 
        ammoLeftBank = gunOptions.maxAmmoInBank; 
        ammoLeftClip = gunOptions.maxAmmoInClip;
        waitFireRate = new WaitForSeconds(gunOptions.fireRate); 
        if (reloadAnimator)
        {
            waitReloadTime = new WaitForSeconds(reloadAnimation.length);
        }
        else
        {
            waitReloadTime = new WaitForSeconds(gunOptions.reloadTime);
        }
        shootOrigin = playerGrab.playerMovement.shootOrigin.transform;
        toolRB = gameObject.GetComponent<Rigidbody>(); 
    }
    void Update()
    {
        
    }

    public override IEnumerator Shoot()
    {
        isShooting = true; 
        rocket = objectPooler.SpawnFromPool("Rocket", shootPoint.transform.position, shootOrigin.rotation);
        Rigidbody rocketRB = rocket.GetComponent<Rigidbody>();
        if (!rocket.activeSelf)
        {
            rocket.SetActive(true); 
        }
        rocketRB.AddForce(shootOrigin.forward * rocketForce, ForceMode.Impulse); 
        toolRB.AddForceAtPosition(gunOptions.recoilAmount * (-shootPoint.transform.forward), shootPoint.transform.position, ForceMode.Impulse);
        shootAudioSource.PlayOneShot(shootSFX[Random.Range(0, shootSFX.Length - 1)]);
        ammoLeftClip--;
        yield return waitFireRate; 
        isShooting = false; 
    }
    public override IEnumerator Reload()
    {
        isReloading = true; 
        reloadIcon.SetActive(true);
        if (reloadAnimator)
        {
            reloadAnimator.SetTrigger("Reload"); 
        }
        yield return waitReloadTime; 
        reloadIcon.SetActive(false);
        isReloading = false; 
        if ((gunOptions.maxAmmoInClip - ammoLeftClip) >= ammoLeftBank)
        {
            ammoLeftClip = ammoLeftClip + ammoLeftBank;
            ammoLeftBank = 0; 
        }
        else if ((gunOptions.maxAmmoInClip - ammoLeftClip) < ammoLeftBank)
        {
            ammoLeftBank = ammoLeftBank - (gunOptions.maxAmmoInClip - ammoLeftClip);
            ammoLeftClip = ammoLeftClip + (gunOptions.maxAmmoInClip - ammoLeftClip);
         
        }
    }
    public override void GunEffects()
    {
        
    }
}
