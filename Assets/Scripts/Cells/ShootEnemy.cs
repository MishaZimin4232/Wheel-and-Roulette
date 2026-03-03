using UnityEngine;

public class ShootEnemy : Cell
{
   
   public override void Action(IGameMember current_player,IGameMember target_player,Board board)
   {
      Narrator.Instance.Talk("ShootEnemy");
      current_player.Round();
      
      current_player.ShootEnemy(target_player);
   }
}
