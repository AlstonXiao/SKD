using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// <para>
/// This class is used to handle the User Interface of the inventory
/// </para>
/// Updated: 1/3/2020<para/>
/// Author: Roland Jiang<para/>
/// Attached object: player<para/>
/// Updates: <para/>
/// </summary>
public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI; // Reference to UI gameObject
    public static InventoryUI instance; // Reference to itself
    public Transform itemsParent; // 
    public KeyCode toggleInventory; // Making inventory appear and disappear
    public Camera Camera; // Reference to Camera
    public HandSlot handSlot; // Slot that is displayed in hand
    public Canvas canvas; // Reference to canvas
    public Light inventoryLight; // Reference to inventory light

    public GameObject inventoryCopy; // Preview of item in hand

    inventory inv; // Reference to actual inventory manager

    InventorySlot[] slots; // Array of slots

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of handslot");
            return;
        }
        instance = this; // instantiates itself
    }

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate references
        inv = inventory.instance;
        handSlot = HandSlot.instance;
        inv.onItemChangedCallback += UpdateUI; // When inventory changes, add an update request to UI
        slots = itemsParent.GetComponentsInChildren<InventorySlot>(); // Set slots to children of the parent class
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleInventory)) // If pressed toggleInventory key
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf); // Toggle active or not
            inventoryLight.enabled = !inventoryLight.enabled;
            UpdateUI();

            if (inventoryUI.activeSelf)
            {
                Camera.cullingMask = 1 << LayerMask.NameToLayer("UI"); // Hide everything not with the UI Layer
                // Unlock cursor so you can move mouse
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                createPreview();
            } else
            {
                Camera.cullingMask = -1; // Show all layers
                // Lock cursor so you can look around
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                removePreview();
            }
        }

        if (inventoryUI.activeSelf)
        {
            if (!handSlot.isEmpty())
            {
                // Constantly rotate object
                inventoryCopy.transform.Rotate(1, 0, 0);
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    // Rotate object by scrolling
                    inventoryCopy.transform.Rotate(0, 4, 0);
                }
            }
        }
    }

    public void createPreview()
    {
        // Make a deep copy of what's in hand slot for displaying
        inventoryCopy = Instantiate(handSlot.getItem());
        inventoryCopy.SetActive(true); // Make it appear

        // Position is relative to parent (canvas)
        inventoryCopy.transform.SetParent(canvas.transform);
        inventoryCopy.transform.localPosition = new Vector3(0, 0, -100);
        Quaternion rot = inventoryCopy.transform.parent.rotation; // Get parent rotation
        inventoryCopy.transform.localRotation = Quaternion.Euler(rot.x - 45, rot.y - 45, rot.z - 45); // Set a fixed rotation
        inventoryCopy.layer = LayerMask.NameToLayer("UI");
    }

    public void removePreview()
    {
        if (inventoryCopy != null) {
            // Remove the duplicate copy
            inventoryCopy.SetActive(false); // Make it disappear instantly. Faster than destroying
            Destroy(inventoryCopy, 1.0f);
        }
    }

    void UpdateUI() // Called when change is detected
    {
        for (int i = 0; i < slots.Length; i++) // Loops through slots and adds item
        {
            if (i < inv.inventoryList.Count)
            {
                slots[i].add(inv.inventoryList[i]);
            } else
            {
                slots[i].clear();
            }
        }
        //Debug.Log("Updating UI");
    }
}
