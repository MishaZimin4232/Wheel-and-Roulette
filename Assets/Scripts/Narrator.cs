using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Narrator : MonoBehaviour
{
    public static Narrator Instance { get; private set; }
    public GameObject NarratorSpeech;
    public TMP_Text NarratorText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator Talk(string message)
    {
        
        NarratorSpeech.SetActive(true);
        NarratorText.text = "Narrator: "+message;
        yield return new WaitForSeconds(2);
        NarratorSpeech.SetActive(false);
    }
    public void Task(string message)
    {
        Debug.Log($"<color=yellow>[Question]: {message}</color>");
    }
}
