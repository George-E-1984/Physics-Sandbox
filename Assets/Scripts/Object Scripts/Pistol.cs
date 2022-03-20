using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [Header("Assignales")]
    public Transform shootPoint;
    public Transform playerCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject gunshotDecal;
    public GameObject bulletShell;
    public GameObject shellEjectPoint;
    public GameObject reloadIcon;
    public Animator slideAnimator;
    private PlayerInteract playerGrabScript;

    [Header("Audio")]
    public AudioSource gunAudioSource;
    public AudioClip[] shootSFX;
    public AudioClip[] reloadSFX;

    [Header("Parameters")]
    public Sprite gunIcon;
    public int magazineSize;
    public int maxAmmoStorageSize;
    public float shootRange;
    public float recoil;
    public float timeBetweenShots;
    public float shootForce;
    public float reloadTime;

    int maxAmmoStorageLeft;
    int bulletsLeft;
    bool canShoot, isShooting, isReloading;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Tool>().enabled = false;

        playerGrabScript = GameObject.Find("Player").GetComponent<PlayerInteract>();

        maxAmmoStorageLeft = maxAmmoStorageSize;
        bulletsLeft = magazineSize;
        canShoot = true;

        //Finds the player ui script
        reloadIcon = GameObject.Find("PlayerUI").GetComponent<UiData>().reloadIcon.gameObject;

        shootPoint = playerGrabScript.playerMovement.shootOrigin.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot && !isReloading && bulletsLeft > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        canShoot = false;

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootRange))
        {

        }
    }
}
