using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;

    inventory inv;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inv = inventory.instance;
        inv.onItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("Updating UI");
    }
}
