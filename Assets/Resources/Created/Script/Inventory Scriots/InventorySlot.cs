using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Sprites;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is for individual inventory slots used by other classes
/// </para>
/// Updated: 1/3/2020<para/>
/// Author: Roland Jiang, Yan Xiao<para/>
/// Attached object: inventory<para/>
/// Updates: <para/>
/// </summary>
public class InventorySlot : MonoBehaviour
{
    private GameObject item; // item in this slot
    private InventoryUI inventoryUI; // Reference to inventoryUI

    public Image icon; // icon of this item (not used yet)
    public GameObject removeButton; // remove button for deleting
    public TMPro.TextMeshProUGUI displayName;

    public GameObject player; 

    private void Start()
    {
        // Instantiates
        inventoryUI = InventoryUI.instance;
        displayName.SetText("");
    }

    /// <summary>
    /// This is called when the x is pressed, will drop item 
    /// </summary>
    public void delete()
    {
        if (identify(item).isGroup("pickUpAble"))
        {
            player.GetComponent<pickUpObject>().DropItem(item);
        }
        else if (identify(item).isGroup("cutted"))
        {
            player.GetComponent<holdCuttedObject>().deleteCuttedObject(item);
        }
        // this will updata UI automatically
        inventory.instance.delete(item);
    
    }

    /// <summary>
    /// This will be called when UI provides information to the InventorySlot
    /// </summary>
    /// <param name="newItem">The target item</param>
    public void Set(GameObject newItem)
    {
        item = newItem; // set item
        if (identify(item).isGroup("pickUpAble"))
        {
            displayName.text = newItem.name;
            icon.sprite = Sprites.getSprite(1);
        }
        else
        {
            displayName.text = newItem.name;
            icon.sprite = Sprites.getSprite(0);
        }
        // Assign to a sprite
        icon.enabled = true;
        removeButton.SetActive(true);
        //removeButton.interactable = true;
    }

    /// <summary>
    /// This will empty the slot
    /// </summary>
    public void clear()
    {
        // clears item, sprite, remove button, etc
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        displayName.text = "";
        removeButton.SetActive(false);
    }

    /// <summary>
    /// This will be called by the button press. Will swap item with handslot if possible
    /// </summary>
    public void selectItem()
    {
        if (item != null)
        {
            inventoryUI.swapFromInventorySlot(item);
        }
    }
}
