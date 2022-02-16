using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collider Effects")]
public class ColliderEffects : ScriptableObject
{
    public string decalPoolName; 
    public string shootEffectPoolName; 
    public AudioClip impactSFX; 
    public AudioClip gunHitSFX; 

}
