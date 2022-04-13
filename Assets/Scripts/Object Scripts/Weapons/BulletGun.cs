using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun : Weapons
{
    //[Header("Variables"); 
    [Header("Assign - Bullet Gun")] 
    public GameObject shellReleasePoint; 
    public ParticleSystem muzzleFlash;
    [Header("Variables to set")]
    public float bulletDistance = 100; 
    public float bulletAppliedForce = 10f; 
    public float shellForce = 5f; 
    public float spread = 0.1f; 
    public float aimSpread = 0.01f;   
    public int bulletDamage = 2; 
    public string bulletShellTag; 
 
    [Header("Info")]
    public RaycastHit shotHit; 
    private ObjectProperties objectProperties;  
    private Vector3 decalLocTran;
    private Rigidbody toolRB;
    private float x; 
    private float y;
    void Start()
    {
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
    }
    void Update()
    {
        
    }
    public override IEnumerator Shoot()
    {
        isShooting = true;    
        if (isAiming)
        {
            x = Random.Range(-aimSpread, aimSpread);
            y = Random.Range(-aimSpread, aimSpread);

        }
        else 
        {
            x = Random.Range(-spread, spread);
            y = Random.Range(-spread, spread);
        }
        Vector3 direction = PlayerManager.instance.playerCam.transform.forward + new Vector3(x, y, 0);
        Physics.Raycast(PlayerManager.instance.playerMovement.shootOrigin.transform.position, direction, out shotHit, bulletDistance);
        if (shotHit.collider != null)
        {
            Rigidbody objRB = shotHit.collider.GetComponent<Rigidbody>(); 
            if (objRB)
            {
                objRB.AddForceAtPosition(bulletAppliedForce * shotHit.transform.forward, shotHit.point, ForceMode.Impulse); 
            }
            if (shotHit.collider.gameObject.tag == "Limb")
            {
                ActiveRagdoll activeRagdoll = shotHit.collider.GetComponentInParent<ActiveRagdoll>();
                activeRagdoll.currentRagdollHealth -= bulletDamage; 
                if (activeRagdoll.currentRagdollHealth <= 0 && activeRagdoll.isAlive)
                {
                    activeRagdoll.RagdollDeath(); 
                }
            }

            objectProperties = shotHit.collider.GetComponent<ObjectProperties>(); 
        }
        GunEffects(); 
        //recoil
        toolRB.AddForceAtPosition(recoilAmount * (-PlayerManager.instance.playerCam.transform.forward), shootPoint.transform.position, ForceMode.Impulse);   
        //shoot sound effect
        shootAudioSource.pitch = Random.Range(minPitch, maxPitch);
        shootAudioSource.PlayOneShot(shootSFX[Random.Range(0, shootSFX.Length - 1)]);
        
        //slide animation
        if (reloadAnimator != null)
        {
            reloadAnimator.SetTrigger("SlideBack");
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
        objectPooler.SpawnFromPool(bulletShellTag, shellReleasePoint.transform.position, PlayerManager.instance.playerCam.transform.rotation).GetComponent<Rigidbody>().AddForce(shootPoint.transform.right * shellForce, ForceMode.Impulse);
    }
}
