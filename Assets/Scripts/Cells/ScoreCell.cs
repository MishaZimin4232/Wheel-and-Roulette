using UnityEngine;

public class ScoreCell : MonoBehaviour,Cell
{
    [SerializeField]private int Score;
    public Game gameref;
    public void Action()
    {
        gameref.current_player.AddScore(Score);
    }
    
}
