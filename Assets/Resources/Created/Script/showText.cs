using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static publicMethods.PublicMethods;
public class showText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    TypeOfEvent collid = TypeOfEvent.doorText;
    doorTextInfo dt = new doorTextInfo();
    UnityAction<EventInfo> act;
    // Start is called before the first frame update
    void Start()
    {
        act = new UnityAction<EventInfo>(eventFunc);
        EventManager e = new EventManager();
    }

    // Update is called once per frame
    void Update()
    {

        EventManager.startListen(collid, act);
    }
    void onTriggerEnter(Collider other){ 
        EventManager.TriggerEvent(collid, dt);
        
    }
    void eventFunc(EventInfo info){
        text.text = "123123";
    }
    void onTriggerExit(Collider other){
        
    }
}

