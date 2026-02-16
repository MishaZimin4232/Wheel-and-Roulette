using UnityEngine;

public class TakeOut : MonoBehaviour,Cell
{
    [SerializeField]private Game gameref;

    public void Action()
    {
        gameref.current_player.TakeOut();
    }
    
}
