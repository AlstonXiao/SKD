using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static publicMethods.PublicMethods;
public class showText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    TypeOfEvent collid = TypeOfEvent.doorText;
    TypeOfEvent leave = TypeOfEvent.doorTextOut;
    doorTextInfo dt = new doorTextInfo();
    doortTextInfoOut dto = new doortTextInfoOut();
    UnityAction<EventInfo> act;
    UnityAction<EventInfo> textout;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

        act = new UnityAction<EventInfo>(eventFunc);
        textout = new UnityAction<EventInfo>(eventFuncOut);
        EventManager.startListen(collid, act);
        EventManager.startListen(leave, textout);
        
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
    void eventFunc(EventInfo info){
        text.text = "Press <-: push the door to the left\nPress ->: push the door to the right";
    }
    void OnTriggerExit(Collider other){
        if (player == other.gameObject){
            EventManager.TriggerEvent(leave, dto);
        }
        
    }

    void eventFuncOut(EventInfo info){
        text.text = "";
    }
}

