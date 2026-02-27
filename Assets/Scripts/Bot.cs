using UnityEngine;

public class Bot : MonoBehaviour,IGameMember
{
    public bool[] BulletCells { get; set; } = new bool[6];
    public int current_Bcell { get; set; }
    public int score { get; set; }
    private char charinput;
    public bool IsAlive { get; set; }=true;
    private string ans;
    public Bullet[] bullet_images=new Bullet[6];
    public void AddScore(int _score)
    {
        this.score += _score;
    }

    public void Die()
    {
        IsAlive = false;
        Debug.Log(gameObject.name + " is dead");
        Destroy(gameObject);
    }

    public void GetAnswer(string answer)
    {
        ans = answer;
    }

    public void AddBullet(int count)
    {
        int current_count = 0;
        for (int i = 0; i < BulletCells.Length && current_count < count; i++)
        {
            if (!BulletCells[i])
            {
                BulletCells[i] = true;
                bullet_images[i].ChangeSprite();
                current_count++;
            }
        }

        if (current_count == 0)
        {
            Narrator.Instance.Talk("У вас полный бак!");
        }
    }

    public bool ShootYourself()
    {
        Round();
        if (BulletCells[current_Bcell])
        {
            Die();
            SoundManager.Instance.Play("Shoot");
            bullet_images[current_Bcell].ChangeSprite();
            return true;
        }
        else
        {
            SoundManager.Instance.Play("ShootFail");
            return false;
        }
    }

    public bool ShootEnemy(IGameMember enemy)
    {
        Round();
        if (BulletCells[current_Bcell] == true)
        {
            BulletCells[current_Bcell] = false;
            bullet_images[current_Bcell].ChangeSprite();
            SoundManager.Instance.Play("Shoot");
            enemy.Die();
            return true;
        }
        else
        {
            SoundManager.Instance.Play("ShootFail");
            return false;
        }
    }
    public void Round()
    {
        SoundManager.Instance.Play("Round");
        current_Bcell = Random.Range(0, 6);
    }
   
    public void TakeOut()
    {
        for (int i = 0; i < BulletCells.Length; i++)
        {
            if (BulletCells[i] == true)
            {
                SoundManager.Instance.Play("BulletOut");
                bullet_images[i].ChangeSprite();
                BulletCells[i] = false;
                break;

            }
        }
    }

    public char CharInput()
    {
        char var1 = ans[Random.Range(0, ans.Length)];
        char var2 = (char)Random.Range(97, 123);
        char input=Random.Range(0,100)<=50?var2:var1;
        return input;
    }
}
