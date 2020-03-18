using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is used to for the inventory
/// </para>
/// Author: Yan Xiao, Roland<para/>
/// Attached object: Player<para/>
/// updated: 3/17/2020: update the data flow and some functions<para/>
/// </summary>
public class inventory : MonoBehaviour
{
    public static inventory instance; // Reference to itself
    private InventoryUI inventoryUI; // Reference to inventoryUI
    private HandSlot handSlot; // reference to the slot for hand

    public delegate void OnItemChanged(); // Used for detecting item change and updating UI
    public OnItemChanged onItemChangedCallback;

    public int capacity = 9; // Max capacity of inventory
    [Header("Debug")]
    public List<GameObject> inventoryList = new List<GameObject>();
    private player_status   status_script;

    /// <summary>
    /// This will make every script access this class easily
    /// </summary>
    private void Awake()
    {
        if (instance != null) // instantiates itself if it's null
        {
            Debug.LogWarning("More than one instance of inventory");
            return;
        }
        instance = this;
    }
    void Start()
    {
        inventoryUI = InventoryUI.instance;
        handSlot = HandSlot.instance;
        status_script = this.GetComponent<player_status>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if player have press the take out button or not
        // this will work even the UI is open
        for (int j  = 0; j < 8; j++) {
            if (Input.GetKeyDown(KeyCode.Alpha1+j)) {
                if (inventoryList.Count < j+1 || inventoryList[j] == null) return;
                takeOut(j);
            }
        }
    }

    /// <summary>
    /// This method is called when we want to put an item into inventory
    /// This will also update UI 
    /// </summary>
    /// <param name="picked">item you want to put in inventory</param>
    public void putIn(GameObject picked){
        
        if (inventoryList.Count >= capacity) // Check if full
        {
            Debug.Log("Can't put in inventory because it is full");
            // Could destroy item?
            return;
        }

        inventoryList.Add(picked);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        picked.SetActive(false);
    }

    /// <summary>
    /// This method is called only when player pressed a button to take out
    /// </summary>
    /// <param name="j">the position of the object in the inventory</param>
    public void takeOut(int j){
        if (!status_script.Hands_available() || inventoryList.Count < j + 1)
        {
            return;
        }

        GameObject itemOut = inventoryList[j]; // take the onject out of the inventory
        inventoryList.RemoveAt(j);

        itemOut.SetActive(true);

        if(identify(itemOut).isGroup("pickUpAble")){
            this.GetComponent<pickUpObject>().takeOut(itemOut);
        }
        else if(identify(itemOut).isGroup("cutted")){
            this.GetComponent<holdCuttedObject>().putCuttedItemToHand(itemOut);
        }

        // update UI
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke(); 
        }
        return;
    }

    /// <summary>
    /// Delete an object in the inventory, will not do other things but just delete
    /// </summary>
    /// <param name="obj">The object</param>
    public void delete(GameObject obj)
    {

        inventoryList.Remove(obj);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke(); // update UI
        }

    }

    public int getCount(){
        return inventoryList.Count;
    }
    public List<GameObject> getInventory(){
        return inventoryList;
    }
}
