using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using UnityEngine.UI; 

public class Tool : MonoBehaviour
{
    public Sprite toolIcon;
    [Header("Gun Variables to set")]
    public int totalAmmoClip;
    public int totalAmmoBank;
    public float totalRecoil;
    public float timeBetweenShots;
    public float shootForce;
    public float bulletDistance;
    public float reloadTime;
    public float shellForce = 10f;
    public float spread = 0.15f; 
    [Header("The time it takes for cleanup")]
    public float timeForBulletShells = 10f;
    public float timeForShootDecals = 10f;
    public float timeForImpactEffect = 3f;  
    [Header("Force Gun Only!")]
    public float forceTome;
    public float forceSpeed; 
    [Tooltip("Only assign value to if the gun is burst!")]
    public int burstAmount; 

    [Header("Options")] 
    public ShootType shootTypes; 

    [Header("Assign")]
    public GameObject shootPoint;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject shootDecal;
    public Animator slideAnimator;
    public GameObject bulletShell;
    public GameObject shellReleasePoint; 
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
    public bool isSpecial;  
    private RaycastHit shotHit;
    private Rigidbody toolRB;
    public GameObject reloadIcon;
    public string test;
    public Transform shootOrigin;
    public GameObject forceShot;
    public Vector3 firstRot; 
    public float LastTimeShot;
    private static System.Timers.Timer aTimer;
    public enum ShootType { Auto, SemiAuto, Burst, Force };

    //Script Refs
    private PlayerGrab playerGrab;

    [Header("Special Tool Variables")]
    public bool isSpecialTool;



    // Start is called before the first frame update
    void Start()
    {
        //finds the playergrab script
        playerGrab = GameObject.Find("Player").GetComponent<PlayerGrab>();

        //finds the tool rigidbody
        toolRB = gameObject.GetComponent<Rigidbody>();

        ammoLeftBank = totalAmmoBank; 
        ammoLeftClip = totalAmmoClip;

        //Finds the player ui script
        reloadIcon = GameObject.Find("PlayerUI").GetComponent<UiData>().reloadIcon.gameObject;

        shootOrigin = playerGrab.PlayerMovement.shootOrigin.transform; 

        if (shootTypes == ShootType.Force)
        {
            isSpecial = true; 
        }

        //Deactivates the script 
        gameObject.GetComponent<Tool>().enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        //shooting semi-auto
        if (Input.GetMouseButtonDown(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootTypes != ShootType.Auto)
        {
            //float x = Random.Range(-spread, spread);
            //float y = Random.Range(-spread, spread); 
            //Vector3 direction = playerGrab.CamPos.forward + new Vector3(x, y, 0); 
            //Physics.Raycast(playerGrab.PlayerMovement.shootOrigin.transform.position, direction, out shotHit, bulletDistance);
            //StartCoroutine(Shoot(shotHit)); 
            canShoot = true; 
        }
        //shooting auto
        else if (Input.GetMouseButton(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootTypes == ShootType.Auto)
        {
            //float x = Random.Range(-spread, spread);
            //float y = Random.Range(-spread, spread); 
            //Vector3 direction = playerGrab.CamPos.forward + new Vector3(x, y, 0); 
            //Physics.Raycast(playerGrab.PlayerMovement.shootOrigin.transform.position, direction, out shotHit, bulletDistance);
            //StartCoroutine(Shoot(shotHit));
            canShoot = true; 
        }
        //shooting burst
        //else if (Input.GetMouseButtonDown(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootTypes == ShootType.Burst)
        //{
            //float x = Random.Range(-spread, spread);
           // float y = Random.Range(-spread, spread); 
           // Vector3 direction = playerGrab.CamPos.forward + new Vector3(x, y, 0); 
            //for (int i = 0; i < burstAmount && !isShooting; i++)
           // {
                
               // Physics.Raycast(playerGrab.PlayerMovement.shootOrigin.transform.position, playerGrab.CamPos.forward, out shotHit, bulletDistance);
               // StartCoroutine(Shoot(shotHit));

            //}
        //}
        //shooting thunder
        //else if (Input.GetMouseButtonDown(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootTypes == ShootType.Force)
        //{
            //StartCoroutine(ForceGun()); 
        //}

        //reloading 
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammoLeftClip != totalAmmoClip && ammoLeftBank > 0)
        {
            StartCoroutine(Reload());
        }

        //Method that handles the slide animations
        
        
        

    }

    private void FixedUpdate()
    {
        if (canShoot && shootTypes != ShootType.Force)
        {
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread); 
            Vector3 direction = playerGrab.CamPos.forward + new Vector3(x, y, 0); 
            Physics.Raycast(playerGrab.PlayerMovement.shootOrigin.transform.position, direction, out shotHit, bulletDistance);
            StartCoroutine(Shoot(shotHit));
            
        } 
        else if (canShoot && shootTypes == ShootType.Force)
        {
            StartCoroutine(ForceGun());
        }
        if (canReload)
        {
            StartCoroutine(Reload());
        }  
    }
    public IEnumerator Shoot(RaycastHit shotHit)
    {
        isShooting = true;     
        //bullet holes and impact effects being made if you hit something
        if (shotHit.collider != null && shotHit.collider.tag != "Tool")
        {
            GameObject decalGO = Instantiate(shootDecal, shotHit.point, Quaternion.LookRotation(shotHit.normal));
            GameObject impactGO = Instantiate(impactEffect, shotHit.point, Quaternion.LookRotation(shotHit.normal));
            if (shotHit.collider.GetComponent<Rigidbody>() != null)
            {
                decalGO.gameObject.transform.parent = shotHit.collider.transform;
                shotHit.collider.attachedRigidbody.AddForceAtPosition(shootForce * playerGrab.CamPos.forward, shotHit.point, ForceMode.Impulse);
            }
            Destroy(impactGO, timeForImpactEffect);
            Destroy(decalGO, timeForShootDecals);
        }
        //slide animation
        if (slideAnimator != null)
        {
            slideAnimator.SetTrigger("SlideBack");
        }
        //bullet shells
        GameObject bulletShellGo = Instantiate(bulletShell, shellReleasePoint.transform.position, bulletShell.transform.rotation);
        bulletShellGo.GetComponent<Rigidbody>().AddForce(-shootPoint.transform.right * shellForce, ForceMode.Impulse);
        Destroy(bulletShellGo, timeForBulletShells); 
        //recoil
        toolRB.AddForceAtPosition(totalRecoil * (-playerGrab.CamPos.forward), shootPoint.transform.position, ForceMode.Impulse);
        //muzzleflash particle system
        muzzleFlash.Play();
        //shoot sound effect
        shootAudioSource.PlayOneShot(shootSFX[Random.Range(0, shootSFX.Length - 1)]);
        //reducing ammo amount
        ammoLeftClip--;
        canShoot = false; 
        //time before you can shoot again    
        Time.timeScale = 1f; 
        yield return new WaitForSeconds(timeBetweenShots);  
        isShooting = false;             
    }

