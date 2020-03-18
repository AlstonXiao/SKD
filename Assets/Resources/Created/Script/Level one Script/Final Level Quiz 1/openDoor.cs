using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static publicMethods.PublicMethods;

public class openDoor : MonoBehaviour {
    public GameObject door;
    public KeyCode left;
    public KeyCode right;
    TypeOfEvent collid = TypeOfEvent.doorText;
    TypeOfEvent leave = TypeOfEvent.doorTextOut;
    doorTextInfo dt = new doorTextInfo();
    doortTextInfoOut dto = new doortTextInfoOut();
    UnityAction<EventInfo> act;
    UnityAction<EventInfo> moveout;
    public GameObject player;
    bool goLeft;
    bool goRight;
    // Start is called before the first frame update
    void Start()
    {
        door.GetComponent<openTheDoor>().enabled = false;
        act = new UnityAction<EventInfo>(moveDoor);
        moveout = new UnityAction<EventInfo>(notMove);
        EventManager.startListen(collid, act);
        EventManager.startListen(leave, moveout);
        
    }

    // Update is called once per frame
    void Update()
    {
        // EventManager.startListen(collid, act);
        // EventManager.startListen(leave, textout);
    }
    void OnTriggerEnter(Collider other){
        if (player == other.gameObject){
            EventManager.TriggerEvent(collid, dt);
        }
        
        
    }
    void moveDoor(EventInfo info){
        //print("ok");
        // if event info = enable, enable update
        // if event info = disable, disable update
        // if(Input.GetKeyDown(left)){
        //     goLeft = true;
        // }
        // if(Input.GetKeyUp(left)){
        //     goLeft = false;
        // }
        // if(Input.GetKeyDown(right)){
        //     goRight = true;
        // }
        // if(Input.GetKeyUp(right)){
        //     goRight = false;
        // }
        // if(goLeft){
        //     door.GetComponent<Rigidbody>().velocity = new Vector3(-1, 0, 0);
        // }
        // else if(goRight){
        //     door.GetComponent<Rigidbody>().velocity = new Vector3(1, 0, 0);
        // }
        // else{
        //     door.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        // }
        door.GetComponent<openTheDoor>().enabled = true;
    }
    void OnTriggerExit(Collider other){
        if (player == other.gameObject){
            EventManager.TriggerEvent(leave, dto);
        }
        
    }

    void notMove(EventInfo info){
        door.GetComponent<openTheDoor>().enabled = false;
    }
}
