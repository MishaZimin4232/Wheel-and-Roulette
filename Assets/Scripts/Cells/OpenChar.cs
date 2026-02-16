using UnityEngine;

public class OpenChar : MonoBehaviour, Cell
{
    public void Action(IGameMember executor, IGameMember opponent, Board board)
    {

        Narrator.Instance.Talk("Открывается случайная буква!");
    }
}
