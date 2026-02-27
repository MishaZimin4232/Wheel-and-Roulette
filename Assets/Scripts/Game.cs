using TMPro;
using UnityEngine;
using System.Collections;
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
    private bool waitingForPlayerInput = false;
    private bool IsFirstRound = true;

    [SerializeField]private GameObject WinMenu;
    [SerializeField]private GameObject LoseMenu;
    [SerializeField]private TextMeshProUGUI win_scores;
    
    void Start()
    {
        WinMenu.SetActive(false);
        LoseMenu.SetActive(false);
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
        board.EnterQuestion(question);
        bot.GetAnswer(answer);
        status = GameStatus.Wheel;
        Narrator.Instance.Talk("Welcome to the Wheel and Roulette!");
        Narrator.Instance.Task(question);
        wheel.OnCellLanded += OnCellLanded;
        SubscribeToPlayerEvents();
        ProcessGameState();
    }
    private void SubscribeToPlayerEvents()
    {
        
        UnsubscribeFromPlayerEvents();
        
        
        player.OnCharChosen += OnPlayerCharChosen;
        player.OnWordChosen += OnPlayerWordChosen;
    }
    
    private void UnsubscribeFromPlayerEvents()
    {
        if (player != null)
        {
            player.OnCharChosen -= OnPlayerCharChosen;
            player.OnWordChosen -= OnPlayerWordChosen;
        }
    }

    private void OnPlayerCharChosen()
    {
        if (waitingForPlayerInput)
        {
            waitingForPlayerInput = false;
            ProcessCharInput();
        }
    }
    private void OnPlayerWordChosen()
    {
        if (waitingForPlayerInput)
        {
            waitingForPlayerInput = false;
            ProcessWordInput();
        }
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
                StartCoroutine(ProcessRouletteState());
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
            
            Narrator.Instance.Talk("Spin the wheel");
            wheel.CanSpin=true;
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
           Debug.Log("pendingCell is ScoreCell");
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
            pendingCell = null;
            ProcessGameState();
        }
        
        
    }
    
    private void HandleScoreCell(ScoreCell cell)
    {
        if (current_player is Player)
        {
           
            ShowPlayerInputChoice();
        }
        else
        {
            ProcessCharInput();
        }
    }
    
    private void ShowPlayerInputChoice()
    {
        player.PlayerInput();
        waitingForPlayerInput = true;
    }
    
    private void ProcessCharInput()
    {
        IsFirstRound = false;
        char input = current_player.CharInput();
        Debug.Log(input);
        if (CheckCharInput(input))
        {
            SoundManager.Instance.Play("Correct");
            Narrator.Instance.Talk( "Right letter");
            board.OpenChar(input);
            target_player.AddBullet(1);
            pendingCell.Action(current_player,target_player, board);
            if (board.IsOpen())
            {
                Narrator.Instance.Talk("Word is done!");
                status = GameStatus.Roulette;
                ChangePlayer(); 
            }
        }
        else
        {
            SoundManager.Instance.Play("Wrong");
            Narrator.Instance.Talk("Wrong char, 1 bullet");
            current_player.AddBullet(1);
            ChangePlayer();
        }
        
        ProcessGameState();
    }
    
    private void ProcessWordInput()
    {
        string input = player.WordInput();
        if (CheckWordInput(input))
        {
            board.OpenString();
            pendingCell.Action(current_player,target_player, board);
            SoundManager.Instance.Play("Correct");
            if (IsFirstRound)
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
            SoundManager.Instance.Play("Wrong");
            current_player.AddBullet(2);
            ChangePlayer();
            IsFirstRound = false;
        }
        
        ProcessGameState();
    }
    
    
    private IEnumerator ProcessRouletteState()
    {
        Narrator.Instance.Talk("Начинается русская рулетка");
        yield return new WaitForSeconds(0.5f);
        current_player.Round();
        yield return new WaitForSeconds(0.5f);
        if (current_player.ShootYourself())
        {
            Narrator.Instance.Talk($"💀 {current_player} killed!");
            yield return new WaitForSeconds(1.5f);
            EndGame(current_player is Bot); 
        }
        else
        {
            Narrator.Instance.Talk("Empty...");
            yield return new WaitForSeconds(1.5f);
            ChangePlayer();
            ProcessGameState();
        }
    }
    
    private bool CheckCharInput(char input)
    {
        if (!char.IsLetter(input))
        {
            Narrator.Instance.Talk("Введите букву!");
            return false;
        }
        bool letterExists = false;
        bool letterAlreadyOpen = false;
        foreach (char c in answer)
        {
            if (char.ToUpper(input) == char.ToUpper(c))
            {
                letterExists = true;
                if (board.IsCharOpen(c))
                {
                    letterAlreadyOpen = true;
                }
            }
        }
    
        if (!letterExists)
        {
            Narrator.Instance.Talk("Такой буквы нет в слове!");
            return false;
        }
    
        if (letterAlreadyOpen)
        {
            Narrator.Instance.Talk("Эта буква уже открыта!");
            return false;
        }
    
        return true;
    }
    
    private bool CheckWordInput(string input)
    {
        if (!string.IsNullOrEmpty(input) &&
            input.ToUpper().Equals(answer.ToUpper()))
        {
            return true;
        }
        else
        {
            return false;
        }
        
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
            Narrator.Instance.Talk("player take 500 score for winning");
            SoundManager.Instance.Play("Win");
            player.AddScore(500);
            int totalScore = player.score;
            WinMenu.SetActive(true);
            win_scores.text = "You won" + totalScore + " score";
            
        }
        else
        {
            SoundManager.Instance.Play("Fail");
           LoseMenu.SetActive(true);
        }
        Time.timeScale = 0;
    }
}