using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// This class is used to track the object that is been cut
/// </para>
/// Updated: 3/29/2019<para/>
/// Author: Yan Xiao<para/>
/// Change Log: <para/> 
/// 3/1 added init and scale<para/>
/// 3/16 removed the scale.. sad <para/>
/// 3/29 Fully functioning check if it can put or not, updated putAllowed and change color <para/>
/// Attached object: Object been cut <para/>
/// </summary>
public class CuttedObject : MonoBehaviour {
    public GameObject original;

    [Header("Debug")]
    public List<GameObject> collids;
    public bool canPutOrNot;
    Material[] unables;
    Material[] originalMaterial;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public bool track = false;
     
 	
	void Start () { 
        // organize the materials
        Material unable = Resources.Load<Material>("Created/Materials/UnablePutDown");
        originalMaterial = GetComponent<MeshRenderer>().sharedMaterials;
        unables = new Material[this.GetComponent<MeshRenderer>().sharedMaterials.Length];
        for (int i = 0; i < this.GetComponent<MeshRenderer>().sharedMaterials.Length; i++) {
            unables[i] = unable;
        }

        collids = new List<GameObject>();
        // it should never be called
        if (track){
            positionOffset = this.transform.position - original.transform.position;
            rotationOffset = this.transform.eulerAngles - original.transform.eulerAngles;
        }
        
    }

	void Update () {
        // if the object is put down (synchronized with the original object), update the transform of the new object
        if (track) {
            this.transform.position = original.transform.position + positionOffset;
            this.transform.eulerAngles = original.transform.eulerAngles + rotationOffset;
            
        } else {
            // check each object collide with others or not
            if (collids.Count != 0) {
                this.GetComponent<MeshRenderer>().materials = unables;
                canPutOrNot = false;
            }
            else {
                this.GetComponent<MeshRenderer>().materials = originalMaterial;
                canPutOrNot = true;
            }
        }
	}

    /// <summary>
    /// This method will change the original object.
    /// </summary>
    /// <param name="org">Original object</param>
    public void setOriginal(GameObject org) {
        original = org;
    }

    /// <summary>
    /// This method is called when player put down the holding object and place it in to the scene. These two objects will start behave the same
    /// </summary>
    public void startTracking() {
        positionOffset = this.transform.position - original.transform.position;
        rotationOffset = this.transform.eulerAngles - original.transform.eulerAngles;
        track = true;
        foreach(Transform child in transform) {
            if (child.gameObject.GetComponent<CuttedObject>()!= null) {
                child.gameObject.GetComponent<CuttedObject>().startTracking();
            }

        }
    }

    /// <summary>
    /// This method is called when player pick the object and the synchronize of these two object will stop
    /// </summary>
    public void stopTracking() {
        track = false;
    }

    /// <summary>
    /// If the object collide with other object
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (track == false && other.gameObject.GetComponent<Rigidbody>() != null &&
            other.gameObject.GetComponent<Rigidbody>().isKinematic == false) {
            GameObject connected = other.gameObject;
            collids.Add(connected);
        }
        
    }

    /// <summary>
    /// if the gem stop colliding with a object objects
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        if (track == false) {
            GameObject connected = other.gameObject;
            if (collids.Contains(connected)) {
                collids.Remove(connected);
            }
        }
    }

    /// <summary>
    /// This method is called to check if a object can be put. It will check all its children
    /// </summary>
    /// <returns>True if it no collision occurred</returns>
    public bool putAllowed() {
        if (canPutOrNot == false) return false;
        foreach (Transform child in transform) {
            if (child.gameObject.GetComponent<CuttedObject>() != null) {
                if (!child.gameObject.GetComponent<CuttedObject>().putAllowed()) {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// This method is called when user is placing the object. So the object's physics will not 
    /// affect the game play. ONLY called once for the parent object
    /// </summary>
    public void istrigger() {
        this.GetComponent<MeshCollider>().isTrigger = true;
        foreach (Transform child in transform) {
            if (child.gameObject.GetComponent<CuttedObject>() != null) {
                child.gameObject.GetComponent<CuttedObject>().istrigger();
            }

        }
    }

    /// <summary>
    /// This method is called when user done placing the object and enable its physics
    /// ONLY called once for the parent object
    /// </summary>
    public void notIstrigger() {
        this.GetComponent<MeshCollider>().isTrigger = false;
        foreach (Transform child in transform) {
            if (child.gameObject.GetComponent<CuttedObject>() != null) {
                child.gameObject.GetComponent<CuttedObject>().notIstrigger();
            }

        }
    }

    


}
