using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static Sprites;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is for single hand slot
/// </para>
/// Updated: 1/3/2020<para/>
/// Author: Roland Jiang, Yan Xiao<para/>
/// Attached object: handSlotUI<para/>
/// Updates: 3/17/2020: moved some functionality to other scripts<para/>
/// </summary>
public class HandSlot : MonoBehaviour
{
    public static HandSlot instance; // Reference to itself
    public GameObject player; // Reference to player for dropping

    public TMPro.TextMeshProUGUI displayName;
    public Image icon; // icon of item
    public GameObject removeButton; // button for deleting

    private GameObject item; // item in hand slot

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("More than one instance of handslot");
            return;
        }
        instance = this; // instantiates itself
    }

    private void Start()
    {
        displayName.SetText("");
    }

    /// <summary>
    /// This is called when the x is pressed, will drop item on the hand
    /// </summary>
    public void delete()
    {
        if (identify(item).isGroup("pickUpAble"))
        {
            player.GetComponent<pickUpObject>().DropItem(item);
        } else if (identify(item).isGroup("cutted"))
        {
            player.GetComponent<holdCuttedObject>().deleteCuttedObject(item);
        }
    }

    /// <summary>
    /// This will be called when UI provides information to the Handslot
    /// </summary>
    /// <param name="newItem">The target item</param>
    public void Set(GameObject newItem)
    {

        // Debug.Log("Set hand slot");
        item = newItem;
        if (identify(item).isGroup("pickUpAble"))
        {
            displayName.text = newItem.name;
            icon.sprite = Sprites.getSprite(1);
        } else
        {
            displayName.text = newItem.name;
            icon.sprite = Sprites.getSprite(0);
        }
        // Assign to a sprite
        icon.enabled = true;
        removeButton.SetActive(true);

    }

    /// <summary>
    /// This will clear all the information on the screen, will only be called when new information
    /// about the hand is avaliable. 
    /// </summary>
    public void clear()
    {
        // Clear all
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        displayName.text = "";
        removeButton.SetActive(false);
    }


    public GameObject getItem()
    {
        return item;
    }

    public bool isEmpty()
    {
        return (item == null);
    }

    /// <summary>
    /// This will be called when handslot is pressed
    /// it just put the item into inventory
    /// </summary>
    public void PutInInventory()
    {
        if (item == null)
            return;
        if (identify(item).isGroup("pickUpAble"))
        {
            player.GetComponent<pickUpObject>().PutInInventory();
        }
        else if (identify(item).isGroup("cutted"))
        {
            player.GetComponent<holdCuttedObject>().PutInInventory();
        }
    }
}
