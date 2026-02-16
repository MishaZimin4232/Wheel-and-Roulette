using UnityEngine;

public class Narrator : MonoBehaviour
{
    public static Narrator Instance { get; private set; }
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
    public void Talk(string message)
    {
        Debug.Log($"<color=green>[Narrator]: {message}</color>");
    }
    public void Task(string message)
    {
        Debug.Log($"<color=yellow>[Question]: {message}</color>");
    }
}
