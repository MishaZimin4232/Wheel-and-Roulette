using UnityEngine;

public class ShootSelf :  Cell
{
    
    
    public override void Action(IGameMember current_player,IGameMember target_player,Board board)
    {
        Narrator.Instance.Talk("SHOOT!");
        if (current_player.ShootYourself())
        {
            Narrator.Instance.Talk("First blood!");
           
        }
        else
        {
            Narrator.Instance.Talk("Nah..bastard..");
        }
    }
}
