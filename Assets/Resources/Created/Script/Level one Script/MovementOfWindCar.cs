using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// <para>
/// This class is used to animate the windmill
/// </para>
/// Updated: 4/15/2019<para/>
/// Author: Yan Xiao<para/>
/// Change Log: <para/> 
/// Attached object: moving wheel of the wind mill <para/>
/// </summary>
public class MovementOfWindCar : MonoBehaviour {
    //public float speed = 5f;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 10 *Time.deltaTime, Space.Self);
    }

}
