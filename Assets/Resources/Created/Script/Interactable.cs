using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    int active;

    public int getActive(){
        return active;
    }

    public void setActive(int num){
        active = num;
    }
    abstract public void reset();
    
}
