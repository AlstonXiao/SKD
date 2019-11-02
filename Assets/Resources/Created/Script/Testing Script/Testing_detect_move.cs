using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_detect_move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(20, 0, 0) * Time.deltaTime;
    }
}
