using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is for single hand slot
/// </para>
/// Updated: 1/3/2020<para/>
/// Author: Roland Jiang<para/>
/// Attached object: player<para/>
/// Updates: <para/>
/// </summary>
public class HandSlot : MonoBehaviour
{
    public static HandSlot instance; // Reference to itself
    public GameObject player; // Reference to player for dropping
    InventoryUI inventoryUI;

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
        inventoryUI = InventoryUI.instance;
    }

    public HandSlot()
    {
        // Otherwise handslot will be null
        instance = this;
    }

    GameObject item; // item in hand slot

    public Image icon; // icon of item
    public GameObject removeButton; // button for deleting

    public void delete()
    {
        //inventory.instance.delete(item); 
        //holdCuttedObject.dropHand();
        if (identify(item).isGroup("pickUpAble"))
        {
            inventoryUI.removePreview();
            player.GetComponent<pickUpObject>().drop();
        } else
        {
            Destroy(item);
            player.GetComponent<player_status>().Hands_change(Hands.free);
        }
        clear();
    }

    public void set(GameObject newItem)
    {
        Debug.Log("Set hand");
        item = newItem;

        icon.sprite = null;
        // Assign to a sprite
        icon.enabled = true;
        removeButton.SetActive(true);

    }

    public void clear()
    {
        // Clear all
        item = null;
        icon.sprite = null;
        icon.enabled = false;
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
}
