using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //public Dialogue dialogue;
    public DialogueManager dialogueM;
    public GameObject dialogueBox;

    public void TriggerDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);
        dialogueM.GetComponent<DialogueManager>().startDialogue(dialogue);
    }
}
