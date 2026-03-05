using TMPro;
using UnityEngine;

public class Letter : MonoBehaviour
{
    [SerializeField]private TMP_Text letter;
    public bool IsOpen=false;

    public void DrawLetter(char c)
    {
        letter.text = c.ToString().ToUpper();
        
    }
    public void ShowLetter()
    {
        letter.enabled = true;
        IsOpen = true;
    }

    public string GetLetter()
    {
        return letter.text;
    }




}
