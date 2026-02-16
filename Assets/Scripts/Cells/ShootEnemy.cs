using UnityEngine;

public class ShootEnemy : MonoBehaviour,Cell
{
   [SerializeField] private Game gameref;
   public void Action()
   {
      gameref.current_player.ShootEnemy();
   }
}
