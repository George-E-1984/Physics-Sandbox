using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using UnityEngine.UI; 

public class Tool : MonoBehaviour
{
    [Header("Assign")]
    public ItemData gunOptions; 
    public GameObject shootPoint;
    public Animator slideAnimator;
    public GameObject shellReleasePoint; 
    public ParticleSystem muzzleFlash; 

    [Header("Options")]
    public ShootType shootType; 

    [Header("Audio Stuff")]
    public AudioSource shootAudioSource;
    public AudioClip[] shootSFX;
    public AudioClip[] reloadSFX; 
 

    [Header("Info (do not touch these values)")]
    public int ammoLeftBank;
    public int ammoLeftClip; 
    public bool isReloading;
    public bool isShooting; 
    public bool canShoot = false; 
    public bool canReload = false;  
    private RaycastHit shotHit;
    private Rigidbody toolRB;
    public GameObject reloadIcon;
    public string test;
    public Transform shootOrigin;
    public GameObject forceShot;
    ObjectPooler objectPooler; 
    public Vector3 firstRot; 
    public float LastTimeShot;
    private static System.Timers.Timer aTimer;
    public Vector3 decalLocTran; 
    public enum ShootType { Auto, SemiAuto, Burst, Force };

    //Script Refs
    private PlayerGrab playerGrab;
    
    // Start is called before the first frame update
    void Start()
    {
        //finds the playergrab script
        playerGrab = GameObject.Find("Player").GetComponent<PlayerGrab>();

        //finds the tool rigidbody
        toolRB = gameObject.GetComponent<Rigidbody>();

        ammoLeftBank = gunOptions.maxAmmoInBank; 
        ammoLeftClip = gunOptions.maxAmmoInClip;

        //Finds the player ui script
        reloadIcon = GameObject.Find("PlayerUI").GetComponent<UiData>().reloadIcon.gameObject;

        shootOrigin = playerGrab.PlayerMovement.shootOrigin.transform; 

        //Deactivates the script 
        gameObject.GetComponent<Tool>().enabled = false; 

        objectPooler = ObjectPooler.Instance; 
    }

