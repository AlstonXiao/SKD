using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;


/// <summary>
/// <para>
/// This class is used to pick up/put down object in the scene and add/remove 
/// the item in inventory system
/// </para>
/// Updated: 3/15/2020<para/>
/// Author: Yan Xiao<para/>
/// Attached object: player<para/>
/// Updates: 3/17/2020 Updates unpick and blocking when screen is not free.<para/>
/// Updates: 3/15/2020 Updates The visual of the objects.<para/>
/// Updates: 3/29 Updates distance for normal objects<para/>
/// </summary>
public class pickUpObject : MonoBehaviour {

    public GameObject       inventoryCanvas;
    public GameObject       displayCanvas;
    public Camera           playerCamera;
    public float            rayRange; // the maximum distance of the object that can be picked
    public float            defaultPickUpScale;
    /// the position of the object that appear on the screen. X is left right. Y is top down. Z is further and closer
    /// It is all relate to the canvas position. 
    public Vector3          placedPosition; 

    [System.Serializable]
    public class keys_for_pickup
    {
        public KeyCode pickUp;
        public KeyCode putDown;
        public KeyCode putInInventory;
    }
    public keys_for_pickup  allKeys;

    [Space(10)]
    [Header("Debug")]
    public GameObject       picked = null;
    private player_status           status_script;
    private inventory inventory;

    // Use this for initialization
    void Start () {
        status_script = this.GetComponent<player_status>();
        inventory = GetComponent<inventory>();
	}

    // Update is called once per frame
    void Update () {

        // This part of code is used to track if user currently is holding a object in his hands
        if (picked != null && status_script.Scree_free())
        {
            // if user want to put down the object
            if (Input.GetKeyDown(allKeys.putDown))
            {
                DropItemsOnHand();
                InventoryUI.instance.RemoveFromHandSlot();

            }
            // if user want to put the items into inventory
            if (Input.GetKeyDown(allKeys.putInInventory))
            {
                PutInInventory();
            }

        }

        //pick up an pickupable object if hands are free
        else if (status_script.Hands_available() && Input.GetKeyDown(allKeys.pickUp) && status_script.Scree_free())
        {
            Ray rayCast = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit rayHit;
            if (Physics.Raycast(rayCast, out rayHit, rayRange))
            {
                if (identify(rayHit.collider.gameObject) != null && identify(rayHit.collider.gameObject).isGroup("pickUpAble"))
                {
                    // step one: change the hand status
                    if (!status_script.Hands_change(Hands.pickUpAble))
                    {
                        Debug.LogWarning("PickUpObject cannot lock hands status to pickUpAble");
                    }
                    picked = rayHit.collider.gameObject;

                    // step two: find the top of the hierachy tree we want to pick up
                    picked = findTopParentOfTheType("pickUpAble", picked);

                    // step three: disable the collider of all its children
                    Queue<GameObject> originalObjects = new Queue<GameObject>();
                    originalObjects.Enqueue(picked);
                    while (originalObjects.Count != 0)
                    {
                        GameObject currentParent = originalObjects.Dequeue();
                        // print(currentParent);
                        foreach (Transform childs in currentParent.transform)
                            originalObjects.Enqueue(childs.gameObject);

                        foreach (Collider c in currentParent.GetComponents<Collider>())
                            c.enabled = false;
                    }
                    // step four: store the current parent for future reference
                    identify(picked).putRegedit("originalParent", picked.transform.parent);
                    float pickUpScale = (float)(identify(picked).getRegeditValue("pickUpScale") == null? defaultPickUpScale : identify(picked).getRegeditValue("pickUpScale"));
                    // step five: 
                    InventoryUI.instance.AddToHandSlot(picked);
                    
                    if (picked.GetComponent<Rigidbody>() != null)
                        picked.GetComponent<Rigidbody>().isKinematic = true;

                    picked.transform.SetParent(displayCanvas.transform);
                    picked.transform.localPosition = placedPosition;
                    picked.transform.localScale = (float)pickUpScale * picked.transform.localScale;
                }

            }

        }
        
	}

