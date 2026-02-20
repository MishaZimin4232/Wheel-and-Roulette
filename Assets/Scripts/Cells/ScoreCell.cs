using UnityEngine;

public class ScoreCell :  Cell
{
    [SerializeField] private int Score;
    
    public override void Action(IGameMember current_player,IGameMember target_player,Board board)
    {
        current_player.AddScore(Score);
        Narrator.Instance.Talk($"Player take {Score} scores!");
    }

}
