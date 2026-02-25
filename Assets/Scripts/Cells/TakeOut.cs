using UnityEngine;

public class TakeOut :  Cell
{
    

    public override void Action(IGameMember current_player,IGameMember target_player,Board board)
    {
        Narrator.Instance.Talk("TakeOut");
        if (current_player.BulletCells[current_player.current_Bcell])
        {
            current_player.TakeOut();
            Narrator.Instance.Talk("Lucky kid!");
        }
        else
        {
            Narrator.Instance.Talk("Narrator laughting: you don't have bullets, asshole!");
        }
        
    }

}
