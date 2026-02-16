using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "QuestionBank", menuName = "Scriptable Objects/QuestionBank")]
public class QuestionBank : ScriptableObject
{
    public List<Word> qb = new List<Word>();
    

}
