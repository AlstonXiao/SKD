using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEventsTest.current.onApproachTrigger += OnApproach;
    }

    public void OnApproach()
    {
        gameObject.transform.Translate(new Vector3(10,20,30));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
