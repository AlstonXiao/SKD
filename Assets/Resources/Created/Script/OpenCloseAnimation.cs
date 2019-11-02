using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// This class is used to control animator of the door
/// </para>
/// Updated: 2/4/2019<para/>
/// Author: Yan Xiao<para/>
/// Attached object: Door<para/>
/// </summary>
public class OpenCloseAnimation : MonoBehaviour {
    public GameObject player;
    Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	/// <summary>
    /// If the user pressed the key E to open the door
    /// </summary>
	void Update () {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (Input.GetKey(KeyCode.E) && distance < 35){
            anim.SetBool("Open", true);
        }

    }

    /// <summary>
    /// Make the flag back to false
    /// </summary>
    void backToFalse(){
        anim.SetBool("Open", false);
    }
}
