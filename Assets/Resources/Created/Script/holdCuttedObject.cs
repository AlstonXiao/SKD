using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is used to hold and put up/put down the object generated in the scene and add/remove 
/// the item in inventory system. We allow the player to adjust its position and rotation carefully.
/// </para>
/// Updated: 12/25/2019<para/>
/// Author: Yan Xiao, Roland<para/>
/// Attached object: player<para/>
/// Updates: TODO: Calculate position need to be change since the overall scale of the game has changed<para/>
/// </summary>
public class holdCuttedObject : MonoBehaviour
{
    HandSlot handSlot;
    InventoryUI inventoryUI;

    public Camera           playerCamera;
    

    [System.Serializable]
    public class keys_for_pickup
    {
        public KeyCode putDown;
        public KeyCode putInInventory;
        [Space(10)]
        // public KeyCode largerKey;
        // public KeyCode smallerKey;

        public KeyCode changeMode;
        public KeyCode rotationF;
        public KeyCode rotationB;

        [Space(10)]
        public KeyCode resetKey;
    }

    public keys_for_pickup  allKeys;

    [Space(10)]
    public float defaultSpawnDistance = 20; // default distance, can be overide in the regidit
    public float defaultMinHoldingDistance = 0.5f; // relative scale
    public float defaultMaxHoldingDistance = 2f;
    public float fartherSpeed;
    public float rotationSpeed;
    public DuplicatedObjectPlacingType limitationType;
    public int duplicatedLimitation;

    [Header("Debug")]
    public float            minHoldingDistance; // relative scale
    public float            maxHoldingDistance;
    public float            spawnDistance;// distance the object is in front of camera
    public GameObject       holdingObject = null;
    public bool             havePicked = false;
    public float            fartherOrCloserFactor = 1;
    public int              rotationMode; // 0 up and down, 1 left and right
    public int              itemPlaced;
    public int              itemExist;

    private player_status   status_script;
    private inventory       inventory_script;

    public static holdCuttedObject instance;

    public enum DuplicatedObjectPlacingType {None, itemPlaced, itemExist };

