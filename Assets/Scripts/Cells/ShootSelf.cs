using UnityEngine;

public class ShootSelf :  Cell
{
    
    
    public override void Action(IGameMember current_player,IGameMember target_player,Board board)
    {
        StartCoroutine(Narrator.Instance.Talk("ShootSelf"));
        current_player.Round();
        if (current_player.ShootYourself())
        {
            StartCoroutine(Narrator.Instance.Talk("First blood!"));
           
        }
        else
        {
            StartCoroutine(Narrator.Instance.Talk("Nah..bastard.."));
            
        }
    }
}
