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
    public float fartherSpeed;
    public float rotationSpeed;

    public float minHoldingDistance = 0.5f; // relative scale
    public float maxHoldingDistance = 2f;


    [Header("Debug")]
    public float            spawnDistance;// distance the object is in front of camera
    public GameObject       holdingObject = null;
    public bool             havePicked = false;
    public float            fartherOrCloserFactor = 1;
    public int              rotationMode; // 0 up and down, 1 left and right

    public player_status           status_script;
    inventory               inventory_script;

    public static holdCuttedObject instance;
    
    void Start () {
        handSlot = HandSlot.instance;
        status_script = this.GetComponent<player_status>();
        inventory_script = this.GetComponent<inventory>();
        // spawnDistance = 0;
        fartherOrCloserFactor = 1.0f;
	}

    
    /// <summary>
    /// TODO
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
        if (holdingObject != null){   
            holdingObject.transform.position = CalculatePosition();

            // Rotation of the object 
            // switch rotation mode
            if (Input.GetKeyDown(allKeys.changeMode)){
                if(rotationMode == 1){
                    rotationMode = 0;
                }
                else{
                    rotationMode += 1;
                } 
            }

            if(Input.GetKey(allKeys.rotationF)){
                if(rotationMode == 0){
                    holdingObject.transform.Rotate(new Vector3(1, 0, 0) * rotationSpeed * Time.deltaTime, Space.World);
                }
                else if(rotationMode == 1){
                    holdingObject.transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime, Space.World);
                }
                
            }

            if(Input.GetKey(allKeys.rotationB)){
                if(rotationMode == 0){

                    holdingObject.transform.Rotate(new Vector3(1, 0, 0) * rotationSpeed * Time.deltaTime * -1, Space.World);
                }
                else if(rotationMode == 1){
                    holdingObject.transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime * -1, Space.World);
                }
            }

            // scale of the object
            if (Input.GetAxis("Mouse ScrollWheel") != 0) {
                fartherOrCloserFactor = fartherOrCloserFactor + fartherSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
                // saturated the scale
                if (fartherOrCloserFactor < minHoldingDistance)
                {
                    fartherOrCloserFactor = minHoldingDistance;
                }
                if (fartherOrCloserFactor > maxHoldingDistance)
                {
                    fartherOrCloserFactor = maxHoldingDistance;
                }
            }
         
            // if user want to put down the object 
            if (Input.GetKeyDown(allKeys.putDown)) {
                if (holdingObject.GetComponent<CuttedObject>().putAllowed()) {
                    while (status_script.Hands_free(Hands.cutted) != true) ;
                    holdingObject.GetComponent<Rigidbody>().detectCollisions = true;
                    if (holdingObject.GetComponent<CuttedObject>() != null) {
                        holdingObject.GetComponent<CuttedObject>().startTracking();
                        holdingObject.GetComponent<CuttedObject>().notIstrigger();
                    }
                    holdingObject = null;
                    if (handSlot != null)
                    {
                        handSlot.clear();
                    }                }
                fartherOrCloserFactor = 1;

            }

            // put the cutted object into the inventory
            if (Input.GetKeyDown(allKeys.putInInventory)) { 
                inventory_script.putIn(holdingObject);
                while (status_script.Hands_free(Hands.cutted) != true) ;
                holdingObject = null;
                fartherOrCloserFactor = 1;
            }

        } 

	}

    /// <summary>
    /// This method is called when we want to put a cutted object on the player's hand
    /// </summary>
    /// <param name="ori">The cutted object</param>
    public void putCuttedItemToHand(GameObject ori){
        ori.GetComponent<CuttedObject>().istrigger();
        holdingObject = ori;

        if (handSlot != null)
        {
            handSlot.set(holdingObject);
        }

        status_script.Hands_change(Hands.cutted);
        
        if(identify(ori).getRegeditValue("pickUpDistance") != null){
            spawnDistance = (float)identify(ori).getRegeditValue("pickUpDistance");
        }
        else{
            spawnDistance = defaultSpawnDistance;
        }
        fartherOrCloserFactor = 1;
        
    }

    /// <summary>
    /// This method is called when the player want to take out an object from the inventory. 
    /// It will take the position as the parameter and get that object in the inventory class
    /// </summary>
    /// <param name="num">The position of that object</param>
    public void takeOut(int num)
    {
        if (!status_script.Hands_avaliable()) {
            return;
        }
        GameObject ret = this.GetComponent<inventory>().takeOut(num);
        if (ret != null) {
            putCuttedItemToHand(ret);
        } else {
            print("Error");
        }
       
    }

    /// <summary>
    /// This private helper method is used to calculate the position of the holding gem
    /// </summary>
    /// <returns>the absolute position of the gem in space</returns>
    private Vector3 CalculatePosition()
    {
        Vector3 playerPos = this.transform.position + new Vector3(0, 4, 0);
        Vector3 playerDirection = playerCamera.transform.forward;
        Vector3 spawnPos = playerPos + playerDirection * spawnDistance * fartherOrCloserFactor;
        return spawnPos;
    }
}
