using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickStatus : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {

    }

    public void pick() {
        this.transform.localScale = 0.1f * this.transform.localScale;
    }

    public void unpick() {
        this.transform.localScale = 10f * this.transform.localScale;
    }
}
