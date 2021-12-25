using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers; 

public class Tool : MonoBehaviour
{
    public Sprite toolIcon;
    [Header("Gun Variables to set")]
    public bool isGun;
    public int totalAmmoClip;
    public int totalAmmoBank;
    public float totalRecoil;
    public int timeBetweenShots;
    public float shootForce;
    public GameObject shootPoint;
    public float bulletDistance;
    public ParticleSystem collisionEffect;
    public float reloadSpinSpeed;   
    [Header("Info")]
    public int ammoLeftBank;
    public int ammoLeftClip; 
    public bool isReloading;
    private RaycastHit shotHit;
    private Rigidbody toolRB;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isReloading && ammoLeftClip > 0)
        {
            Shoot(); 
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammoLeftClip != totalAmmoClip && ammoLeftBank > 0)
        {
            Reload();
        }
        else
        {
            isReloading = false; 
        }
    }

    public void Shoot()
    {           
        Physics.Raycast(playerGrab.CamPos.position, playerGrab.CamPos.forward, out shotHit, bulletDistance);    
        
        if (shotHit.collider != null && shotHit.collider.GetComponent<Rigidbody>() != null)
        {
            shotHit.collider.GetComponent<Rigidbody>().AddForce(shootForce * (playerGrab.CamPos.forward), ForceMode.Impulse);
        }
        toolRB.AddForce(totalRecoil * (shootPoint.transform.forward), ForceMode.Impulse);
        toolRB.AddForce((totalRecoil * (shootPoint.transform.up)) / 2, ForceMode.Impulse);
        ammoLeftClip--; 
        print("Shot");                      
    }

    public void Reload()
    {
        isReloading = true; 

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
    }

    private void Recoil()
    {

    }

}
