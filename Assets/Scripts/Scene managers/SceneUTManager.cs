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
    public TextMeshProUGUI healthAmountTextMesh; 
    public int targetFrameRate = 300; 
    
    

    [Header("Script References")]
    public PlayerMovement playerMovement;
    public ToolbarManager toolbarManager;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateFPS()); 
         
    }

    // Update is called once per frame
    void Update()
    {
        GravityToggle();

        //Ammo counter
        if (toolbarManager.items[toolbarManager.currentlySelected] != null && toolbarManager.items[toolbarManager.currentlySelected].gameObject.tag == "Tool")
        {
            ammoLeftTextmesh.text = toolbarManager.items[toolbarManager.currentlySelected].gameObject.GetComponent<Weapons>().ammoLeftClip.ToString();
            ammoBankLeftTextmesh.text = toolbarManager.items[toolbarManager.currentlySelected].gameObject.GetComponent<Weapons>().ammoLeftBank.ToString();
        }          
        else
        {
            ammoLeftTextmesh.text = 00f.ToString();
            ammoBankLeftTextmesh.text = 00f.ToString();
        }

        healthAmountTextMesh.text = playerMovement.playerData.currentPlayerHealth.ToString(); 

        Application.targetFrameRate = targetFrameRate;
        Current = (int)(1f / Time.unscaledDeltaTime);
    }
   

    private void FixedUpdate()
    {

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
    private IEnumerator UpdateFPS()
    {
        yield return new WaitForSeconds(0.1f); 
        fpsTextmesh.text = Current.ToString();
        StartCoroutine(UpdateFPS()); 
    }
}
