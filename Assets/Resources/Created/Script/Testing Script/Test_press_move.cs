using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_press_move : MonoBehaviour
{
    // Start is called before the first frame update
    public KeyCode dirone;
    public KeyCode dirtwo;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(dirone)){
           GetComponent<Rigidbody>().velocity = new Vector3(2, 0, 0) ;//* Time.deltaTime;

        }else if (Input.GetKey(dirtwo)){
           GetComponent<Rigidbody>().velocity = new Vector3(-2, 0, 0);// * Time.deltaTime;

        } else {
            GetComponent<Rigidbody>().velocity  = new Vector3(0, 0, 0);;
        }
    }
}
