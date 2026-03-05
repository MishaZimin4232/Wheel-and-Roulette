using UnityEngine;

public class ShootEnemy : Cell
{
   
   public override void Action(IGameMember current_player,IGameMember target_player,Board board)
   {
     
      current_player.Round();
      if (current_player.ShootEnemy(target_player))
      {
         StartCoroutine(Narrator.Instance.Talk("good..."));
      }
      else
      {
         StartCoroutine(Narrator.Instance.Talk("unluck :("));
      }

   }
}
