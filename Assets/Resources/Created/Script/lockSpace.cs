using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is used to control the cutting behavior of gem
/// </para>
/// Updated: 3/16/2019: add a array that contains all objects that are going to cut<para/>
/// Author: Yan Xiao, Yuzhe Gu<para/>
/// Attached object: Gems<para/>
/// updated: 3/16 can cut multiple objects now<para/>
/// </summary>
public class lockSpace : MonoBehaviour {

    public bool hasPut;
    public GameObject connected;
    public List<GameObject> collids; // array of objects we need to 
    public Material picked_material;
    public Material unpick_material;
    public GameObject player;
    private holdCuttedObject hold_script;
    //public Material slice_meterial


    // Use this for initialization
    void Start () {
        GameObject[] obj = (GameObject[])Object.FindObjectsOfType(typeof(GameObject));
        for (int i = 0; i < obj.Length; i++){
            if(obj[i].name == "Player"){
                player = obj[i];
            }
        }
        hasPut = false;
        //ppu = player.GetComponent<pickUpObject>();
        this.GetComponent<Renderer>().material = picked_material;
        hold_script = player.GetComponent<holdCuttedObject>();

    }
	
	// Update is called once per frame
	void Update () {
    }

    /// <summary>
    /// If the gem collide with other object
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!hasPut && other.gameObject.GetComponent<Identifier>() != null && 
            (other.gameObject.GetComponent<Identifier>().isGroup("cutable") || identify(other.gameObject).isGroup("cutted") ))
        {
            connected = other.gameObject;
            if(!collids.Contains(connected)){
                collids.Add(connected);
            }
            
        }        
    }

    /// <summary>
    /// if the gem stop colliding with a object objects
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!hasPut){
            connected = other.gameObject;
            if(collids.Contains(connected)){
            collids.Remove(connected);
            }
            
        }
    }


    /// <summary>
    /// This method is called when the user unpick the object
    /// returns 0 if it cut a object, return 1 if a object is deleted, return 0 if nothing happeded 
    /// </summary>
    /// <returns>0 if it cut a object, 1 if a object is deleted, 0 if nothing happeded </returns>
    public int Unpick() {

        // first modify the gem
        this.GetComponent<Renderer>().material = unpick_material;
        hasPut = true;

        // finds all the parents in the objects that is colliding with the gem
        List<GameObject> parents = new List<GameObject>();
        if (collids.Count == 0){
            return 0;
        }

        // find all parents and put them into the parents variable
        foreach (GameObject connected in collids) {
            if (connected.transform.parent == null || !collids.Contains(connected.transform.parent.gameObject)){ 
                if (!parents.Contains(connected)) parents.Add(connected);
            }
            else if (parents.Contains(connected) && collids.Contains(connected.transform.parent.gameObject)){
                parents.Remove(connected);
                if(!parents.Contains(connected.transform.parent.gameObject))
                    parents.Add(connected.transform.parent.gameObject);
            } 
            else if (collids.Contains(connected.transform.parent.gameObject)) {
                if(!parents.Contains(connected.transform.parent.gameObject))
                    parents.Add(connected.transform.parent.gameObject);
            }
        }

        GameObject finding_parent = parents[0]; // to find the object we are interested in / do operation on
        int cutable_flag = 0; // to check if this operation is a cut or delete operation

        foreach (GameObject parent in parents) {
            if (identify(parent).isGroup("cutable")) {
                finding_parent = parent;
                cutable_flag = 1;
                break;
            }
        }
        // print(finding_parent);
        // print(cutable_flag);

        if (cutable_flag == 1) {
            // select the first parent to cut
            GameObject theparent = cutit(finding_parent);

            Queue<GameObject> waittobeCut = new Queue<GameObject>();
            Queue<GameObject> originalObjects = new Queue<GameObject>();
            originalObjects.Enqueue(finding_parent);
            waittobeCut.Enqueue(theparent);
            
            // cut all its children
            while (originalObjects.Count != 0) {
                GameObject currentParent = originalObjects.Dequeue();
                // print(currentParent);
                foreach (Transform childs in currentParent.transform) {
                    if (collids.Contains(childs.gameObject)) {
                        GameObject resultChilds = cutit(childs.gameObject);
                        waittobeCut.Enqueue(resultChilds);
                        originalObjects.Enqueue(childs.gameObject);
                        resultChilds.transform.SetParent(waittobeCut.Peek().transform);
                    }
                }
                waittobeCut.Dequeue();
            }

            // attach the rigid body component to it
            theparent.AddComponent<Rigidbody>();
            theparent.GetComponent<Rigidbody>().useGravity = false;
            theparent.GetComponent<Rigidbody>().isKinematic = true;
            hold_script.putCuttedItemToHand(theparent);
            this.transform.parent = parents[0].transform;
            return 0;

        }
        else {
            while (finding_parent.transform.parent != null && identify(finding_parent.transform.parent.gameObject) != null && identify(finding_parent.transform.parent.gameObject).isGroup("cutted"))
            {
                finding_parent = finding_parent.transform.parent.gameObject;
            }
            Destroy(finding_parent);
            hold_script.DestoriedDuplicatedObjectFromScene();
            return 1;
        }
    }


    /// <summary>
    /// This is a private helper method that cut the object by the all faces of the cube. This will
    /// return the final object inside the cube
    /// </summary>
    /// <param name="connected">the object you want to cut</param>
    /// <returns>the final remaining object</returns>
    private GameObject cutit(GameObject connected){
        if (connected != null) {
            GameObject[] temp;
            string name = connected.name;
            GameObject newObject = Instantiate(connected);
            
            Vector3 pos = connected.transform.position;
            newObject.transform.position = pos;
            newObject.transform.rotation = connected.transform.rotation;
            newObject.transform.localScale = connected.transform.lossyScale;
            
            Mesh abc = newObject.GetComponent<MeshFilter>().mesh;
            //print(abc.uv);

            //cutting process
            temp = BLINDED_AM_ME.MeshCut.Cut(newObject, get_ankerpoint()[0], get_normalDirection()[0], picked_material);
            Destroy(temp[0]);
            temp = BLINDED_AM_ME.MeshCut.Cut(temp[1], get_ankerpoint()[0], get_normalDirection()[1], picked_material);
            Destroy(temp[0]);
            temp = BLINDED_AM_ME.MeshCut.Cut(temp[1], get_ankerpoint()[0], get_normalDirection()[2], picked_material);
            Destroy(temp[0]);
            temp = BLINDED_AM_ME.MeshCut.Cut(temp[1], get_ankerpoint()[1], get_normalDirection()[3], picked_material);
            Destroy(temp[0]);
            temp = BLINDED_AM_ME.MeshCut.Cut(temp[1], get_ankerpoint()[1], get_normalDirection()[4], picked_material);
            Destroy(temp[0]);
            temp = BLINDED_AM_ME.MeshCut.Cut(temp[1], get_ankerpoint()[1], get_normalDirection()[5], picked_material);
            Destroy(temp[0]);
            GameObject resultObject = temp[1];


            //add attributes to the cutted objects
            resultObject.AddComponent<Identifier>();
            resultObject.GetComponent<Identifier>().setName(name + "Copied");
            resultObject.GetComponent<Identifier>().copyIdentifier(connected.GetComponent<Identifier>());
            resultObject.GetComponent<Identifier>().deleteGroup("cutable");
            resultObject.GetComponent<Identifier>().addGroup("cutted"); //cutted object

            resultObject.AddComponent<CuttedObject>();
            resultObject.GetComponent<CuttedObject>().setOriginal(connected);

            resultObject.AddComponent<MeshCollider>();
            resultObject.GetComponent<MeshCollider>().convex = true;
            
            return resultObject;
        }
        return null;
    }


    /// <summary>
    /// This method is a private helper method when it cut the mesh
    /// It will return two points in space which is the upper, right, back and lower, left front.<para/>
    /// For each point, it will cut 3 times for each face connected to it
    /// </summary>
    /// <returns></returns>
    public Vector3[] get_ankerpoint() {
        Vector3[] twoPoints = new Vector3[2];
        twoPoints[0]= transform.position + (transform.forward + transform.up + transform.right)* transform.localScale.x/2;
        twoPoints[1] = transform.position - (transform.forward + transform.up + transform.right) * transform.localScale.x / 2; ;
        return twoPoints;
    }


    /// <summary>
    /// This method is a private helper method when it cut the mesh<para/>
    /// it will return the normals of the faces to cut. The first three element relates to the first anker point and the 
    /// last three elements are related to the second point.<para/>
    /// They are actually the normals of the 6 faces of the cube
    /// </summary>
    /// <returns></returns>
    public Vector3[] get_normalDirection() {
        Vector3[] normals = new Vector3[6];
        normals[0] = -transform.forward;
        normals[1] = -transform.up;
        normals[2] = -transform.right;
        normals[3] = transform.forward;
        normals[4] = transform.up;
        normals[5] = transform.right;
        return normals;
    }
}
