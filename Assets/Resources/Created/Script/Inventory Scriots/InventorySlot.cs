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
/// Author: Roland Jiang<para/>
/// Attached object: inventory<para/>
/// Updates: <para/>
/// </summary>
public class InventorySlot : MonoBehaviour
{
    GameObject item; // item in this slot

    public InventoryUI inventoryUI; // Reference to inventoryUI
    public Image icon; // icon of this item (not used yet)
    public GameObject removeButton; // remove button for deleting
    public TMPro.TextMeshProUGUI name;
    HandSlot handSlot; // Reference to slot
    //public holdCuttedObject holdCuttedObject; // Reference to holdCuttedObject

    public GameObject player; 

    private void Start()
    {
        // Instantiates
        inventoryUI = InventoryUI.instance;
        handSlot = HandSlot.instance;
        //holdCuttedObject = holdCuttedObject.instance;
    }

    public void delete()
    {
        // deletes this item from inventory
        if (identify(item).isGroup("pickUpAble"))
        {
            bool emptyHandSlot = handSlot.isEmpty();
            selectItem();

            //player.GetComponent<pickUpObject>().takeOut(player.GetComponent<inventory>().inventoryList.IndexOf(item));
            handSlot.delete();

            if (!emptyHandSlot)
            {
                int newIndex = player.GetComponent<inventory>().inventoryList.Count - 1;
                if (identify(player.GetComponent<inventory>().inventoryList[newIndex]).isGroup("pickUpAble"))
                {
                    player.GetComponent<pickUpObject>().takeOut(newIndex);
                }
                else
                {
                    player.GetComponent<holdCuttedObject>().takeOut(newIndex); // take out this item from inventory
                }
            }

        } else
        {
            inventory.instance.delete(item);
        }
        clear();
    }

    public void add(GameObject newItem)
    {
        item = newItem; // set item
        if (identify(item).isGroup("pickUpAble"))
        {
            name.text = "pickedUp_1";
            icon.sprite = Sprites.getSprite(1);
        }
        else
        {
            name.text = "cutted_1";
            icon.sprite = Sprites.getSprite(0);
        }
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
        name.text = "Empty";
        removeButton.SetActive(false);
    }

    public void selectItem()
    {
        if (!handSlot.isEmpty()) // if item is in hand already
        {
            Debug.Log("Swapping objects");
            //inventory.instance.putIn(handSlot.getItem());
            if (identify(handSlot.getItem()).isGroup("pickUpAble"))
            {
                Debug.Log("Putting in pickupable");
                player.GetComponent<inventory>().putIn(handSlot.getItem());
                player.GetComponent<player_status>().Hands_free(Hands.pickUpAble);
            }
            else
            {
                Debug.Log("Putting in cutted");
                player.GetComponent<inventory>().putIn(handSlot.getItem());
                player.GetComponent<player_status>().Hands_free(Hands.cutted);
                player.GetComponent<holdCuttedObject>().holdingObject = null;
                player.GetComponent<holdCuttedObject>().fartherOrCloserFactor = 1;
            }
            inventoryUI.removePreview();
        }
        Debug.Log("taking out");
        Debug.Log("Hands free? " + player.GetComponent<player_status>().Hands_available());
        int index = player.GetComponent<inventory>().inventoryList.IndexOf(item);
        if (identify(item).isGroup("pickUpAble"))
        {
            player.GetComponent<pickUpObject>().takeOut(index);
        }
        else
        {
            player.GetComponent<holdCuttedObject>().takeOut(index); // take out this item from 
        }
        inventoryUI.createPreview();
    }
}
