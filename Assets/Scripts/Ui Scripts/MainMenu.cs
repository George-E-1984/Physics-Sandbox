using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour

{ 
    public SceneDealer sceneDealer;  
    public void PlayGame(string sceneToLoad)
    {
        sceneDealer.sceneToTransitionToName = sceneToLoad; 
        SceneManager.LoadScene(sceneDealer.loadingScreenScene); 
    }
    public void QuitGame()
    {
        Application.Quit();
    }
  
}
