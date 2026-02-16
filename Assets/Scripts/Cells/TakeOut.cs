using UnityEngine;

public class TakeOut : MonoBehaviour, Cell
{
    [SerializeField] private Game gameref;

    public void Action(IGameMember executor, IGameMember opponent, Board board)
    {
        if (executor.CurrentBullets > 0)
        {
            executor.TakeOut();
            Narrator.Instance.Talk("Lucky kid!");
        }
        else
        {
            Narrator.Instance.Talk("Narrator laughting: you don't have bullets, asshole!");
        }
    }

}
