public interface IGameMember
{
    int current_Bcell { get; set; }
    int score { get; set; }
    bool[] BulletCells { get; set; }
    bool IsAlive { get; set; }
    void AddBullet(int count);
    bool ShootYourself();
    bool ShootEnemy(IGameMember enemy);
    void Round();
    char CharInput();
    void TakeOut();
    void AddScore(int count);
    void Die();


}
