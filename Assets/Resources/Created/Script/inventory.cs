using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

public class inventory : MonoBehaviour
{
    // Singleton

    public static inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory");
            return;
        }
        instance = this;
    }


    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int capacity = 9;


    // Start is called before the first frame update

    public GameObject           inventoryField;


    public List<GameObject>    inventoryList = new List<GameObject>();
    TMPro.TextMeshProUGUI       inventoryText;
    player_status               status_script;

    void Start()
    {
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

    public void putIn(GameObject picked){
        // Check if full
        Debug.Log("put in");
        if (inventoryList.Count >= capacity)
        {
            Debug.Log("Full inventory");
            // Could destroy item?
            return;
        }

        inventoryList.Add(picked);
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

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public GameObject takeOut(int j){

        GameObject picked = new GameObject();
        if (inventoryList.Count >= j+1) {
            picked = inventoryList[j]; // take the onject out of the inventory
            inventoryList.RemoveAt(j);
            picked.SetActive(true);

            string temp_inventory = "";
            for (int i = 0; i < inventoryList.Count; i++) {
                temp_inventory = temp_inventory + (i + 1) + ", " + identify(inventoryList[i]).getName() + " ";
            }
            inventoryText.text = temp_inventory;

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
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
            onItemChangedCallback.Invoke();
        }

    }
    public int getCount(){
        return inventoryList.Count;
    }
    public List<GameObject> getInventory(){
        return inventoryList;
    }
}
