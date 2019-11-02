using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing_movement : MonoBehaviour {

    public bool left;
    public float movement;
    public bool linked;
    public Vector3 getVelocity()
    {
        if (linked) return getTheVelocity();
        throw new System.AccessViolationException();
    }

    public void locked()
    {
        linked = true;
    }

    public void unlocked()
    {
        linked = false;
    }

    // Use this for initialization
    void Start () {
        linked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!linked)
        {
            GetComponent<Rigidbody>().velocity = getTheVelocity();

        }
    }

    private Vector3 getTheVelocity()
    {
        if (transform.position.x > 500) left = false;
        if (transform.position.x < -500) left = true;
        if (left)
        {
            return new Vector3(0, -movement, 0) * Time.deltaTime;

        }
        else
        {
            return new Vector3(0, -movement, 0) * Time.deltaTime;
        }
    }
}
