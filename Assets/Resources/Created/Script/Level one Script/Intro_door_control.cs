using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// <para>
/// This class is used to control when the door should open
/// </para>
/// Updated: 4/15/2019<para/>
/// Author: Yan Xiao<para/>
/// Change Log: <para/> 
/// Attached object: the door of entry of level 1 <para/>
public class Intro_door_control : MonoBehaviour
{
    public GameObject leftCube;
    public GameObject rightCube;

    public int leftOffset;
    public int rigthOffset;

    [Header("Debug")]
    public bool leftActive = false;
    public bool rightActive = false;
    public bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rightCube.transform.position.z > rigthOffset) {
            
            rightActive = true;
        }
        if (leftCube.transform.position.z > leftOffset) {
            leftActive = true;
        }
        if (rightActive && leftActive && !done) {
            for (int i = 0; i < 20; i++) {
                this.transform.position += new Vector3(1, 0, 0);
            }
            done = true;
            
        }
    }
}
