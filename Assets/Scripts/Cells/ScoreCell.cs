using UnityEngine;

public class ScoreCell : MonoBehaviour, Cell
{
    [SerializeField] private int Score;
    public Game gameref;
    public void Action(IGameMember player, IGameMember enemy, Board board)
    {
        gameref.current_player.AddScore(Score);
        Narrator.Instance.Talk($"Player take {Score} scores!");
    }

}
