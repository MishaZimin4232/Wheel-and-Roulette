using UnityEngine;

public interface IGameMember
{
    void AddBullet(int count);
    bool ShootYourself();
    bool ShootEnemy();
    void Round();
    
    void TakeOut();
    void AddScore(int count);


}
