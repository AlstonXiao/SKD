﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void onTriggerEnter(Collider other){ 
        text.text = "123123";
    }
    void onTriggerExit(Collider other){
        text.text = "";
    }
}

