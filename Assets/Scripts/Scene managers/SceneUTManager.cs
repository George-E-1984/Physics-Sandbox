using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class SceneUTManager : MonoBehaviour
{
    bool GravityNala = true;
    bool SlowNala = true;
    [Header("Assignables")]
    public PlayerMovement PlayerMovement;
    public TextMeshProUGUI fpsTextmesh;
    private int Current;
    public TextMeshProUGUI ammoLeftTextmesh;
    public TextMeshProUGUI ammoBankLeftTextmesh;
    [Range(0, 300)]
    public int targetFrameRate = 300; 
    

    [Header("Script References")]
    public PlayerGrab playerGrab;
    public PlayerMovement playerMovement;
    public ToolbarManager toolbarManager;


    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        GravityToggle();
        if (Input.GetKey(KeyCode.P))
        {
            PlayerMovement.allowedMovement = false;
        }
        else
        {
            PlayerMovement.allowedMovement = true;
        }
        //Fps counter
        Current = (int)(1f / Time.unscaledDeltaTime);
        fpsTextmesh.text = Current.ToString();

        //Ammo counter
        if (toolbarManager.Tools[toolbarManager.currentlySelected] != null)
        {
            ammoLeftTextmesh.text = toolbarManager.Tools[toolbarManager.currentlySelected].gameObject.GetComponent<Tool>().ammoLeftClip.ToString();
            ammoBankLeftTextmesh.text = toolbarManager.Tools[toolbarManager.currentlySelected].gameObject.GetComponent<Tool>().ammoLeftBank.ToString();
        }          
        else
        {
            ammoLeftTextmesh.text = 00f.ToString();
            ammoBankLeftTextmesh.text = 00f.ToString();
        }

        Application.targetFrameRate = targetFrameRate;
    }
   

    private void FixedUpdate()
    {
        fpsTextmesh.text = Current.ToString();
    }
    private void GravityToggle()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GravityNala = !GravityNala;
            Physics.gravity = GravityNala ? new Vector3(0f, 1f, 0f) : new Vector3(0f, -9.81f, 0f);
            print(Physics.gravity);
        }
    }
}
