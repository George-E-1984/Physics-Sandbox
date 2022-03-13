using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 
using UnityEngine.InputSystem; 

public class ToolbarManager : MonoBehaviour
{
    [Header("Needed Script References")]
    public PlayerMovement playerMovement;
    public PlayerInteract playerGrab;
    public PlayerManager playerManager; 
    public RectTransform selected;
    // public GameObject toolbar;
    public RectTransform[] slots;
    public Image[] icons;
    public GameObject[] items;
    public GameObject grabHolder; 
    public int initiallySelected = 0;
    public bool canScroll = true;
    public int currentlySelected = 0;
    public int lastSelected; 
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
        //scrollWheelInt = (int)Input.mouseScrollDelta.y;

        //dropping tool
        if (Input.GetKeyDown(KeyCode.F) && items[currentlySelected] != null)
        {
            if (items[currentlySelected].gameObject.tag == "Tool")
            {
                currentToolScript.StopAllCoroutines();
                currentToolScript.isReloading = false; 
                currentToolScript.reloadIcon.SetActive(false);
                currentToolScript.isShooting = false;
                currentToolScript.HandleInput(false); 
                currentToolScript.StopAim();
                currentToolScript.enabled = false; 
                SceneMaster.instance.forceShot.SetActive(false);
            }
            setItemActive(currentlySelected, false);
            items[currentlySelected].gameObject.SetActive(true);
            icons[currentlySelected].sprite = null; 
            items[currentlySelected] = null;
            playerGrab.isGrabbing = false; 
        }
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        currentlySelected += ((int)playerManager.playerInputActions.Player.TaskbarScroll.ReadValue<Vector2>().y);
        currentlySelected = mod(currentlySelected, slots.Length);
        print(currentlySelected);
        if (currentlySelected != lastSelected)
        {
            print("scrolled lel");
            SetSelectedSlot(currentlySelected);
            lastSelected = currentlySelected;
        }
    }
    void SetSelectedSlot(int slot)
    {
        Vector2 slotTransform = slots[slot].position;
        selected.position = slotTransform;
        if (items[lastSelected] != null)
        {
            print("poop");
            setItemActive(lastSelected, false);
            if (items[lastSelected].gameObject.tag == "Tool")
            {
                currentToolScript.StopAllCoroutines();
                currentToolScript.isReloading = false; 
                currentToolScript.reloadIcon.SetActive(false); 
                currentToolScript.isShooting = false; 
                currentToolScript = null; 
                SceneMaster.instance.forceShot.SetActive(false);
            } 
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
               currentToolScript.HandleInput(true); 
               print("Nala"); 
            } 
            SetSelectedSlot(currentlySelected); 
            lastSelected = currentlySelected; 
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
               currentToolScript.HandleInput(true);
               playerGrab.isGrabbingTool = true;
               currentToolScript.enabled = true;
            } 
            else 
            {
                playerGrab.isGrabbing = true; 
            }
            
        }
        else if (state == false)
        {            
            playerGrab.ReleaseObject();
            if (items[lastSelected].gameObject.tag == "Tool")
            {
                currentToolScript = items[lastSelected].gameObject.GetComponent<Tool>(); 
                if (currentToolScript.isAiming)
                {
                    currentToolScript.StopAim(); 
                }
                currentToolScript.HandleInput(false); 
                playerGrab.isGrabbingTool = false;
            }
            else
            {
                playerGrab.isGrabbing = false; 
            }
        }
    }
   

    int mod(int k, int n) 
    { 
        return ((k %= n) < 0) ? k + n : k; 
    }

    
}