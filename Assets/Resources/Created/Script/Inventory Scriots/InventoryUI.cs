using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is used to handle the User Interface of the inventory
/// </para>
/// Updated: 1/3/2020<para/>
/// Author: Roland Jiang, Yan Xiao<para/>
/// Attached object: Inventory Canvas<para/>
/// Updates: 3/17/2020: modify the data flow<para/>
/// </summary>
public class InventoryUI : MonoBehaviour
{
    public KeyCode              toggleInventory; // Making inventory appear and disappear
    [Space(10)]
    public GameObject           inventoryUI; // Reference to UI gameObject
    public Transform            itemsParent; 
    public Light                inventoryLight; 
    [Space(10)]
    public GameObject           player;
    public Camera               Camera;
    public GameObject           crosshair; // black square in middle of screen

    [Space(10)]
    [Header("Debug")]
    public GameObject           inventoryCopy; // Preview of item in hand
    public static InventoryUI   instance; // Reference to itself
    private player_status       player_situation;
    private HandSlot            handSlot; // Slot that is displayed in hand
    private inventory           inv; // Reference to actual inventory manager
    private InventorySlot[]     slots; // Array of slots
    private bool                UIActive; // cache the UI status and no need to ask the canvas

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory UI");
            return;
        }
        instance = this; 
    }

    void Start()
    {
        // Instantiate references
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inv = inventory.instance;
        // When inventory changes, add an update request to UI
        inv.onItemChangedCallback += UpdateUI;

        handSlot = HandSlot.instance;
        // Set slots to children of the parent class
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        player_situation = player.GetComponent<player_status>();

        UIActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleInventory)) 
        {
            // toggle off
            if (this.GetComponent<Canvas>().enabled)
            {
                // successfully free the screen
                if (player_situation.Screen_unloock(ScreenLockStatus.inventory))
                {
                    this.GetComponent<Canvas>().enabled = !this.GetComponent<Canvas>().enabled;
                    inventoryLight.enabled = !inventoryLight.enabled;
                    UpdateUI();
                    Camera.cullingMask = -1; // Show all layers
                    removePreview();

                    // Lock cursor so you can look around, also allow the player to rotate camera
                    player.GetComponent<FirstPersonController>().ToggleRotation(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    // Show cross hair
                    crosshair.SetActive(true);
                    
                    UIActive = false;
                }
                else
                {
                    Debug.LogError("cannot free the screem. Must have some error");
                }
            }
            // toggle on
            else
            {
                // successfully lock the screen
                if (player_situation.Screen_lock(ScreenLockStatus.inventory))
                {
                    // toggle the visibility of the canvas and the lights
                    this.GetComponent<Canvas>().enabled = !this.GetComponent<Canvas>().enabled;
                    inventoryLight.enabled = !inventoryLight.enabled;
                    UpdateUI();
                    Camera.cullingMask = 1 << LayerMask.NameToLayer("UI"); // Hide everything not with the UI Layer 
                    createPreview();

                    // Unlock cursor so you can move mouse, dont allow player to rotate camera
                    player.GetComponent<FirstPersonController>().ToggleRotation(false);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    UIActive = true;

                    crosshair.SetActive(false);

                }
                else
                {
                    Debug.LogError("cannot lock the screem. Must have some error");
                }
            }
        }

        if (this.GetComponent<Canvas>().enabled)
        {
            if (!handSlot.isEmpty())
            {
                // Constantly rotate object
                inventoryCopy.transform.Rotate(0, (float)0.1, 0);
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    // Rotate object by scrolling
                    inventoryCopy.transform.Rotate(0, (float)0.4, 0);
                }
            }
        }
    }

    /// <summary>
    /// This is a wrapper function to the handslot. 
    /// It will add the item to handslot and create preview if the UI is active
    /// </summary>
    /// <param name="item">Item that is on the hand</param>
    public void AddToHandSlot(GameObject item)
    {
        handSlot.Set(item);
        if (UIActive)
            createPreview();
    }

    /// <summary>
    /// This is a wrapper function to the handslot.
    /// It will delete the object at hand and remove preive
    /// </summary>
    public void RemoveFromHandSlot()
    {
        handSlot.clear();
        removePreview();
    }

    /// <summary>
    /// This will create preview on the screen, only be called when the UI is active
    /// </summary>
    private void createPreview()
    {
        if (handSlot.getItem() == null)
            return;

        // Make a deep copy of what's in hand slot for displaying
        inventoryCopy = Instantiate(handSlot.getItem());
        inventoryCopy.SetActive(true); // Make it appear

        // Position is relative to parent (canvas)
        inventoryCopy.transform.SetParent(this.transform);
        
        // Apply scale to different type of objects
        if (identify(inventoryCopy).isGroup("pickUpAble"))
        {
            inventoryCopy.transform.localPosition = new Vector3(100, -100, -100);
            inventoryCopy.transform.localScale *= ((float)0.0007);
        }
        else
        {
            inventoryCopy.transform.localPosition = new Vector3(0, 0, -100);
            inventoryCopy.transform.localScale *= ((float)0.2);
        }
        Quaternion rot = inventoryCopy.transform.parent.rotation; // Get parent rotation
        inventoryCopy.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z); // Set a fixed rotation
        inventoryCopy.layer = LayerMask.NameToLayer("UI");

    }

    /// <summary>
    /// This will remove preview on the screen, it will be called when the UI is closed or the handslot is free
    /// </summary>
    public void removePreview()
    {
        if (inventoryCopy != null) {
            // Remove the duplicate copy, Make it disappear instantly. Faster than destroying
            inventoryCopy.SetActive(false); 
            Destroy(inventoryCopy, 1.0f);
        }
    }

    /// <summary>
    /// If the inventory data is change, this should be called
    /// </summary>
    void UpdateUI() // Called when change is detected
    {
        for (int i = 0; i < slots.Length; i++) 
        {
            if (i < inv.inventoryList.Count)
            {
                slots[i].Set(inv.inventoryList[i]);
            } else
            {
                slots[i].clear();
            }
        }
        //Debug.Log("Updating UI");
    }

    /// <summary>
    /// This is a wrapper function if a inventory slot is click. 
    /// It will first put the items on hand to inventory
    /// Then it will take the item in the inventory out
    /// </summary>
    /// <param name="item">The item that is been clicked</param>
    public void swapFromInventorySlot(GameObject item)
    {
        // first check if the player is ready to hold the item
        if (!player.GetComponent<player_status>().Hands_swap_able())
        {
            return;
        }

        // if item is in hand already
        if (!handSlot.isEmpty()) 
        {
            if (identify(handSlot.getItem()).isGroup("pickUpAble"))
            {
                player.GetComponent<pickUpObject>().PutInInventory();
            }
            else if(identify(handSlot.getItem()).isGroup("cutted"))
            {
                player.GetComponent<holdCuttedObject>().PutInInventory();
            }
            else
            {
                Debug.LogError("cannot identify the object on hand");
            }
            
        }

        // take them out
        player.GetComponent<inventory>().delete(item);
        item.SetActive(true);
        if (identify(item).isGroup("pickUpAble"))
        {
            player.GetComponent<pickUpObject>().takeOut(item);
        }
        else if (identify(item).isGroup("cutted"))
        {
            player.GetComponent<holdCuttedObject>().putCuttedItemToHand(item);
        }
        else
        {
            Debug.LogError("cannot identify the object on hand");
        }

    }
}
