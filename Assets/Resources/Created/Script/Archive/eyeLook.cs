using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// <para>
/// This class is used to make the camera rotate as the user move the mouse
/// </para>
/// Updated: 2/4/2019<para/>
/// Author: Yan Xiao<para/>
/// Attached object: Main Camera <para/>
/// </summary>
public class eyeLook : MonoBehaviour {

	// Mouse direction.
	public Vector2 mD;
    public GameObject player;

	// The capsule parent!
	//private Transform myBody;

	void Start () {

		//myBody = this.transform.parent.transform;

		Cursor.lockState = CursorLockMode.Locked;

	}
	

	void Update () {
        if (Cursor.lockState == CursorLockMode.None) 
        {
            return;
        }

		// Do we want to see mouse cursor again?
		if (Input.GetKey (KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}


		float movement = Input.GetAxis ("Vertical");
		movement *= Time.deltaTime;

		// How much has the mouse moved?
		Vector2 mC = new Vector2
			(Input.GetAxisRaw ("Mouse X") * 3f,
				Input.GetAxisRaw("Mouse Y") * 3f);

		mD += mC;


        //			myBody.localRotation =
        //			Quaternion.AngleAxis (mD.x, Vector3.up);

        Quaternion qR =
		this.transform.localRotation =
			Quaternion.AngleAxis (mD.x, Vector3.up);

		// The actual rotation happening!
		this.transform.localRotation = qR * 
			Quaternion.AngleAxis (-mD.y, Vector3.right);

		this.transform.Translate
			(Vector3.forward * movement);
        // transfrom the rotation to the player
        player.transform.rotation = Quaternion.AngleAxis(mD.x, Vector3.up);

    }
}
