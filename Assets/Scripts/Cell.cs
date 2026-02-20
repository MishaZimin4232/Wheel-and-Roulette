using UnityEngine;

public abstract class Cell:MonoBehaviour
{
   public abstract void Action(IGameMember current_player,IGameMember target_player,Board board);

}
