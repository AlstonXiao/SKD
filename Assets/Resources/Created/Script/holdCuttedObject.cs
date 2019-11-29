using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

public class holdCuttedObject : MonoBehaviour
{
    HandSlot handSlot;

    public Camera           playerCamera;
    public float            spawnDistance;// distance the object is in front of camera, const to each object

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
    // public float scaleSpeed;
    public float fartherSpeed;
    public float rotationSpeed;
    public float minimunDistance;

    public float minHoldingDistance = 0.5f;

    public float maxHoldingDistance = 2f;


    [Header("Debug")]  
    public GameObject       holdingObject = null;
    public bool             havePicked = false;
    public float            fartherOrCloserFactor = 1;
    public int              rotationMode;

    public player_status           status_script;
    inventory               inventory_script;

    // Use this for initialization
    void Start () {
        handSlot = HandSlot.instance;
        status_script = this.GetComponent<player_status>();
        inventory_script = this.GetComponent<inventory>();
        // spawnDistance = 0;
        fartherOrCloserFactor = 1.0f;
	}

    public static holdCuttedObject instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of holdCuttedObject");
            return;
        }
        instance = this;
    }


    // Update is called once per frame
    void Update () {

        // This part of code is used to track if user currently is holding a object in his hands
        if (holdingObject != null){   
            holdingObject.transform.position = CalculatePosition();

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

            if (Input.GetAxis("Mouse ScrollWheel") != 0) {
                fartherOrCloserFactor = fartherOrCloserFactor + fartherSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime; 

                if(fartherOrCloserFactor < minHoldingDistance)
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
                    handSlot.clear();
                }
                fartherOrCloserFactor = 1;

            }
            if (Input.GetKeyDown(allKeys.putInInventory)) { // put the cutted object into the inventory
                inventory_script.putIn(holdingObject);
                while (status_script.Hands_free(Hands.cutted) != true) ;
                holdingObject = null;
                fartherOrCloserFactor = 1;
            }

        } 

	}

    // waited to be remove
    public void cutSpace(GameObject ori){
        ori.GetComponent<CuttedObject>().istrigger();
        holdingObject = ori;

        handSlot.set(holdingObject);

        status_script.Hands_change(Hands.cutted);
        
        if(identify(ori).getRegeditValue("pickUpDistance") != null){
            spawnDistance = (float)identify(ori).getRegeditValue("pickUpDistance");
        }
        else{
            spawnDistance = 20;
        }
        fartherOrCloserFactor = 1;
        
    }

    public void takeOut(int num)
    {
        if (!status_script.Hands_avaliable()) {
            return;
        }
        GameObject ret = this.GetComponent<inventory>().takeOut(num);
        if (ret != null) {
            cutSpace(ret);
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
