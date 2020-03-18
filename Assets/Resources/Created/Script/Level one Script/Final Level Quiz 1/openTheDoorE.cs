using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class openTheDoorE : MonoBehaviour
{
    public GameObject player;
    public KeyCode left;
    public KeyCode right;
    UnityEvent open;
    public TMPro.TextMeshProUGUI text;
    public GameObject collid;
    bool goLeft;
    bool goRight;
    // Start is called before the first frame update
    void Start()
    {
        if(open == null){
            open = new UnityEvent();
            
        }
        open.AddListener(push);
    }

    // Update is called once per frame
    void Update()
    {   
        
        if(Vector3.Distance(player.transform.position, this.transform.position) <= 22){
            // text.text = "123123";
            open.Invoke();
        }
    }

    void push(){
        if(Input.GetKeyDown(left)){
            goLeft = true;
        }
        if(Input.GetKeyUp(left)){
            goLeft = false;
        }
        if(Input.GetKeyDown(right)){
            goRight = true;
        }
        if(Input.GetKeyUp(right)){
            goRight = false;
        }
        if(goLeft){
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -1);
        }
        else if(goRight){
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 1);
        }
        else{
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}

