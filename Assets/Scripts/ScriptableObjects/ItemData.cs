using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tool")]
public class ItemData : ScriptableObject
{
    [Header("Assignables")]
    public Sprite icon;
    public GameObject gunPrefab; 
    public string bulletShellTag;
    public string impactEffectTag;
    public string shootDecalTag;

    [Header("The time it takes for cleanup")]
    public float timeForBulletShells = 10f;
    public float timeForShootDecals = 10f;
    public float timeForImpactEffect = 3f;

    public string gunName; 

    [Header("Bullet Variables")]
    public float bulletDistance;
    public float bulletAppliedForce; 
    public float shellForce = 10f;
    public float spread = 0.15f;
    public float fireRate; 
    public float recoilAmount; 

    [Header("Ammo Variables")]
    public int maxAmmoInClip; 
    public int maxAmmoInBank; 
    public float reloadTime;
    
    [Header("Burst Options")]
    public int burstAmount;
    public float timeBetweenBursts; 

    //Info
    public enum ShootType {Semi, Auto, Force, Burst}
    
}
