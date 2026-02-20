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
    
    // Флаг для отслеживания, выбрана ли уже клетка
    private bool isCellSelected = false;
    private Cell pendingCell; 
    
    void Start()
    {
        board = FindObjectOfType<Board>();
        wheel = FindObjectOfType<Wheel>();
        player = FindObjectOfType<Player>();
        bot = FindObjectOfType<Bot>();
        
        current_player = player;
        target_player = bot;
        
        int question_number = Random.Range(0, bank.qb.Count);
        answer = bank.qb[question_number].answer;
        question = bank.qb[question_number].question;
        
        board.SetWord(answer);
        status = GameStatus.Wheel;
        
        Narrator.Instance.Talk("Welcome to the Wheel and Roulette!");
        Narrator.Instance.Task(question);
       
        // Подписываемся на событие приземления клетки
        wheel.OnCellLanded += OnCellLanded;
        
        // Запускаем игру
        ProcessGameState();
    }
    
    private void OnDestroy()
    {
        if (wheel != null)
            wheel.OnCellLanded -= OnCellLanded;
    }
    
    /// <summary>
    /// Основной метод состояния игры
    /// </summary>
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
    
    /// <summary>
    /// Состояние колеса
    /// </summary>
    private void ProcessWheelState()
    {
        Debug.Log($"🎡 Состояние: WHEEL, игрок: {current_player}");
        
        if (!isCellSelected)
        {
            // 1. ВЫБИРАЕМ клетку (ДО вращения)
            SelectCellForCurrentPlayer();
        }
        else
        {
            // 2. Клетка уже выбрана, ждём вращение
            Debug.Log("Клетка выбрана, ожидание вращения...");
        }
    }
    
    /// <summary>
    /// Выбрать клетку для текущего игрока
    /// </summary>
    private void SelectCellForCurrentPlayer()
    {
        // Здесь можно добавить логику выбора клетки:
        // - Для игрока - через UI
        // - Для бота - случайно
        
        if (current_player is Player)
        {
            // Игрок - можно показать UI для выбора клетки
            // Но по ТЗ клетка выбирается случайно вращением
            // Поэтому просто запускаем случайное вращение
            Debug.Log("Игрок вращает колесо...");
            wheel.SpinToRandomCell();
        }
        else
        {
            // Бот - выбираем случайную клетку
            Debug.Log("Бот вращает колесо...");
            wheel.SpinToRandomCell();
        }
        
        isCellSelected = true;
    }
    
    /// <summary>
    /// Вызывается, когда колесо завершило вращение и клетка приземлилась
    /// </summary>
    private void OnCellLanded(Cell landedCell)
    {
       
        
        // Сохраняем клетку для активации
        pendingCell = landedCell;
        isCellSelected = false;
        
        // Активируем клетку
        ActivatePendingCell();
    }
    
    /// <summary>
    /// Активировать выбранную клетку
    /// </summary>
    private void ActivatePendingCell()
    {
        if (pendingCell == null)
        {
            Debug.LogError("Нет клетки для активации!");
            ProcessGameState();
            return;
        }
        
        
        
        // Проверяем тип клетки
        if (pendingCell is ScoreCell scoreCell)
        {
            // ScoreCell требует выбора буквы/слова
            HandleScoreCell(scoreCell);
        }
        else
        {
            // Обычная клетка - сразу выполняем действие
            pendingCell.Action(current_player, target_player, board);
            
            // Проверяем, не умер ли кто
            if (!player.IsAlive || !bot.IsAlive)
            {
                EndGame(player.IsAlive);
                return;
            }
            
            // Продолжаем игру
            ProcessGameState();
        }
        
        pendingCell = null;
    }
    
    /// <summary>
    /// Обработка ScoreCell
    /// </summary>
    private void HandleScoreCell(ScoreCell cell)
    {
        // Сначала начисляем очки
        cell.Action(current_player, target_player, board);
        
        if (current_player is Player)
        {
            // Игрок выбирает букву или слово
            // Показываем UI выбора
            ShowPlayerInputChoice();
        }
        else
        {
            // Бот автоматически выбирает букву
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
            // Угадал
            board.OpenChar(playerChar);
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
            status = GameStatus.Roulette;
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
            status = GameStatus.Roulette;
            ChangePlayer();
        }
        
        ProcessGameState();
    }
    
    /// <summary>
    /// Состояние русской рулетки
    /// </summary>
    private void ProcessRouletteState()
    {
        Debug.Log($"state: ROULETTE, turn: {current_player}");
        
        // Анимация вращения барабана
        current_player.Round();
        
        // Выстрел
        if (current_player.ShootYourself())
        {
            Narrator.Instance.Talk($"💀 {current_player} killed!");
            EndGame(current_player is Bot); // Если умер текущий, выиграл противник
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
                return true;
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
        }
        else
        {
            current_player = player;
            target_player = bot;
        }
        
        Debug.Log($" Turn: {current_player}");
    }
    
    private void EndGame(bool playerWon)
    {
        if (playerWon)
        {
            int totalScore = player.score + 500;
            Narrator.Instance.Talk($" Lucky asshole!");
        }
        else
        {
            Narrator.Instance.Talk("Loser1");
        }
        
        // Здесь можно показать экран окончания
    }
}