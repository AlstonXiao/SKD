using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public Transform itemsParent;
    public KeyCode toggleInventory;
    public Camera Camera;
    public HandSlot handSlot;
    public Canvas canvas;

    inventory inv;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inv = inventory.instance;
        handSlot = HandSlot.instance;
        inv.onItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        //Debug.Log(slots.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleInventory))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            UpdateUI();

            if (inventoryUI.activeSelf)
            {
                Camera.cullingMask = 1 << LayerMask.NameToLayer("UI");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // Make a deep copy here
                GameObject inventoryCopy = Instantiate(handSlot.getItem());

                // Position is relative to parent (canvas)
                inventoryCopy.transform.SetParent(canvas.transform);
                inventoryCopy.transform.localPosition = new Vector3(0, 0, -100);
                inventoryCopy.layer = LayerMask.NameToLayer("UI");
            } else
            {
                Camera.cullingMask = -1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                // Remove the duplicate copy

            }
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
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
