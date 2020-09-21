using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{

    GameObject player;
    bool canClimb = false;
    public float speed = 1;

    void OnTriggerEnter(Collider coll)
    {
        //Debug.Log("Ladder collision");
        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("canClimb true");
            canClimb = true;
            player = coll.gameObject;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            canClimb = false;
            player = null;
        }
    }

    void Update()
    {
        if (canClimb)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Debug.Log("Up");
                player.transform.Translate(new Vector3(0, 1, 0) * 2 * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                player.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * speed);
            }
        }
    }
}