using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeesawTestCubeDrop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
