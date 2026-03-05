using UnityEngine;

public class ShootSelf :  Cell
{
    
    
    public override void Action(IGameMember current_player,IGameMember target_player,Board board)
    {
        
        
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
