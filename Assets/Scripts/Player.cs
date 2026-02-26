using TMPro;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Player : MonoBehaviour, IGameMember
{
    public string wordinput;
    public char charinput;
    public int score { get; set; }= 0;
    public bool IsAlive { get; set; } = true;
    public bool[] BulletCells { get; set; } = new bool[6];
    public int current_Bcell{ get; set; } = 0;
    public TextMeshProUGUI score_text;
    public TextMeshProUGUI player_char;
    public TextMeshProUGUI player_string;

    public GameObject ChoisePanel;
    public GameObject LetterPanel;
    public GameObject WordPanel;
    public Bullet[] bullet_images=new Bullet[6];
    public Action OnCharChosen;      
    public Action OnWordChosen; 
    

    void Start()
    {
        score_text.text = "Score - "+this.score.ToString();
        ChoisePanel.SetActive(false);
        LetterPanel.SetActive(false);
        WordPanel.SetActive(false);
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
                bullet_images[i].ChangeSprite();
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
        Round();
        if (BulletCells[current_Bcell])
        {

            Die();
            bullet_images[current_Bcell].ChangeSprite();
            return true;
        }
        else
        {
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
        current_Bcell = UnityEngine.Random.Range(0, 6);
    }

    public void TakeOut()
    {
        for (int i = 0; i < BulletCells.Length; i++)
        {
            if (BulletCells[i] == true)
            {
                BulletCells[i] = false;
                bullet_images[i].ChangeSprite();
                break;

            }
        }

    }

    public void CharGet()
    {
        
        charinput = player_char.text[0];
        OnCharChosen?.Invoke();
        LetterPanel.SetActive(false);
    }

    public void WordGet()
    {
        // Удаляем символ 8203 (Zero Width Space)
        wordinput = player_string.text.Replace("\u200B", "").Trim();
    
        Debug.Log($"Input after cleaning: '{wordinput}'");
        Debug.Log($"Cleaned length: {wordinput.Length}");
    
        OnWordChosen?.Invoke();
        WordPanel.SetActive(false);
    }

    public char CharInput()
    {
        return charinput;
    }

    public string WordInput()
    {
        return wordinput;
    }
    
    public void PlayerInput()
    {
        
        ChoisePanel.SetActive(true);
        
    }

    public void ShowCharInput()
    {
        
        ChoisePanel.SetActive(false);
        LetterPanel.SetActive(true);
        
    }
    public void ShowWordInput()
    {
        ChoisePanel.SetActive(false);
        WordPanel.SetActive(true);
        
    }
    private void OnDestroy()
    {
        
        OnCharChosen = null;
        OnWordChosen = null;
        
    }


}