using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;
public class TestEventListen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        EventManager.startListen(TypeOfEvent.test, move);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            EventManager.StopListening(publicMethods.PublicMethods.TypeOfEvent.test, move);
        }
    }

    public void move(EventInfo number){
        print("triggered"+this.name);
    }
}