    // Update is called once per frame
    void Update()
    {
        //shooting semi-auto
        if (Input.GetMouseButtonDown(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootType != ShootType.Auto)
        {
            float x = Random.Range(-gunOptions.spread, gunOptions.spread);
            float y = Random.Range(-gunOptions.spread, gunOptions.spread); 
            Vector3 direction = playerGrab.CamPos.forward + new Vector3(x, y, 0); 
            Physics.Raycast(playerGrab.PlayerMovement.shootOrigin.transform.position, direction, out shotHit, gunOptions.bulletDistance);
            StartCoroutine(Shoot(shotHit));  
        }
        //shooting auto
        else if (Input.GetMouseButton(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootType == ShootType.Auto)
        {
            float x = Random.Range(-gunOptions.spread, gunOptions.spread);
            float y = Random.Range(-gunOptions.spread, gunOptions.spread); 
            Vector3 direction = playerGrab.CamPos.forward + new Vector3(x, y, 0); 
            Physics.Raycast(playerGrab.PlayerMovement.shootOrigin.transform.position, direction, out shotHit, gunOptions.bulletDistance);
            StartCoroutine(Shoot(shotHit));
        }      
        //reloading 
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammoLeftClip != gunOptions.maxAmmoInClip && ammoLeftBank > 0 || Input.GetMouseButtonDown(0) && !isReloading && ammoLeftClip == 0 && ammoLeftBank > 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void FixedUpdate()
    {
        
    }
    public IEnumerator Shoot(RaycastHit shotHit)
    {
        isShooting = true;     
        GunEffects(); 

        //slide animation
        if (slideAnimator != null)
        {
            slideAnimator.SetTrigger("SlideBack");
        }
         
        //recoil
        toolRB.AddForceAtPosition(gunOptions.recoilAmount * (-playerGrab.CamPos.forward), shootPoint.transform.position, ForceMode.Impulse);
        //muzzleflash particle system
        
        //shoot sound effect
        shootAudioSource.PlayOneShot(shootSFX[Random.Range(0, shootSFX.Length - 1)]);

        //reducing ammo amount
        ammoLeftClip--;
        canShoot = false; 

        //time before you can shoot again    
        yield return new WaitForSeconds(gunOptions.fireRate);  
        isShooting = false;             
    }

    public void ShootInput()
    {
       
     
    }

    //public IEnumerator ForceGun()
    //{
        // canShoot = false;
        // forceShot.SetActive(true); 
        // isShooting = true; 
        // firstRot = playerGrab.CamPos.forward;
        // forceShot.transform.position = shootOrigin.position;
        // forceShot.transform.rotation = shootOrigin.rotation;
        // toolRB.AddForceAtPosition(gunOptions.recoilAmount * (shootPoint.transform.forward), shootPoint.transform.position, ForceMode.Impulse);
        // //muzzleFlash.Play();
        // shootAudioSource.PlayOneShot(shootSFX[Random.Range(0, shootSFX.Length - 1)]);
        // ammoLeftClip--;
        // for (int i = 0; i < forceTome * 60; i++)
        // {
        //     forceShot.transform.Translate(firstRot * forceSpeed * Time.deltaTime, Space.World);
        //     yield return null;
        // }
        // forceShot.SetActive(false);  
        // yield return new WaitForSecondsRealtime(Time.deltaTime / timeBetweenShots);
        // isShooting = false; 
    //}

    public IEnumerator Reload()
    {
        isReloading = true;
        reloadIcon.SetActive(true);
        shootAudioSource.PlayOneShot(reloadSFX[Random.Range(0, reloadSFX.Length - 1)]);
        yield return new WaitForSeconds(gunOptions.reloadTime * Time.deltaTime);
        if ((gunOptions.maxAmmoInClip - ammoLeftClip) > ammoLeftBank)
        {
            ammoLeftClip = ammoLeftClip + ammoLeftBank;
            ammoLeftBank = 0; 
        }
        else if ((gunOptions.maxAmmoInClip - ammoLeftClip) < ammoLeftBank)
        {
            ammoLeftBank = ammoLeftBank - (gunOptions.maxAmmoInClip - ammoLeftClip);
            ammoLeftClip = ammoLeftClip + (gunOptions.maxAmmoInClip - ammoLeftClip);
         
        }
        reloadIcon.SetActive(false);
        isReloading = false;         
    }   

    public void GunEffects()
    {
        //muzzle flash 
        if (muzzleFlash)
        {
            muzzleFlash.Play(); 
        }

        if (shotHit.collider != null && shotHit.collider.tag != "Tool")
        {
          //Shoot Decals
         GameObject decal = objectPooler.SpawnFromPool(gunOptions.shootDecalTag, shotHit.point, Quaternion.LookRotation(shotHit.normal)); 
         if (shotHit.collider.GetComponent<Rigidbody>() != null)
         {
             decal.transform.parent = shotHit.collider.transform;
             decalLocTran = decal.transform.position;
             shotHit.collider.attachedRigidbody.AddForceAtPosition(gunOptions.bulletAppliedForce * playerGrab.CamPos.forward, shotHit.point, ForceMode.Impulse);
         }
         if (decal.transform.position != decalLocTran)
         {
             decal.transform.parent = null; 
         }

         //Impact effects 
         GameObject impactEff = objectPooler.SpawnFromPool(gunOptions.impactEffectTag, shotHit.point, Quaternion.LookRotation(shotHit.normal));
         impactEff.GetComponent<ParticleSystem>().Play();
        }
        //Bullet Shells 
        objectPooler.SpawnFromPool(gunOptions.bulletShellTag, shellReleasePoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(-shootPoint.transform.right * gunOptions.shellForce, ForceMode.Impulse);

    }

}
