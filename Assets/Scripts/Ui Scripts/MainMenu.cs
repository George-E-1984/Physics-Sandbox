using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour

{
    public string loadingScreenSceneName;
    public string sceneToLoadName;  
    public void PlayGame()
    {
        SceneManager.LoadScene(loadingScreenSceneName);
    }
    public void QuitGame()
    {
        print("Quit");
        Application.Quit();
    }
  
}
