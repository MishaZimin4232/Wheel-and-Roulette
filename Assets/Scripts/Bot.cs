using UnityEngine;

public class Bot : MonoBehaviour,IGameMember
{
    public bool[] BulletCells { get; set; } = new bool[6];
    public int current_Bcell { get; set; }
    public int score { get; set; }
    private char charinput;
    public bool IsAlive { get; set; }=true;
    private string ans;

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
                current_count++;
            }
        }

        if (current_count == 0)
        {
            Narrator.Instance.Talk("You have full pack!");
        }
    }

    public bool ShootYourself()
    {
        if (BulletCells[current_Bcell])
        {
            Die();
            return true;
        }
        else
        {
            
            return false;
        }
    }

    public bool ShootEnemy(IGameMember enemy)
    {
        if (BulletCells[current_Bcell] == true)
        {
            BulletCells[current_Bcell] = false;
            enemy.Die();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Round()
    {
        current_Bcell = Random.Range(0, 6);
    }
   
    public void TakeOut()
    {
        for (int i = 0; i < BulletCells.Length; i++)
        {
            if (BulletCells[i] == true)
            {
                BulletCells[i] = false;
                break;

            }
        }
    }

    public char CharInput()
    {
        char var1 = ans[Random.Range(0, ans.Length)];
        char var2 = (char)Random.Range(97, 123);;
        char input=Random.Range(0,100)<=80?var2:var1;
        return input;
    }
}
