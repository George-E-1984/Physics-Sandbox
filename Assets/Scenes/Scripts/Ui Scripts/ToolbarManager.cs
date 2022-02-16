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
    public GameObject[] items;
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
        currentlySelected = initiallySelected;
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
        if (Input.GetKeyDown(KeyCode.F) && items[currentlySelected] != null)
        {
            if(items[currentlySelected].gameObject.tag == "Tool")
            {
                currentToolScript.StopAllCoroutines();
                currentToolScript.isReloading = false; 
                currentToolScript.reloadIcon.SetActive(false);
                currentToolScript.isShooting = false;
                items[currentlySelected].gameObject.GetComponent<Tool>().enabled = false;
                SceneMaster.instance.forceShot.SetActive(false);
            }
            
            setItemActive(currentlySelected, false);
            items[currentlySelected].gameObject.SetActive(true);
            icons[currentlySelected].sprite = null; 
            items[currentlySelected] = null;
        }
    }

    void SetSelectedSlot(int slot)
    {
        Vector2 slotTransform = slots[slot].position;
        selected.position = slotTransform;
        if (items[lastSelected] != null)
        {
            if (items[lastSelected].gameObject.tag == "Tool")
            {
                currentToolScript.StopAllCoroutines();
                currentToolScript.isReloading = false; 
                currentToolScript.reloadIcon.SetActive(false); 
                currentToolScript.isShooting = false; 
                currentToolScript = null; 
                SceneMaster.instance.forceShot.SetActive(false);
            } 
            setItemActive(lastSelected, false);
        }
        
       if (items[currentlySelected] != null)
        {
            setItemActive(currentlySelected, true);
        }
    }

    public void AddItem(GameObject item)
    {
        var firstNull = Array.FindIndex(items, I => I == null);
        if (firstNull != -1)
        {
            items[firstNull] = item;
            currentlySelected = firstNull;
            icons[currentlySelected].sprite = items[currentlySelected].GetComponent<ObjectProperties>().icon;
            if (item.tag == "Tool")
            {
               currentToolScript = items[currentlySelected].GetComponent<Tool>();
               print("Nala"); 
            } 
        }
        else
        {
            print("Invetory is full m8"); 
        }
    }

    public void setItemActive(int item, bool state)
    {
        items[item].gameObject.SetActive(state);
        if (state)
        {
            items[currentlySelected].transform.position = (grabHolder.transform.position - playerGrab.grabHolderConfig.anchor);
            items[currentlySelected].transform.rotation = grabHolder.transform.rotation;
            playerGrab.GrabObject(items[currentlySelected].gameObject);
            if (items[currentlySelected].gameObject.tag == "Tool")
            {
               currentToolScript = items[currentlySelected].GetComponent<Tool>();
               playerGrab.isGrabbingTool = true;
               currentToolScript.enabled = true;
            } 
            
        }
        else if (state == false)
        {            
            playerGrab.ReleaseObject(false);
            if (items[lastSelected].gameObject.tag == "Tool")
            {
                playerGrab.isGrabbingTool = false;
            }
        }
    }
   

    int mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }

    
}