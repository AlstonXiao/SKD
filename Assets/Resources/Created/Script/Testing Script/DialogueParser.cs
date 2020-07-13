using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/**
 * Parse story scripts in txt files to Dialogues 
 */
public class DialogueParser 
{
    private static List<Dialogue> Dialogues = new List<Dialogue>(); // all Dialogues in the given file 
    private static int index = 0; // index of the next Dialogue to return


    public void Parse(string FilePath)
    {
        try
        {
            StreamReader sr = new StreamReader(FilePath);
            Dialogue d = ParseByPart(sr); 
            while (d != null)
            {
                Dialogues.Add(d);
                d = ParseByPart(sr);
            }
            sr.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message); 
        }
    }

    private Dialogue ParseByPart(StreamReader sr)
    {
        Dialogue d = new Dialogue();
        d.name = sr.ReadLine();
        if (d.name == null)
            return null;
        string line = sr.ReadLine();
        //int i = 0;
        while (line != "END")
        {
            d.sentences.Add(line);
            line = sr.ReadLine();
        //  i++;
        }
        return d;
    }

    public Dialogue Next()
    {
        Dialogue d = Dialogues[index];
        index++;
        return d;
    }

    public bool HasNext()
    {
        return Dialogues.Count > index;
    }
}
