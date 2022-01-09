using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scene Dealer")]
public class SceneDealer : ScriptableObject
{
    public string currentSceneName; 
    public string sceneToTransitionToName;

}
