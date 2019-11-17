using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    GameObject item;

    public Image icon;
    public GameObject removeButton;

    public void add(GameObject newItem)
    {
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

    public void onRemoveButton()
    {

    }
}
