using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

/// <summary>
/// This class is the listener example for our centralized event system. <para/>
/// Updated: 12/26/2019<para/>
/// Author: Yan Xiao<para/>
/// Attached object: Anything<para/>
/// </summary>
public class TestEventListen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        // we tell the event manager we want to listen to the event "test". 
        // When it triggers, we want to perform the action move
        EventManager.startListen(TypeOfEvent.test, move);
    }

    // Update is called once per frame
    void Update()
    {
        // This is the example when we are no longer interested in the event "move"
        if (Input.GetKeyDown(KeyCode.P)) {
            EventManager.StopListening(publicMethods.PublicMethods.TypeOfEvent.test, move);
        }
    }

    /// <summary>
    /// This method is the action we want to perform, It has to take in a parameter of type "EventInfo",
    /// though you might not need it.
    /// </summary>
    /// <param name="number">the required field, you can put information here</param>
    public void move(EventInfo number){
        print("triggered"+this.name);
    }
}