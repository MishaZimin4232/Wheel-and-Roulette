using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public QuestionBank bank;
    private string answer;
    private string question;
    private int question_number;
    public Bot bot;
    public Player player;
    private Narrator narrator = new ();
    public Board board;
  
    public IGameMember current_player;
    public GameStatus status;
    
    
    
    void Start()
    {
        
        current_player = player;
        question_number = Random.Range(0, bank.qb.Count);
        answer = bank.qb[question_number].answer;
        question = bank.qb[question_number].question;
        status = GameStatus.Wheel;
        narrator.Talk("Добро пожаловать в игру на выживание, угадайте слово:");
        narrator.Talk(question);
        
        
        
        
    }

    void ChangePlayer()
    {
        if (current_player == player)
        { 
            current_player = bot;
        }
        else
        {
            current_player = player;
        }   
    }

    void EndGame(bool IsWin)
    {
        if (IsWin)
        {
            
            narrator.Talk("Ты победил!");
        }
        else
        {
            
            narrator.Talk("Зажмурился Ж)");
        }
    }
    void WaitInput()
    {
        
        string word_apply=player.WordInput();
        char char_apply=player.CharInput();
        
        if(answer.Contains(char_apply))
        {
            narrator.Talk("Правильный ответ");
            board.OpenChar(char_apply);
            if (current_player == player)
            {
                player.AddScore(500);
            }
            
            Debug.Log("Ответ верный");
        }
        else
        {
            Debug.Log("Ответ неверный");
        }
        //Вызывается меню для выбора опции(выбрать букву или назвать слово)
        //В зависимости от кнопки сработает либо CharInput(), либо WordInput()
        //В переменные выше присвоется ответ пользователя
        //Если ответ правильный, то будет board.OpenChar или board.OpenWord
        //Если current_player == player,то current_player.AddScore()
    }

    void EnemyCharge(IGameMember current_player)
    {
        switch (current_player)
        {
            case Player:
                bot.AddBullet(1);
                break;
            case Bot:
                player.AddBullet(1);
                break;
        }
    }

}
