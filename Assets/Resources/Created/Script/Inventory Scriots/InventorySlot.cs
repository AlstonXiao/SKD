using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is for individual inventory slots used by other classes
/// </para>
/// Updated: 1/3/2020<para/>
/// Author: Roland Jiang<para/>
/// Attached object: inventory<para/>
/// Updates: <para/>
/// </summary>
public class InventorySlot : MonoBehaviour
{
    GameObject item; // item in this slot

    public Image icon; // icon of this item (not used yet)
    public GameObject removeButton; // remove button for deleting
    HandSlot handSlot; // Reference to slot
    public holdCuttedObject holdCuttedObject; // Reference to holdCuttedObject

    private void Start()
    {
        // Instantiates
        handSlot = HandSlot.instance;
        holdCuttedObject = holdCuttedObject.instance;
    }

    public void delete()
    {
        // deletes this item from inventory
        inventory.instance.delete(item);
    }

    public void add(GameObject newItem)
    {
        item = newItem; // set item

        icon.sprite = null;
        // Assign to a sprite
        icon.enabled = true;
        removeButton.SetActive(true);
        //removeButton.interactable = true;
    }

    public void clear()
    {
        // clears item, sprite, remove button, etc
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.SetActive(false);
    }

    public void selectItem()
    {
        if (!handSlot.isEmpty()) // if item is in hand already
        {
            Debug.Log("Swapping objects");
            //inventory.instance.putIn(handSlot.getItem());
            holdCuttedObject.GetComponent<inventory>().putIn(handSlot.getItem());
            while (holdCuttedObject.status_script.Hands_free(Hands.cutted) != true) ; //TODO by Yan: You need to directly access to the status script but not through the holdcuttedobject
            holdCuttedObject.holdingObject = null;
            holdCuttedObject.fartherOrCloserFactor = 1;
            inventory.instance.GetComponent<holdCuttedObject>().takeOut(inventory.instance.inventoryList.IndexOf(item)); // take out this item from inventory
            // inventory.instance.takeOut(inventory.instance.inventoryList.IndexOf(item));
        } else
        {
            inventory.instance.GetComponent<holdCuttedObject>().takeOut(inventory.instance.inventoryList.IndexOf(item)); // take out this item from inventory
        }
    }
}
