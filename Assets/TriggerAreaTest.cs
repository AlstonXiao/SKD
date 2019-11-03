using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameEventsTest.current.ApproachTrigger();
    }
}
