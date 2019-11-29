using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static publicMethods.PublicMethods;

public class InventorySlot : MonoBehaviour
{
    GameObject item;

    public Image icon;
    public GameObject removeButton;
    HandSlot handSlot;
    public holdCuttedObject holdCuttedObject;

    private void Start()
    {
        handSlot = HandSlot.instance;
        holdCuttedObject = holdCuttedObject.instance;
    }

    public void delete()
    {
        inventory.instance.delete(item);
    }

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

    public void selectItem()
    {
        if (!handSlot.isEmpty())
        {
            Debug.Log("Swapping objects");
            //inventory.instance.putIn(handSlot.getItem());
            holdCuttedObject.GetComponent<inventory>().putIn(handSlot.getItem());
            while (holdCuttedObject.status_script.Hands_free(Hands.cutted) != true) ;
            holdCuttedObject.holdingObject = null;
            holdCuttedObject.fartherOrCloserFactor = 1;
            inventory.instance.GetComponent<holdCuttedObject>().takeOut(inventory.instance.inventoryList.IndexOf(item));
            // inventory.instance.takeOut(inventory.instance.inventoryList.IndexOf(item));
        } else
        {
            inventory.instance.GetComponent<holdCuttedObject>().takeOut(inventory.instance.inventoryList.IndexOf(item));
        }
        //inventory.instance.takeOut(inventory.instance.inventoryList.IndexOf(item));
    }
}
