using UnityEngine;

public class ShootSelf : MonoBehaviour,Cell
{
    [SerializeField] private Game gameref;

    public void Action()
    {
        gameref.current_player.ShootYourself();
    }
}
