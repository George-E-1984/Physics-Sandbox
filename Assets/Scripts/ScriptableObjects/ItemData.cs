using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tool")]
public class ItemData : ScriptableObject
{
    public string gunName;
    [Header("Assignables")]
    public Sprite icon;
    public GameObject gunPrefab; 
    public string bulletShellTag;
   
    [Header("Gun Variables")]
    public bool canAim = true; 
    public float bulletDistance;
    public float bulletAppliedForce; 
    public float shellForce = 10f;
    public float spread = 0.15f;
    public float aimSpread;
    public float fireRate; 
    public float recoilAmount; 
    public float shootPointOffset = 1f; 
    public int aimFov = 35; 
    public float timeToAim = 1f; 
    
    [Header("Audio Range")]
    public float maxPitch = 1; 
    public float minPitch = 1; 

    [Header("Ammo Variables")]
    public int maxAmmoInClip; 
    public int maxAmmoInBank; 
    public float reloadTime;
    
    [Header("Burst Options")]
    public int burstAmount;
    public float timeBetweenBursts; 
    
}
