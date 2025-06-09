using System;

class ExpManager
{
    public static ExpManager Instance = new ExpManager();
    private Random rand = new Random();

    private ExpManager() { }

    public void GenerateExpChar()
    {
        if (rand.NextDouble() < 0.1)
        {
            int x = rand.Next(1, GameManager.Width - 1);
            int y = rand.Next(GameManager.Height / 2, GameManager.Height - 2);

            if (MapManager.Instance.GetCharAt(y, x) == ' ')
                MapManager.Instance.SetCharAt(y, x, '*');
        }
    }
}