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
    public GameObject shootPoint;
    public float bulletDistance;
    public ParticleSystem collisionEffect;
    public float reloadTime;

    [Header("Options")] 
    public ShootType shootTypes; 
   
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
            Physics.Raycast(playerGrab.CamPos.position, playerGrab.CamPos.forward, out shotHit, bulletDistance);
            StartCoroutine(Shoot(shotHit)); 
        }
        else if (Input.GetMouseButton(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootTypes == ShootType.Auto)
        {
            Physics.Raycast(playerGrab.CamPos.position, playerGrab.CamPos.forward, out shotHit, bulletDistance);
            StartCoroutine(Shoot(shotHit));
        }
        else if (Input.GetMouseButtonDown(0) && !isReloading && ammoLeftClip > 0 && !isShooting && shootTypes == ShootType.Burst)
        {

        }

        //reloading 
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammoLeftClip != totalAmmoClip && ammoLeftBank > 0)
        {
            StartCoroutine(Reload());
        }
        
        

    }

    IEnumerator Shoot(RaycastHit shotHit)
    {
        isShooting = true;             
        if (shotHit.collider != null && shotHit.collider.GetComponent<Rigidbody>() != null && !isReloading)
        {
            shotHit.collider.attachedRigidbody.AddForceAtPosition(shootForce * playerGrab.CamPos.forward, shotHit.point, ForceMode.Impulse);
        }
        toolRB.AddForceAtPosition(totalRecoil * (shootPoint.transform.forward), shootPoint.transform.position, ForceMode.Impulse);
        ammoLeftClip--;
        yield return new WaitForSeconds(timeBetweenShots);
        isShooting = false;
        print("Shot");                      
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        reloadIcon.SetActive(true);
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
