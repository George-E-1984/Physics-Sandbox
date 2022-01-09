using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tool")]
public class ItemData : ScriptableObject
{
    [Header("Assignables")]
    public Sprite icon;
    public GameObject prefab; 
    public string gunName; 

    [Header("Variables")]
    public float fireRate; 
    public int maxAmmoInClip; 
    public int maxAmmoInBank; 
    public enum ShootType {Semi, Auto, Force, Burst}
    public ShootType shootType; 
}
