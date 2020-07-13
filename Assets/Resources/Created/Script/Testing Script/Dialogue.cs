using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    public string name;
    
    // Lets you use text area boxes to put in dialogue
    [TextArea(3, 10)]
    public List<string> sentences = new List<string>();

    public List<string> answers = new List<string>(); // Match the index up with the sentences/questions

}
