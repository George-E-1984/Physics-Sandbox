using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoadName; 
    public SceneDealer sceneDealer; 

    void Start()
    {
        sceneToLoadName = sceneDealer.sceneToTransitionToName; 
        if (sceneToLoadName.Length == 0)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu");
        }
        else 
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadName);
        }
    }
    void Update()
    {
    
    }
}
