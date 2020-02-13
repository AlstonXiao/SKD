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
/// updated: <para/>
/// </summary>
public class inventory : MonoBehaviour
{
    public static inventory instance; // Reference to itself
    public InventoryUI inventoryUI; // Reference to inventoryUI
    public HandSlot handSlot; // reference to the slot for hand

    private void Awake()
    {
        if (instance != null) // instantiates itself if it's null
        {
            Debug.LogWarning("More than one instance of inventory");
            return;
        }
        instance = this;
    }


    public delegate void OnItemChanged(); // Used for detecting item change and updating UI
    public OnItemChanged onItemChangedCallback;
    public int capacity = 9; // Max capacity of inventory


    // Start is called before the first frame update

    public GameObject           inventoryField;


    public List<GameObject>    inventoryList = new List<GameObject>();
    TMPro.TextMeshProUGUI       inventoryText;
    player_status               status_script;

    void Start()
    {
        inventoryUI = InventoryUI.instance;
        handSlot = HandSlot.instance;
        inventoryText = inventoryField.GetComponent<TMPro.TextMeshProUGUI>();
        status_script = this.GetComponent<player_status>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int j  = 0; j < 8; j++) {
            if (Input.GetKeyDown(KeyCode.Alpha1+j)) {
                if(identify(inventoryList[j]).isGroup("pickUpAble")){
                    this.GetComponent<pickUpObject>().takeOut(j);
                }
                else if(identify(inventoryList[j]).isGroup("cutted")){
                    this.GetComponent<holdCuttedObject>().takeOut(j);
                }
            }
        }
    }

    public void invAdd(GameObject obj)
    {
        inventoryUI.removePreview();
        inventoryList.Add(obj);
        handSlot.clear();

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public void putIn(GameObject picked){
        Debug.Log("put in");
        if (inventoryList.Count >= capacity) // Check if full
        {
            Debug.Log("Can't put in inventory");
            // Could destroy item?
            return;
        }

        //inventoryUI.removePreview();
        //inventoryList.Add(picked); 
        //handSlot.clear();
        invAdd(picked);

        picked.SetActive(false);
        string temp_inventory = "";
        // generate the text below the screen
        for (int i = 0; i < inventoryList.Count; i++)
        {
            //display information of each item in the inventory
            temp_inventory = temp_inventory + (i + 1) + ", " + identify(inventoryList[i]).getName() + " ";
        }
        inventoryText.text = temp_inventory;

        picked = null; //set hand free

        //if (onItemChangedCallback != null)
        //{
        //    onItemChangedCallback.Invoke();
        //}
    }

    public GameObject takeOut(int j){

        GameObject picked = new GameObject();
        if (inventoryList.Count >= j+1) {
            inventoryUI.removePreview();
            picked = inventoryList[j]; // take the onject out of the inventory
            handSlot.set(inventoryList[j]);
            inventoryList.RemoveAt(j);

            picked.SetActive(true);

            string temp_inventory = "";
            for (int i = 0; i < inventoryList.Count; i++) {
                temp_inventory = temp_inventory + (i + 1) + ", " + identify(inventoryList[i]).getName() + " ";
            }
            inventoryText.text = temp_inventory;

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke(); // update UI
            }

            return picked;
        }
        return null;
        
        
    }

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
