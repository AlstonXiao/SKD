using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using static publicMethods.PublicMethods;

/// <summary>
/// This class is the trigger example for our centralized event system. <para/>
/// Updated: 12/26/2019<para/>
/// Author: Yan Xiao<para/>
/// Attached object: Anything<para/>
public class TestTrigger : MonoBehaviour
{
    // public UnityEvent buttonpress;

    //We don't want to use this C# event system since we have better Unity event system
    //public delegate void onkpress(int ahugenumber);
    //static public event onkpress klisteners;

    // C# event system
    //public void registers(onkpress f){
    //    klisteners += f;
    //}

    // Update is called once per frame
    void Update()
    {
        // if someone press the button, trigger the event by calling this method. And we are require to put a event info though 
        // it is not needed
        if (Input.GetKeyDown(KeyCode.K)){
            EventManager.TriggerEvent(TypeOfEvent.test, new NoInfo());
        }
        
    }

    // C# event system
    //void haha(){
    //    if (klisteners != null){
    //        klisteners(1);
    //    }

    //}
}
