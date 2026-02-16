using UnityEngine;

public class Bot : MonoBehaviour,IGameMember
{
    public bool[] BulletCells = new bool[6];
    private int current_Bcell=0;
    public int count;
    public int score = 0;
    public char charinput;
    public void AddScore(int _score)
    {
        this.score += _score;
    }
    
    public void AddBullet(int count)
    {
        int current_count = 0;
        for (int i = 0; i < BulletCells.Length; i++)
        {
            if (current_count < count)
            {
                if (BulletCells[i] == false)
                {
                    BulletCells[i] = true;
                    current_count++;
                }
            }
            else
            {
                break;
            }
        }

    }

    public bool ShootYourself()
    {
        if (BulletCells[current_Bcell] == true)
        {
            BulletCells[current_Bcell] = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ShootEnemy()
    {
        if (BulletCells[current_Bcell] == true)
        {
            BulletCells[current_Bcell] = false;
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
        return charinput;
    }

    
    
}
