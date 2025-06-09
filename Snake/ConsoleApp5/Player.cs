using System.Collections.Generic;

class Player
{
    public static Player Instance = new Player();
    public List<(int x, int y)> Body { get; private set; } = new List<(int x, int y)>();
    public int Exp { get; set; }
    public int Level { get; set; }
    public int DirX { get; set; } = 1;
    public int DirY { get; set; } = 0;

    private Player() { }

    public void Initialize()
    {
        Body.Clear();
        Body.Add((GameManager.Width / 2, GameManager.Height - 2));
        for (int i = 1; i <= 3; i++) Body.Add((GameManager.Width / 2 - i, GameManager.Height - 2));
        Exp = 0;
        Level = 0;
    }

    public void Move()
    {
        var newHead = (x: Body[0].x + DirX, y: Body[0].y + DirY);

        if (newHead.x <= 0 || newHead.x >= GameManager.Width - 1 ||
            newHead.y <= 0 || newHead.y >= GameManager.Height - 1 ||
            Body.Exists(part => part.x == newHead.x && part.y == newHead.y))
        {
            GameManager.Instance.IsRunning = false;
            return;
        }

        if (MapManager.Instance.GetCharAt(newHead.y, newHead.x) == '*')
        {
            Exp++;
            MapManager.Instance.SetCharAt(newHead.y, newHead.x, ' ');
            Body.Insert(0, newHead);
        }
        else
        {
            Body.Insert(0, newHead);
            if (Body.Count > SkillManager.Instance.FixedBodyLength) Body.RemoveAt(Body.Count - 1);
        }
    }
}