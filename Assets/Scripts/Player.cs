using TMPro;
using UnityEngine;
using System;
using System.Collections;
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
    public TMP_InputField player_char;
    public TMP_InputField player_string;
    public Transform revolver;
    private bool IsRotated = false;

    public GameObject ChoisePanel;
    public GameObject LetterPanel;
    public GameObject WordPanel;
    public Bullet[] bullet_images=new Bullet[6];
    public Action OnCharChosen;      
    public Action OnWordChosen;
    private bool ShootResult;
    

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
                SoundManager.Instance.Play("Reload");
                BulletCells[i] = true;
                bullet_images[i].ChangeSprite();
                current_count++;
            }

        }
        if (current_count == 0)
        {
            StartCoroutine(Narrator.Instance.Talk("You have full pack!"));
        }
        SoundManager.Instance.Play("AfterReload");
    }


    public bool ShootYourself()
    {
        StartCoroutine(ShootYourselfRoutine());
        if (ShootResult)
        {
            ShootResult = false;
            return true;
        }
        else
        {
            ShootResult = false;
            return false;
        }
       
        
    }
    private IEnumerator ShootYourselfRoutine()
    {
        MoveRevolver();
        yield return new WaitForSeconds(1f);
    
        
        if (BulletCells[current_Bcell])
        {
            
            Die();
            SoundManager.Instance.Play("Shoot");
            bullet_images[current_Bcell].ChangeSprite();
            ShootResult = true;
        }
        else
        {
            SoundManager.Instance.Play("ShootFail");
            ShootResult = false;
        }
    
        yield return new WaitForSeconds(1f);
        MoveRevolver();
    }
    public bool ShootEnemy(IGameMember enemy)
    {
        
        if (BulletCells[current_Bcell] == true)
        {
            BulletCells[current_Bcell] = false;
            SoundManager.Instance.Play("Shoot");
            bullet_images[current_Bcell].ChangeSprite();
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
        current_Bcell = UnityEngine.Random.Range(0, 6);
    }

    public void TakeOut()
    {
        for (int i = 0; i < BulletCells.Length; i++)
        {
            if (BulletCells[i] == true)
            {
                SoundManager.Instance.Play("BulletOut");
                BulletCells[i] = false;
                bullet_images[i].ChangeSprite();
                break;
            }
        }
    }

    public void CharGet()
    {   
        if (string.IsNullOrEmpty(player_char.text))
        {
           
            return;
        }
        charinput = player_char.text[0];
        player_char.text = "";
        OnCharChosen?.Invoke();
        LetterPanel.SetActive(false);
    }

    public void WordGet()
    {
        if (string.IsNullOrEmpty(player_string.text))
        {
            
            return;
        }
        wordinput = player_string.text.Replace("\u200B", "").Trim();
        player_string.text = "";
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

    public void MoveRevolver()
    {
        if (!IsRotated)
        {
            revolver.transform.eulerAngles = new Vector3(-30, 150, 0);
            IsRotated = true;
        }
        else
        {
            revolver.transform.eulerAngles = new Vector3(0, -50, 0);
            IsRotated = false;
        }
    }
    
}