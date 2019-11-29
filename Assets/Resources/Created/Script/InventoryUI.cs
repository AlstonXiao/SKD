using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public Transform itemsParent;
    public KeyCode toggleInventory;

    inventory inv;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inv = inventory.instance;
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
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
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
