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
        // inventory
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
            // add items
        }
        Debug.Log("Updating UI");
    }
}
