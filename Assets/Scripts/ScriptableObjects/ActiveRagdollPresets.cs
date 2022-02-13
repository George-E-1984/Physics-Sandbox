using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActiveRagdollPresets")]
public class ActiveRagdollPresets : ScriptableObject
{
    public float falloverDistance;  
    public float attackDistance; 
    public int attackDamage;
    public int maxRagdollHealth; 

}