    public void ShootInput()
    {
       
     
    }

    public IEnumerator ForceGun()
    {
        canShoot = false;
        forceShot.SetActive(true); 
        isShooting = true; 
        firstRot = playerGrab.CamPos.forward;
        forceShot.transform.position = shootOrigin.position;
        forceShot.transform.rotation = shootOrigin.rotation;
        toolRB.AddForceAtPosition(totalRecoil * (shootPoint.transform.forward), shootPoint.transform.position, ForceMode.Impulse);
        muzzleFlash.Play();
        shootAudioSource.PlayOneShot(shootSFX[Random.Range(0, shootSFX.Length - 1)]);
        ammoLeftClip--;
        for (int i = 0; i < forceTome * 60; i++)
        {
            forceShot.transform.Translate(firstRot * forceSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        forceShot.SetActive(false);  
        yield return new WaitForSecondsRealtime(Time.deltaTime / timeBetweenShots);
        isShooting = false; 
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        reloadIcon.SetActive(true);
        shootAudioSource.PlayOneShot(reloadSFX[Random.Range(0, reloadSFX.Length - 1)]);
        yield return new WaitForSeconds(reloadTime * Time.deltaTime);
        if ((totalAmmoClip - ammoLeftClip) > ammoLeftBank)
        {
            ammoLeftClip = ammoLeftClip + ammoLeftBank;
            ammoLeftBank = 0; 
        }
        else if ((totalAmmoClip - ammoLeftClip) < ammoLeftBank)
        {
            ammoLeftBank = ammoLeftBank - (totalAmmoClip - ammoLeftClip);
            ammoLeftClip = ammoLeftClip + (totalAmmoClip - ammoLeftClip);
         
        }
        reloadIcon.SetActive(false);
        isReloading = false;         
    }   
}
