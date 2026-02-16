public interface IGameMember
{
    int Score { get; }
    int CurrentBullets { get; }
    bool IsAlive { get; }
    void AddBullet(int count);
    bool ShootYourself();
    bool ShootEnemy(IGameMember enemy);
    void Round();

    void TakeOut();
    void AddScore(int count);
    void Die();


}
