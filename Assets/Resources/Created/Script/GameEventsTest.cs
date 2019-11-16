using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsTest : MonoBehaviour
{
    public static GameEventsTest current;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
    }

    public event Action onApproachTrigger;

    public void ApproachTrigger()
    {
        if (onApproachTrigger != null)
        {
            onApproachTrigger();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
