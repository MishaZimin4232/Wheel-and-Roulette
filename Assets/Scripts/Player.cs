using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IGameMember
{
    public string wordinput;
    public char charinput;
    public int score { get; set; }= 0;
    public bool IsAlive { get; set; } = true;
    public bool[] BulletCells { get; set; } = new bool[6];
    public int current_Bcell{ get; set; } = 0;
    public TextMeshProUGUI score_text;

    void Start()
    {
        score_text.text = "Score - "+this.score.ToString();
    }

    public void Die()
    {
        IsAlive = false;
        Debug.Log(gameObject.name + " is dead");
        Destroy(gameObject);
    }
    public void AddScore(int _score)
    {
        this.score += _score;
        score_text.text = "Score - "+this.score.ToString();
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
        //поле ввода для символа
        char input = 'c';
        return input;
    }

    public string WordInput()
    {
        //поле ввода для строки
        wordinput = "family";
        return wordinput;
    }

    public void PlayerInput()
    {
        //вызывает меню выбора - символ или слово
    }

  

}
