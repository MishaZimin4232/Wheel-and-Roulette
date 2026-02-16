using UnityEngine;

public class ShootSelf : MonoBehaviour, Cell
{
    [SerializeField] private Game gameref;

    public void Action(IGameMember player, IGameMember opponent, Board board)
    {
        Narrator.Instance.Talk("SHOOT!");
        if (player.ShootYourself())
        {
            Narrator.Instance.Talk("First blood!");
            player.Die();
        }
        else
        {
            Narrator.Instance.Talk("Nah..bastard..");
        }
    }
}
