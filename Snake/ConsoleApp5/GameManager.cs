using System;

class GameManager
{
    public const int Width = 60;
    public const int Height = 30;

    // 싱글톤 인스턴스
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    public bool IsRunning { get; set; }
    public int Tick { get; set; }

    // 생성자는 외부에서 호출할 수 없도록 private로 설정!!
    private GameManager()
    {
        // 초기화 로직 (필요한 경우 추가)
    }

    public void ShowStartScreen()
    {
        Console.Clear();
        Console.WriteLine("=== SNAKE GAME ===");
        Console.WriteLine("Use arrow keys to move.");
        Console.WriteLine("Press Space to shoot (consumes body).");
        Console.WriteLine("Press C to spawn clone (if available).");
        Console.WriteLine("Eat * to gain EXP and grow.");
        Console.WriteLine("Avoid walls and your own body!");
        Console.WriteLine("Press any key to start...");
        Console.ReadKey(true);
    }

    public bool ShowGameOver()
    {
        Console.Clear();
        Console.WriteLine("==== GAME OVER ====");
        Console.WriteLine($"EXP: {Player.Instance.Exp} | Level: {Player.Instance.Level}");
        Console.WriteLine("Press R to restart or Q to quit.");
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.R) return true;
            else if (key == ConsoleKey.Q) return false;
        }
    }

    public void InitGame()
    {
        MapManager.Instance.Initialize();
        Player.Instance.Initialize();
        BulletManager.Instance.Initialize();
        BossManager.Instance.Initialize();
        SkillManager.Instance.Initialize();
        IsRunning = true;
        Tick = 0;
    }

    public void Update()
    {
        Player.Instance.Move();
        BulletManager.Instance.Update();
        CloneManager.Instance.Update();
        WallManager.Instance.Update();
        ExpManager.Instance.GenerateExpChar();
        BossManager.Instance.Update();
    }

    public void CheckLevelUp()
    {
        if (Player.Instance.Exp >= (Player.Instance.Level + 1) * 5)
        {
            Player.Instance.Level++;
            SkillManager.Instance.ShowSkillChoices();
            System.Threading.Thread.Sleep(1000);
        }
    }
}