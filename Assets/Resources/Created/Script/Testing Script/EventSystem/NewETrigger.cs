using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class NewETrigger : MonoBehaviour
{
     public UnityEvent buttonpress;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)){
            buttonpress.Invoke();
            haha();
        }
    }

    void haha(){

    }
}
