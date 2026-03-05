using Unity.VisualScripting;
using UnityEngine;

public class TakeOut :  Cell
{
    

    public override void Action(IGameMember current_player,IGameMember target_player,Board board)
    {
       
        if (current_player.BulletCells[current_player.current_Bcell])
        {
            current_player.TakeOut();
            StartCoroutine(Narrator.Instance.Talk("Lucky kid!"));
        }
        else
        {
            StartCoroutine(Narrator.Instance.Talk("you don't have bullets, asshole!"));
        }
        
    }

}
