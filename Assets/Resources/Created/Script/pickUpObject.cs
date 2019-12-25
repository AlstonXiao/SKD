using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;


/// <summary>
/// <para>
/// This class is used to pick up/put down object in the scene and add/remove 
/// the item in inventory system
/// </para>
/// Updated: 3/29/2019<para/>
/// Author: Yan Xiao<para/>
/// Attached object: player<para/>
/// Updates: 3/29 Updates distance for normal objects<para/>
/// </summary>
public class pickUpObject : MonoBehaviour {

    public Camera           playerCamera;
    public float            rayRange; // the maximum distance of the object that can be picked
    public float            spawnDistance;// distance the object is in front of camera, const to each object

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

    // public List<GameObject> inventory;
    player_status           status_script;

    public float fartherOrCloserFactor = 1;
      

    // Use this for initialization
    void Start () {
        status_script = this.GetComponent<player_status>();
        fartherOrCloserFactor = 1.0f;
	}
	

    public void takeOut(int num){
        if(!status_script.Hands_avaliable()){
            return;
        }
        GameObject ret = this.GetComponent<inventory>().takeOut(num);
        if(ret != null){
            picked = ret;
        }
        this.GetComponent<player_status>().Hands_change(Hands.pickUpAble);
    }

    /// <summary>
    /// This private helper method is used to calculate the position of the holding gem
    /// </summary>
    /// <returns>the absolute position of the gem in space</returns>
    private Vector3 CalculatePosition()
    {
        //print(this.transform.rotation);
        Vector3 playerPos = this.transform.position + 5 * playerCamera.transform.right + 0 * playerCamera.transform.up + -3 * playerCamera.transform.forward;
        Vector3 playerDirection = playerCamera.transform.forward;
        Vector3 spawnPos = playerPos + playerDirection * spawnDistance * fartherOrCloserFactor;
        return spawnPos;
    }

    // Update is called once per frame
    void Update () {
        // This part of the script is used to check if user pressed a button to move things out of inventory 
        

        // This part of code is used to track if user currently is holding a object in his hands
        if (picked != null){
            if (!status_script.Hands_change(Hands.pickUpAble)){
            }
            picked.transform.position = CalculatePosition();
            picked.transform.rotation = this.transform.rotation;
            // if user want to put down the object
            if (Input.GetKeyDown(allKeys.putDown)) {
                
                Queue<GameObject> waittobeCut = new Queue<GameObject>();
                    Queue<GameObject> originalObjects = new Queue<GameObject>();
                    originalObjects.Enqueue(picked);
                    //waittobeCut.Enqueue(picked);
                    
                    // cut all its children
                    while (originalObjects.Count != 0) {
                        GameObject currentParent = originalObjects.Dequeue();
                        // print(currentParent);
                        foreach (Transform childs in currentParent.transform) {
                            GameObject resultChilds = childs.gameObject;
                            originalObjects.Enqueue(childs.gameObject);    
                        }
                        foreach(Collider c in currentParent.GetComponents<Collider> ()) {
                            c.enabled = true;
                        }   
                    }

                picked.GetComponent<Rigidbody>().detectCollisions = true;
                //clear the momentum right before putting the pickup object down
                picked.GetComponent<Rigidbody>().velocity = new Vector3(0.0f,0.0f,0.0f);
                picked.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f,0.0f,0.0f);
                if (!status_script.Hands_free(Hands.pickUpAble)){
                    print("error");
                }
                picked = null;
                
            }
            if (Input.GetKeyDown(allKeys.putInInventory))
            {
                
                this.GetComponent<inventory>().putIn(picked);
                if (!status_script.Hands_free(Hands.pickUpAble)){
                    print("error");
                }
                picked = null;
            }

        } else {
            if (!status_script.Hands_free(Hands.pickUpAble)){
            }
        }

        //pick up an pickupable object if hands are free
        if (status_script.Hands_avaliable()&& Input.GetKeyDown(allKeys.pickUp)) {
            
            Ray rayCast = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit rayHit;
            if (Physics.Raycast(rayCast, out rayHit, rayRange)) {
                if (!status_script.Hands_change(Hands.pickUpAble)){
                    print("error");
                }
                if (identify(rayHit.collider.gameObject).isGroup("pickUpAble")) {
                    picked = rayHit.collider.gameObject;
                    while(picked.transform.parent != null && picked.transform.parent.gameObject!= null) {
                        if (!identify(picked.transform.parent.gameObject).isGroup("pickUpAble")){
                            print("break on " + picked);
                            break;
                            
                        }
                        picked = picked.transform.parent.gameObject;
                    }

                    // disable physics when holding it
                    Queue<GameObject> originalObjects = new Queue<GameObject>();
                    originalObjects.Enqueue(picked);

                    // disable the collider of all its children
                    while (originalObjects.Count != 0) {
                        GameObject currentParent = originalObjects.Dequeue();
                        // print(currentParent);
                        foreach (Transform childs in currentParent.transform) {
                            GameObject resultChilds = childs.gameObject;
                            originalObjects.Enqueue(childs.gameObject);    
                        }
                        foreach(Collider c in currentParent.GetComponents<Collider> ()) {
                            c.enabled = false;
                        }   
                    }
                }

            }

        }
	}
}
