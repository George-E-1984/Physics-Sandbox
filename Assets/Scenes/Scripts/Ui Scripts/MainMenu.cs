using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour

{ 
    public SceneDealer sceneDealer;  
    public OptionsSettings optionsSettings; 
    public void PlayGame(string sceneToLoad)
    {
        optionsSettings.SavePrefs(); 
        sceneDealer.sceneToTransitionToName = sceneToLoad; 
        SceneManager.LoadScene(sceneDealer.loadingScreenScene); 
    }
    public void QuitGame()
    {
        optionsSettings.SavePrefs(); 
        Application.Quit();
    }
  
}
