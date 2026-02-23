using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public QuestionBank bank;
    private string answer;
    private string question;
    
    private Bot bot;
    private Player player;
    private Board board;
    private Wheel wheel;
  
    public IGameMember current_player;
    public IGameMember target_player;
    private GameStatus status;
    public TextMeshProUGUI Turn;
    
    private bool isCellSelected = false;
    private Cell pendingCell; 
    
    void Start()
    {
        board = FindObjectOfType<Board>();
        wheel = FindObjectOfType<Wheel>();
        player = FindObjectOfType<Player>();
        bot = FindObjectOfType<Bot>();
        Turn.text ="Player turn";  
        current_player = player;
        target_player = bot;
        
        int question_number = Random.Range(0, bank.qb.Count);
        answer = bank.qb[question_number].answer;
        question = bank.qb[question_number].question;
        
        board.SetWord(answer);
        bot.GetAnswer(answer);
        status = GameStatus.Wheel;
        Narrator.Instance.Talk("Welcome to the Wheel and Roulette!");
        Narrator.Instance.Task(question);
        wheel.OnCellLanded += OnCellLanded;
        ProcessGameState();
    }
    
    private void OnDestroy()
    {
        if (wheel != null)
            wheel.OnCellLanded -= OnCellLanded;
    }

    private void ProcessGameState()
    {
        if (!player.IsAlive || !bot.IsAlive)
        {
            EndGame(player.IsAlive);
            return;
        }
        
        switch (status)
        {
            case GameStatus.Wheel:
                ProcessWheelState();
                break;
                
            case GameStatus.Roulette:
                ProcessRouletteState();
                break;
        }
    }

    private void ProcessWheelState()
    {
        Debug.Log($"🎡 Состояние: WHEEL, игрок: {current_player}");
        
        if (!isCellSelected)
        {
            SelectCellForCurrentPlayer();
        }
        else
        {
            Debug.Log("Клетка выбрана, ожидание вращения...");
        }
    }
    
    private void SelectCellForCurrentPlayer()
    {
        
        if (current_player is Player)
        {
            Debug.Log("Игрок вращает колесо...");
            wheel.SpinToRandomCell();
        }
        else
        {
         
            Debug.Log("Бот вращает колесо...");
            wheel.SpinToRandomCell();
        }
        
        isCellSelected = true;
    }
    
    
    private void OnCellLanded(Cell landedCell)
    {
   
        pendingCell = landedCell;
        isCellSelected = false;
        ActivatePendingCell();
    }
    
   
    private void ActivatePendingCell()
    {
        if (pendingCell == null)
        {
            Debug.LogError("Нет клетки для активации!");
            ProcessGameState();
            return;
        }
        
        
        
       
        if (pendingCell is ScoreCell scoreCell)
        {
           
            HandleScoreCell(scoreCell);
        }
        else
        {
        
            pendingCell.Action(current_player, target_player, board);
            
            if (!player.IsAlive || !bot.IsAlive)
            {
                EndGame(player.IsAlive);
                return;
            }
            
            ProcessGameState();
        }
        
        pendingCell = null;
    }
    
    private void HandleScoreCell(ScoreCell cell)
    {
        
        
        
        if (current_player is Player)
        {
           
            ShowPlayerInputChoice();
        }
        else
        {
            
            ProcessBotCharInput();
        }
    }
    
    private void ShowPlayerInputChoice()
    {
        
        ProcessPlayerCharInput();
    }
    
    private void ProcessPlayerCharInput()
    {
        char playerChar = player.CharInput();
        
        if (CheckCharInput(playerChar))
        {
            Narrator.Instance.Talk("Правильная буква");
            board.OpenChar(playerChar);
            target_player.AddBullet(1);
            pendingCell.Action(current_player,target_player, board);
            if (board.IsOpen())
            {
    
                Narrator.Instance.Talk("Слово отгадано");
                status = GameStatus.Roulette;
                ChangePlayer(); 
            }
        }
        else
        {
            Narrator.Instance.Talk("Неправильная буква");
            current_player.AddBullet(1);
            
            ChangePlayer();
        }
        
        ProcessGameState();
    }
    
    private void ProcessPlayerWordInput()
    {
        string playerWord = player.WordInput();
        
        if (CheckWordInput(playerWord))
        {
            board.OpenString();
            
            if (IsFirstRound())
            {
                Narrator.Instance.Talk("Shit! Game Over!");
                EndGame(true);
                return;
            }
            
            status = GameStatus.Roulette;
            ChangePlayer();
        }
        else
        {
            current_player.AddBullet(2);
            ChangePlayer();
        }
        
        ProcessGameState();
    }
    
    private void ProcessBotCharInput()
    {
        char botChar = bot.CharInput();
        
        if (CheckCharInput(botChar))
        {
            Narrator.Instance.Talk("Правильная буква");
            board.OpenChar(botChar);
            target_player.AddBullet(1);
            
            if (board.IsOpen())
            {
                status = GameStatus.Roulette;
                ChangePlayer();
            }
            
        }
        else
        {
            current_player.AddBullet(1);
            Narrator.Instance.Talk("Неправильная буква");
            ChangePlayer();
        }
        
        ProcessGameState();
    }
    private void ProcessRouletteState()
    {
        Narrator.Instance.Talk("Начинается русская рулетка");
        Debug.Log($"state: ROULETTE, turn: {current_player}");
        
        
        current_player.Round();
        Debug.Log($" {current_player} вращает барабан");
        
        
        if (current_player.ShootYourself())
        {
            Narrator.Instance.Talk($"💀 {current_player} killed!");
            EndGame(current_player is Bot); 
        }
        else
        {
            Narrator.Instance.Talk("Empty...");
            ChangePlayer();
            ProcessGameState();
        }
    }
    
    private bool CheckCharInput(char input)
    {
        if (!char.IsLetter(input)) return false;
        
        foreach (char c in answer)
        {
            if (char.ToUpper(input) == char.ToUpper(c) && !board.IsCharOpen(c))
            {
                return true;
            }
            else
            {
                Narrator.Instance.Talk("Эта буква уже есть!");
                return false;
            }
        }
        return false;
    }
    
    private bool CheckWordInput(string input)
    {
        return !string.IsNullOrEmpty(input) && 
               input.ToUpper().Equals(answer.ToUpper());
    }
    
    private bool IsFirstRound()
    {
        for (int i = 0; i < answer.Length; i++)
        {
            if (board.IsCharOpen(answer[i]))
                return false;
        }
        return true;
    }
    
    private void ChangePlayer()
    {
        if (current_player == player)
        { 
            current_player = bot;
            target_player = player;
            Turn.text ="Bot turn"; 
        }
        else
        {
            Turn.text ="Player turn"; 
            current_player = player;
            target_player = bot;
        }
        
        
    }
    
    private void EndGame(bool playerWon)
    {
        if (playerWon)
        {
            int totalScore = player.score + 500;
            Narrator.Instance.Talk($" You won {totalScore} score!");
        }
        else
        {
            Narrator.Instance.Talk("Wasted!");
        }
    }
}