using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    GameObject item;

    public void add(GameObject newItem)
    {
        item = newItem;
    }

    public void clear()
    {
        item = null;
    }
}
