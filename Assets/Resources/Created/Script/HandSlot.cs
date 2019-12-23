using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandSlot : MonoBehaviour
{
    // Singleton

    public static HandSlot instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of handslot");
            return;
        }
        instance = this;
    }

    public HandSlot()
    {
        // Otherwise handslot will be null
        instance = this;
    }

    //public delegate void OnHandChanged();
    //public OnHandChanged onHandChangedCallback;

    GameObject item;

    public Image icon;
    public GameObject removeButton;

    public void delete()
    {
        inventory.instance.delete(item);
    }

    public void set(GameObject newItem)
    {
        Debug.Log("Set hand");
        item = newItem;

        icon.sprite = null;
        // Assign to a sprite
        icon.enabled = true;
        removeButton.SetActive(true);
        //removeButton.interactable = true;

        //if (onHandChangedCallback != null)
        //{
        //    onHandChangedCallback.Invoke();
        //}

    }

    public void clear()
    {
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
