using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This is the class that gives an example of a single unity event system. The event controller is in this class <para/>
/// </summary>
public class NewETrigger : MonoBehaviour
{
     public UnityEvent buttonpress; // this is the event we care about. 


    void Start()
    {      
        // we add listener to this event, if this event is triggered, something will perform
        buttonpress.AddListener(haha);
    }

    // Update is called once per frame
    void Update()
    {
        // if someone press the letter K, we want to trigger the event, and the buttonpress will envoke the haha method
        if (Input.GetKeyDown(KeyCode.K)){
            buttonpress.Invoke();
           
        }
    }

    /// <summary>
    /// This is the action we want to do when the event is triggered
    /// </summary>
    public void haha(){
        print("this is great");
    }
}
