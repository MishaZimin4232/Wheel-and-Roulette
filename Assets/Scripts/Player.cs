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
    public Action OnCharChosen;      // Событие выбора буквы
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
        Time.timeScale = 1f;
        charinput = player_char.text[0];
        OnCharChosen?.Invoke();
        LetterPanel.SetActive(false);
    }

    public void WordGet()
    {
        Time.timeScale = 1f;
        wordinput = player_string.text;
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
    private void ClearInputFields()
    {
        if (player_char != null)
            player_char.text = "";
            
        if (player_string != null)
            player_string.text = "";
    }
    public void PlayerInput()
    {
        ClearInputFields();
        ChoisePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowCharInput()
    {
        
        ChoisePanel.SetActive(false);
        LetterPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ShowWordInput()
    {
        ChoisePanel.SetActive(false);
        WordPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    private void OnDestroy()
    {
        // Очищаем все подписки при уничтожении игрока
        OnCharChosen = null;
        OnWordChosen = null;
        
    }


}