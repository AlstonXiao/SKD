using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    GameObject item;

    public Image icon;
    public GameObject removeButton;

    public void delete()
    {
        inventory.instance.delete(item);
    }

    public void add(GameObject newItem)
    {
        Debug.Log("Added");
        item = newItem;

        icon.sprite = null;
        // Assign to a sprite
        icon.enabled = true;
        removeButton.SetActive(true);
        //removeButton.interactable = true;
    }

    public void clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.SetActive(false);
    }

    public void selectItem()
    {
        inventory.instance.GetComponent<holdCuttedObject>().takeOut(inventory.instance.inventoryList.IndexOf(item));
        //inventory.instance.takeOut(inventory.instance.inventoryList.IndexOf(item));
    }
}
