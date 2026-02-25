using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    private string word;
    public Letter[] charPrefab =  new Letter[6];
    [SerializeField]private TMP_Text question_text;

    

    public void OpenChar(char ch)
    {
        for (int i=0; i < word.Length; i++)
        {
            if (ch == word[i])
            {
                charPrefab[i].ShowLetter();
            }
            
        }
    }

    public bool IsOpen()
    {
        foreach (Letter letter in charPrefab)
            {
                if (!letter.IsOpen)
                {
                    return false;
                }
            }
        return true;
    }
    
    private List<char> GetUnopenedLetters()
    {
        List<char> unopened = new List<char>();
        
        for (int i = 0; i < word.Length; i++)
        {
            if (charPrefab[i] != null && !charPrefab[i].IsOpen)
            {
                unopened.Add(word[i]);
            }
        }
        
        return unopened;
    }
    public void RandomOpenChar()
    {
        List<char> unopened = GetUnopenedLetters();
        
        if (unopened.Count == 0)
        {
            Debug.Log("Все буквы уже открыты!");
            return;
        }
        
        char randomLetter = unopened[Random.Range(0, unopened.Count)];
        Debug.Log($"Открываем случайную неоткрытую букву: '{randomLetter}'");
        OpenChar(randomLetter);
    }

    public void OpenString()
    {
        for (int i=0; i < word.Length; i++)
        {
            charPrefab[i].ShowLetter();
        }
    }

    public void SetWord(string  _word)
    {
        word = _word;
        for (int i = 0; i < charPrefab.Length; i++)
        {
            if (i < word.Length)
            {
               
                charPrefab[i].DrawLetter(word[i]);
                charPrefab[i].gameObject.SetActive(true); 
            }
            else
            {
                charPrefab[i].gameObject.SetActive(false);
            }
        }
    }
    public bool IsCharOpen(char ch)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (charPrefab[i] != null && charPrefab[i].IsOpen && charPrefab[i].GetLetter()==ch.ToString().ToUpper())
            {
                return true;
            }    
            
        }
        return false;
    }

    public void EnterQuestion(string question)
    {
        question_text.text = question;
    }
}