    void Start () {
        inventoryUI = InventoryUI.instance;
        handSlot = HandSlot.instance;
        status_script = this.GetComponent<player_status>();
        inventory_script = this.GetComponent<inventory>();
        fartherOrCloserFactor = 1.0f;
        itemPlaced = 0;
        itemExist = 0;
	}

    
    /// <summary>
    /// Single Hold Cutted Object instance in the game
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of holdCuttedObject");
            return;
        }
        instance = this;
    }

    
    void Update () {

        // This part of code is used to track if user currently is holding a object in his hands
        if (holdingObject == null)
            return;
        holdingObject.transform.position = CalculatePosition();

        // if the screen is busy, then we do not allow any other instructions
        if (!status_script.Scree_free())
            return;

        // Rotation of the object 
        // switch rotation mode
        // if rotation mode == 0, rotate the X axis, if rotation mode == 1, rotate the y axis
        if (Input.GetKeyDown(allKeys.changeMode))
            rotationMode = (rotationMode + 1) % 2;

        if (Input.GetKey(allKeys.rotationF))
            holdingObject.transform.Rotate(new Vector3((1 - rotationMode), rotationMode, 0) * rotationSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey(allKeys.rotationB))
            holdingObject.transform.Rotate(new Vector3((1 - rotationMode), rotationMode, 0) * rotationSpeed * Time.deltaTime * -1, Space.World);

        // scale of the object
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            fartherOrCloserFactor = fartherOrCloserFactor + fartherSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
            // saturated the scale
            fartherOrCloserFactor = Mathf.Min(fartherOrCloserFactor, maxHoldingDistance);
            fartherOrCloserFactor = Mathf.Max(fartherOrCloserFactor, minHoldingDistance);
        }

        // if user want to put down the object 
        if (Input.GetKeyDown(allKeys.putDown))
        {
            if (holdingObject.GetComponent<CuttedObject>().putAllowed() && PutDuplicatedObjectAllowed())
            {
                if (status_script.Hands_free(Hands.cutted) != true)
                    Debug.LogError("Cannot Free Hands while dropping cutted objects");

                holdingObject.GetComponent<Rigidbody>().detectCollisions = true;
                if (holdingObject.GetComponent<CuttedObject>() != null)
                {
                    holdingObject.GetComponent<CuttedObject>().startTracking();
                    holdingObject.GetComponent<CuttedObject>().notIstrigger();
                }
                itemPlaced++;
                itemExist++;
                ResetToDefault();
            }

        }

        // put the cutted object into the inventory
        if (Input.GetKeyDown(allKeys.putInInventory))
            PutInInventory();
	}

    public void PutInInventory()
    {
        inventory_script.putIn(holdingObject);
        if (status_script.Hands_free(Hands.cutted) != true)
        {
            Debug.LogError("Cannot Free Hands while putting cutted object to the inventory");
        }
        ResetToDefault();
    }

    public void deleteCuttedObject(GameObject item)
    {
        if (item == holdingObject)
        {
            ResetToDefault();
            if (status_script.Hands_free(Hands.cutted) != true)
                Debug.LogError("Cannot Free Hands while putting cutted object to the inventory");

        }
        Destroy(item);
    }

    /// <summary>
    /// This method is called when we want to put a cutted object on the player's hand
    /// </summary>
    /// <param name="ori">The cutted object</param>
    public void putCuttedItemToHand(GameObject ori){
        ori.GetComponent<CuttedObject>().istrigger();
        holdingObject = ori;

        inventoryUI.AddToHandSlot(ori);

        status_script.Hands_change(Hands.cutted);
        
        spawnDistance = identify(ori).getRegeditValue("cuttedSpawnDistance") != null ? (float)identify(ori).getRegeditValue("cuttedSpawnDistance") : defaultSpawnDistance;
        minHoldingDistance = identify(ori).getRegeditValue("minimumDistance") != null ? (float)identify(ori).getRegeditValue("minimumDistance") : defaultMinHoldingDistance;
        maxHoldingDistance = identify(ori).getRegeditValue("maximumDistance") != null ? (float)identify(ori).getRegeditValue("maximumDistance") : defaultMaxHoldingDistance;
        fartherOrCloserFactor = 1;
    }

    /// <summary>
    /// This private helper method is used to calculate the position of the holding gem
    /// </summary>
    /// <returns>the absolute position of the gem in space</returns>
    private Vector3 CalculatePosition()
    {
        Vector3 playerPos = this.transform.position; //+ new Vector3(0, 4, 0);
        Vector3 playerDirection = playerCamera.transform.forward;
        Vector3 spawnPos = playerPos + playerDirection * spawnDistance * fartherOrCloserFactor;
        return spawnPos;
    }

    private void ResetToDefault()
    {
        holdingObject = null;
        fartherOrCloserFactor = 1;
        minHoldingDistance = defaultMinHoldingDistance;
        maxHoldingDistance = defaultMaxHoldingDistance;
        inventoryUI.RemoveFromHandSlot();
    }

    /// <summary>
    /// This method is used to test if the player is allowed to put more item into the scene
    /// </summary>
    /// <returns>True if not exceed the limitation, false otherwise</returns>
    private bool PutDuplicatedObjectAllowed()
    {
        if (limitationType == DuplicatedObjectPlacingType.None) return true;
        else if (limitationType == DuplicatedObjectPlacingType.itemExist) return itemExist < duplicatedLimitation;
        else return itemPlaced < duplicatedLimitation;
     }

    /// <summary>
    /// This method will be called when deduplication happened
    /// </summary>
    public void DestoriedDuplicatedObjectFromScene()
    {
        itemExist--;
    }
}
