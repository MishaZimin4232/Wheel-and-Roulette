using UnityEngine;

public class ShootEnemy : Cell
{
   
   public override void Action(IGameMember current_player,IGameMember target_player,Board board)
   {
      current_player.ShootEnemy(target_player);
   }
}
