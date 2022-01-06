using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 

public class ToolbarManager : MonoBehaviour
{
    [Header("Needed Script References")]
    public PlayerMovement playerMovement;
    public PlayerGrab playerGrab;
    public RectTransform selected;
    // public GameObject toolbar;
    public RectTransform[] slots;
    public Image[] icons;
    public GameObject[] Tools;
    public GameObject grabHolder; 
    public int initiallySelected;
    public bool canScroll = true;
    public int currentlySelected = 0;
    private int lastSelected; 
    private int scrollWheelInt = 0;

    [Header("Info")]
    public Tool currentToolScript; 

    void Start()
    {
        currentlySelected = initiallySelected - 1;
        SetSelectedSlot(currentlySelected);
        lastSelected = currentlySelected; 
    }

    void Update()
    {
        scrollWheelInt = (int)Input.mouseScrollDelta.y;
        currentlySelected += scrollWheelInt * System.Convert.ToUInt16(canScroll);
        currentlySelected = mod(currentlySelected, slots.Length);
        if (currentlySelected != lastSelected)
        {
            SetSelectedSlot(currentlySelected);
            lastSelected = currentlySelected;
        }
        //dropping tool
        if (Input.GetKeyDown(KeyCode.F) && Tools[currentlySelected] != null)
        {
            if (currentToolScript.isReloading)
            {
                currentToolScript.StopAllCoroutines();
                currentToolScript.isReloading = false;
                currentToolScript.reloadIcon.SetActive(false); 
            }
            currentToolScript.isShooting = false;
            setToolActive(currentlySelected, false);
            Tools[currentlySelected].gameObject.GetComponent<Tool>().enabled = false;
            Tools[currentlySelected].gameObject.SetActive(true);
            icons[currentlySelected].sprite = null; 
            Tools[currentlySelected] = null;
        }
    }

    void SetSelectedSlot(int slot)
    {
        Vector2 slotTransform = slots[slot].position;
        selected.position = slotTransform;
        if (Tools[lastSelected] != null)
        {
            if (currentToolScript.isReloading == true)
            {
                currentToolScript.StopAllCoroutines();
                currentToolScript.isReloading = false;
                currentToolScript.reloadIcon.SetActive(false);
            }
            currentToolScript.isShooting = false;
            currentToolScript = null; 
            setToolActive(lastSelected, false);
        }
        
       if (Tools[currentlySelected] != null)
        {
            setToolActive(currentlySelected, true);
        }
    }

    public void AddTool(GameObject tool)
    {
        var firstNull = Array.FindIndex(Tools, I => I == null);
        if (firstNull != -1)
        {
            Tools[firstNull] = tool;
            currentlySelected = firstNull;
            currentToolScript = Tools[currentlySelected].GetComponent<Tool>();
            icons[currentlySelected].sprite = currentToolScript.toolIcon; 
        }
        else
        {
            print("Invetory is full m8"); 
        }
    }

    public void setToolActive(int tool, bool state)
    {
        Tools[tool].gameObject.SetActive(state);
        if (state)
        {
            Tools[currentlySelected].transform.position = (grabHolder.transform.position - playerGrab.grabHolderConfig.anchor);
            print(Tools[currentlySelected].transform.position);
            Tools[currentlySelected].transform.rotation = grabHolder.transform.rotation;
            playerGrab.GrabObject(Tools[currentlySelected].gameObject);
            playerGrab.isGrabbingTool = true;
            currentToolScript = Tools[currentlySelected].GetComponent<Tool>();
            currentToolScript.enabled = true; 
        }
        else if (state == false)
        {            
            playerGrab.ReleaseObject(false);
            playerGrab.isGrabbingTool = false;
        }
    }
   

    int mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }

    
}