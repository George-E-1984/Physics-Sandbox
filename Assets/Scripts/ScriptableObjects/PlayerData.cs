using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public float sensitivity; 
    public float volume; 
    public float fieldOfView; 
    public GameObject playerPrefab;   
}



