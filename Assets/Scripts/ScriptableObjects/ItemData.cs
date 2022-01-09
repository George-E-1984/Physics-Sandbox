using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tool")]
public class ItemData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public GameObject prefab; 
}
