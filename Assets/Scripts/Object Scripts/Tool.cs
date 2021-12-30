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
    [Tooltip("Only assign value to if the gun is burst!")]
    public int burstAmount; 

    [Header("Options")] 
    public ShootType shootTypes; 

    [Header("Assign")]
    public GameObject shootPoint;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject shootDecal;
    public AudioSource shootAudioSource;
    public AudioSource reloadAudioSource;
 

    [Header("Info (do not touch these values)")]
    public int ammoLeftBank;
    public int ammoLeftClip; 
    public bool isReloading;
    public bool isShooting; 
    private RaycastHit shotHit;
    private Rigidbody toolRB;
    public GameObject reloadIcon;
    public string test; 
    public enum ShootType { Auto, SemiAuto, Burst };

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

        
    }

    // Update is called once per frame
    void Update()
    {
        //shooting semi-auto
        if (Input.GetMouseButtonDown(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootTypes == ShootType.SemiAuto)
        {
            Physics.Raycast(playerGrab.PlayerMovement.shootOrigin.transform.position, playerGrab.CamPos.forward, out shotHit, bulletDistance);
            StartCoroutine(Shoot(shotHit)); 
        }
        //shooting auto
        else if (Input.GetMouseButton(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootTypes == ShootType.Auto)
        {
            Physics.Raycast(playerGrab.PlayerMovement.shootOrigin.transform.position, playerGrab.CamPos.forward, out shotHit, bulletDistance);
            StartCoroutine(Shoot(shotHit));
        }
        //shooting burst
        else if (Input.GetMouseButtonDown(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootTypes == ShootType.Burst)
        {
            for (int i = 0; i < burstAmount && !isShooting; i++)
            {
                
                Physics.Raycast(playerGrab.PlayerMovement.shootOrigin.transform.position, playerGrab.CamPos.forward, out shotHit, bulletDistance);
                StartCoroutine(Shoot(shotHit));

            }
        }

        //reloading 
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammoLeftClip != totalAmmoClip && ammoLeftBank > 0)
        {
            StartCoroutine(Reload());
        }
        
        

    }

    public IEnumerator Shoot(RaycastHit shotHit)
    {
        isShooting = true;             
    
        
        if (shotHit.collider != null && shotHit.collider.tag != "Tool")
        {
            GameObject decalGO = Instantiate(shootDecal, shotHit.point, Quaternion.LookRotation(shotHit.normal));
            GameObject impactGO = Instantiate(impactEffect, shotHit.point, Quaternion.LookRotation(shotHit.normal));
            if (shotHit.collider.GetComponent<Rigidbody>() != null)
            {
                decalGO.gameObject.transform.parent = shotHit.collider.transform;
                shotHit.collider.attachedRigidbody.AddForceAtPosition(shootForce * playerGrab.CamPos.forward, shotHit.point, ForceMode.Impulse);
            }
            Destroy(impactGO, 2f);
            Destroy(decalGO, 5f);
        }
        toolRB.AddForceAtPosition(totalRecoil * (shootPoint.transform.forward), shootPoint.transform.position, ForceMode.Impulse);
        muzzleFlash.Play();
        shootAudioSource.Play();
        ammoLeftClip--;
        yield return new WaitForSeconds(timeBetweenShots);
        isShooting = false;
        print("Shot");                      
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        reloadIcon.SetActive(true);
        reloadAudioSource.Play();
        yield return new WaitForSeconds(reloadTime);
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
