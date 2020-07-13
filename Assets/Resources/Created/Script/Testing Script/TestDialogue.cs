using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    public GameObject dialogueT;
    public DialogueParser dialogueP = new DialogueParser();
    // Start is called before the first frame update
    void Start()
    {
        dialogueP.Parse("Assets/Resources/Created/StoryFiles/Chapter1.txt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //    if (collision.gameObject.tag == "Player")
        //    {
        //Debug.Break();
        //dialogueT.GetComponent<DialogueTrigger>().dialogue = dp.GetComponent<DialogueParser>().Next();
        Dialogue d = dialogueP.Next();
        dialogueT.GetComponent<DialogueTrigger>().TriggerDialogue(d);

        //    }
    }
}

