using UnityEngine;

public class OpenChar : Cell
{
    
    public override void Action(IGameMember current_player,IGameMember target_player,Board board)
    {
        board.RandomOpenChar();
        StartCoroutine(Narrator.Instance.Talk("Opened new letter!"));
    }
}
