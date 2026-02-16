using System;
using UnityEngine;

[Serializable]public class Word
{
    public string answer;
    public string question;
    public Word(string answer, string question)
    {
        this.answer = answer;
        this.question = question;
    }
}
