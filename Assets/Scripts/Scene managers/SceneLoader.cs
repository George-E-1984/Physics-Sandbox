using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoadName; 
    void Update()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadName);
        if(asyncLoad.isDone == true)
        {
            print("Nala");
        }
    }
}
