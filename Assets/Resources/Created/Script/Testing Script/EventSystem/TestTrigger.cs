using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    public UnityEvent buttonpress;

    public delegate void onkpress(int ahugenumber);
    static public event onkpress klisteners;

    void Start()
    {
        
    }

    public void registers(onkpress f){
        klisteners += f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)){
            EventManager.TriggerEvent(publicMethods.PublicMethods.TypeOfEvent.test, new NoInfo());
        }
        
    }

    void haha(){
        if (klisteners != null){
            klisteners(1);
        }

    }
}
