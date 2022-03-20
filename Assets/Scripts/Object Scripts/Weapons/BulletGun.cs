using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun : Weapons
{
    //[Header("Variables"); 
    [Header("Assign - Bullet Gun")]
    public Animator slideAnimator; 
    public GameObject shellReleasePoint; 
    public ParticleSystem muzzleFlash;
    [Header("Info")]
    public RaycastHit shotHit; 
    private ObjectProperties objectProperties;  
    private Vector3 decalLocTran;
    private Rigidbody toolRB;
    private float x; 
    private float y;
    void Start()
    {
        playerGrab = PlayerManager.instance.playerGrab; 
        objectPooler = ObjectPooler.Instance; 
        toolRB = gameObject.GetComponent<Rigidbody>();
        ammoLeftBank = gunOptions.maxAmmoInBank; 
        ammoLeftClip = gunOptions.maxAmmoInClip;
        waitFireRate = new WaitForSeconds(gunOptions.fireRate); 
        waitReloadTime = new WaitForSeconds(gunOptions.reloadTime);
        //Finds the player ui script
        reloadIcon = GameObject.Find("PlayerUI").GetComponent<UiData>().reloadIcon.gameObject;
    }
    void Update()
    {
        
    }
    public override IEnumerator Shoot()
    {
        isShooting = true;    
        if (isAiming)
        {
            x = Random.Range(-gunOptions.aimSpread, gunOptions.aimSpread);
            y = Random.Range(-gunOptions.aimSpread, gunOptions.aimSpread);

        }
        else 
        {
            x = Random.Range(-gunOptions.spread, gunOptions.spread);
            y = Random.Range(-gunOptions.spread, gunOptions.spread);
        }
        Vector3 direction = playerGrab.camPos.forward + new Vector3(x, y, 0);
        Physics.Raycast(playerGrab.playerMovement.shootOrigin.transform.position, direction, out shotHit, gunOptions.bulletDistance);
        if (shotHit.collider != null)
        {
            if (shotHit.collider.GetComponent<ObjectProperties>() != null)
            {
                objectProperties = shotHit.collider.GetComponent<ObjectProperties>(); 
                if (objectProperties.objectRigidbody != null)
                {
                    objectProperties.objectRigidbody.AddForceAtPosition(gunOptions.bulletAppliedForce * shotHit.transform.forward, shotHit.point, ForceMode.Impulse); 
                }
                else
                {
                    Debug.Log("No rigidbody specified!"); 
                }
            }
            if (shotHit.collider.gameObject.tag == "Limb")
            {
                ActiveRagdoll activeRagdoll = shotHit.collider.GetComponentInParent<ActiveRagdoll>();
                activeRagdoll.currentRagdollHealth -= gunOptions.bulletDamage; 
                if (activeRagdoll.currentRagdollHealth <= 0 && activeRagdoll.isAlive)
                {
                    activeRagdoll.RagdollDeath(); 
                }
            }

        }
        GunEffects(); 
        //recoil
        toolRB.AddForceAtPosition(gunOptions.recoilAmount * (-playerGrab.camPos.forward), shootPoint.transform.position, ForceMode.Impulse);   
        //shoot sound effect
        shootAudioSource.pitch = Random.Range(gunOptions.minPitch, gunOptions.maxPitch);
        shootAudioSource.PlayOneShot(shootSFX[Random.Range(0, shootSFX.Length - 1)]);
        
        //slide animation
        if (slideAnimator != null)
        {
            slideAnimator.SetTrigger("SlideBack");
        }

        //reducing ammo amount
        ammoLeftClip--; 
        //time before you can shoot again   
        yield return waitFireRate; 
        isShooting = false; 
        if (playerInputActions.Tool.Shoot.ReadValue<float>() == 1f && shootType == ShootType.Auto && ammoLeftClip > 0)
        {
            StartCoroutine(Shoot()); 
        }
        else if (ammoLeftClip == 0 && ammoLeftBank > 0 && !isReloading)
        {
            StartCoroutine(Reload()); 
        }
    }
    public override IEnumerator Reload()
    {
        isReloading = true; 
        reloadIcon.SetActive(true);
        shootAudioSource.PlayOneShot(reloadSFX[Random.Range(0, reloadSFX.Length - 1)]); 
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
        //muzzle flash 
        if (muzzleFlash)
        {
            muzzleFlash.Play(); 
        }

        if (shotHit.collider != null && shotHit.collider.tag != "Tool" && objectProperties != null)
        {
            //Shoot Decals
            if (objectProperties.colliderEffects.decalPoolName.Length > 0)
            {
                GameObject decal = objectPooler.SpawnFromPool(objectProperties.colliderEffects.decalPoolName, shotHit.point, Quaternion.LookRotation(shotHit.normal));
                if (shotHit.collider.GetComponent<Rigidbody>() != null)
                {
                    decal.transform.parent = shotHit.collider.transform; 
                    decalLocTran = decal.transform.position; 
                }
                if (decal.transform.position != decalLocTran)
                {
                    decal.transform.parent = null; 
                }
            }
            else 
            {
                Debug.Log("No Decal Effects on collider that was shot");
            }

           //Impact effects 
           if (objectProperties.colliderEffects.shootEffectPoolName.Length > 0)
           {
               GameObject impactEff = objectPooler.SpawnFromPool(objectProperties.colliderEffects.shootEffectPoolName, shotHit.point, Quaternion.LookRotation(shotHit.normal));
               impactEff.GetComponent<ParticleSystem>().Play();
           }
           else 
           {
               Debug.Log("No Impact Effects on collider that was shot"); 
           }
         
        }
        //Bullet Shells 
        objectPooler.SpawnFromPool(gunOptions.bulletShellTag, shellReleasePoint.transform.position, playerGrab.camPos.rotation).GetComponent<Rigidbody>().AddForce(shootPoint.transform.right * gunOptions.shellForce, ForceMode.Impulse);
    }
}
