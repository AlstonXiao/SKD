using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    //public TextMeshProUGUI dialogueAnswers;
    public GameObject choice1;
    public GameObject choice2;
    public GameObject choice3;
    public GameObject choice4;

    public GameObject dialogueBox;
    private Queue<string> sentences;
    private Queue<string> answers;
    private List<string> displayedSentences;

    void Start()
    {
        sentences = new Queue<string>();
        answers = new Queue<string>();
        displayedSentences = new List<string>();
    }

    public void startDialogue(Dialogue dialogue)
    {
        //dialogueAnswers.text = "";
        //nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        answers.Clear();

        foreach (string answer in dialogue.answers)
        {
            answers.Enqueue(answer);
        }

        displayedSentences.Clear();
        displayedSentences.Add("");
        displayedSentences.Add("");
        displayedSentences.Add("");
        DisplayNext();
    }

    public void DisplayNext()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        // StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        //dialogueText.text = "";
        //foreach (char letter in sentence.ToCharArray())
        //{
        //    dialogueText.text += letter;
        //    yield return null;
        //}
        char[] letters = sentence.ToCharArray();
        Boolean isQuestion = false;
        for (int i = 0; i < letters.Length; i++)
        {
            displayedSentences[displayedSentences.Count-1] += letters[i];
            if (i == letters.Length - 1)
            {
                //dialogueText.text += "\n";
                setText();
                if (letters[i] == '?')
                {
                    isQuestion = true;
                    Debug.Log("Detected question");
                }
                else yield return new WaitForSeconds(3);
            } else
            {
                setText();
                yield return null;
            }
        }
        displayedSentences.Add("");
        if (displayedSentences.Count > 3)
        {
            displayedSentences.RemoveAt(0);
        }
        if (!isQuestion)
        {
            DisplayNext();
        } else
        {
            DisplayAnswers();
        }
    }

    public void setText()
    {
        dialogueText.text = "";
        foreach (string s in displayedSentences)
        {
            dialogueText.text += s;
            dialogueText.text += "\n";
        }
    }

    public void DisplayAnswers()
    {
        if (answers.Count == 0)
        {
            Debug.Log("Error: no more answers found");
            return;
        }

        string answer = answers.Dequeue();
        string[] choices = answer.Split(',');
        int number = 1;
        foreach (string choice in choices)
        {
            // This code can be compacted into a list
            if (number == 1)
            {
                choice1.SetActive(true);
                choice1.GetComponentInChildren<Text>().text = choice;
            } 
            else if (number == 2)
            {
                choice2.SetActive(true);
                choice2.GetComponentInChildren<Text>().text = choice;
            }
            else if (number == 3)
            {
                choice3.SetActive(true);
                choice3.GetComponentInChildren<Text>().text = choice;
            }
            else if (number == 4)
            {
                choice4.SetActive(true);
                choice4.GetComponentInChildren<Text>().text = choice;
            }
            number++;
        }
        //dialogueAnswers.text = answerText;
    }

    public void chooseAnswer(Text answer)
    {
        Debug.Log("Answer: " + answer.text);
        choice1.SetActive(false);
        choice2.SetActive(false);
        choice3.SetActive(false);
        choice4.SetActive(false);
        DisplayNext();
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
    }
}