    /// <summary>
    /// This will put the current Item to inventory and update the information on UI
    /// </summary>
    public void PutInInventory()
    {
        if (picked == null)
        {
            Debug.LogError("put null into inventory");
        }
        inventory.putIn(picked);
        InventoryUI.instance.RemoveFromHandSlot();
        if (!status_script.Hands_free(Hands.pickUpAble))
        {
            Debug.LogWarning("Cannot free hands when put items into inventory");
        }
        picked = null;
    }

    /// <summary>
    /// This method will put the item on hand
    /// </summary>
    /// <param name="item"></param>
    public void takeOut(GameObject item)
    {
        if (!status_script.Hands_change(Hands.pickUpAble))
        {
            Debug.LogWarning("cannot let hands busy when taking out");
        }
        picked = item;
        InventoryUI.instance.AddToHandSlot(item);
    }

    /// <summary>
    /// This method is a wrapper function for drop. It can drop items both at hand and in the inventory
    /// </summary>
    /// <param name="item">items you want to drop</param>
    public void DropItem(GameObject item)
    {
        if (picked == item)
            DropItemsOnHand();
        else DropItemsNotOnHand(item);
    }

    /// <summary>
    /// Drop the item on hand. Will update UI
    /// </summary>
    private void DropItemsOnHand()
    {
        // Debug.Log("Dropping Items in pickUpObject");
        if (picked == null)
        {
            Debug.LogWarning("There is no items on hand when drop items is called");
        }
        // Debug.Log("Y coord: " + picked.transform.position.y);
        double height = picked.GetComponent<Renderer>().bounds.size.y;
        Debug.Log("Height: " + height);
        //Debug.Log("Height (Scale): " + picked.GetComponent<Renderer>().bounds.size.y * picked.transform.localScale.y);
        if (picked.transform.position.y - (0.5*height) < 100)
        {
            return;
        }

        DropHelp(picked);
        if (!status_script.Hands_free(Hands.pickUpAble))
        {
            Debug.LogWarning("cannot free hands when put it down");
        }
        float pickUpScale = (float)(identify(picked).getRegeditValue("pickUpScale") == null ? defaultPickUpScale : identify(picked).getRegeditValue("pickUpScale"));
        InventoryUI.instance.RemoveFromHandSlot();
        picked.transform.localPosition = new Vector3(0, picked.transform.localPosition.y, 1000);
        picked.transform.localScale *= 1/pickUpScale;
        picked.transform.SetParent((UnityEngine.Transform)identify(picked).getRegeditValue("originalParent"));
        picked = null;

    }

    /// <summary>
    /// Drop the item that is in the inventory.
    /// But it will not update information in the inventory
    /// </summary>
    /// <param name="item">items you want to drop</param>
    private void DropItemsNotOnHand(GameObject item)
    {
        // Debug.Log("Dropping Items in pickUpObject");

        DropHelp(item);
        item.SetActive(true);
        float pickUpScale = (float)(identify(item).getRegeditValue("pickUpScale") == null ? defaultPickUpScale : identify(item).getRegeditValue("pickUpScale"));
        item.transform.localPosition = new Vector3(0, item.transform.localPosition.y, 500);
        item.transform.localScale *= 1 / pickUpScale;
        item.transform.SetParent((UnityEngine.Transform)identify(item).getRegeditValue("originalParent"));
    }

    /// <summary>
    /// Private helper function to undo all the changes to the object when it is picked
    /// </summary>
    /// <param name="item"></param>
    private void DropHelp(GameObject item)
    {
        Queue<GameObject> originalObjects = new Queue<GameObject>();
        originalObjects.Enqueue(item);

        while (originalObjects.Count != 0)
        {
            GameObject currentParent = originalObjects.Dequeue();
            // print(currentParent);
            foreach (Transform childs in currentParent.transform)
                originalObjects.Enqueue(childs.gameObject);
            foreach (Collider c in currentParent.GetComponents<Collider>())
                c.enabled = true;
        }

        if (item.GetComponent<Rigidbody>() != null)
        {
            item.GetComponent<Rigidbody>().isKinematic = false;
            item.GetComponent<Rigidbody>().detectCollisions = true;
            //clear the momentum right before putting the pickup object down
            item.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            item.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}
