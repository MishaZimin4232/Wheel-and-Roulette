using UnityEngine;

public class EnemySelf : Cell
{
    public override void Action(IGameMember current_player,IGameMember target_player,Board board)
    {
        Narrator.Instance.Talk("Выпала EnemySelf");
        if (target_player.ShootYourself())
        {
            Narrator.Instance.Talk("Rampage!");
           
        }
        else
        {
            Narrator.Instance.Talk("Baka!");
        }
    }
}
