class CloneManager
{
    public static CloneManager Instance = new CloneManager();
    private List<(int x, int y)> clones = new List<(int x, int y)>();
    public int Cooldown { get; set; } = 0;

    private CloneManager() { }

    public void Initialize()
    {
        clones.Clear();
        Cooldown = 0;
    }

    public void SpawnClone(int x, int y)
    {
        clones.Add((x, y));
    }

    public void Update()
    {
        if (Cooldown > 0) Cooldown--;

        for (int i = clones.Count - 1; i >= 0; i--)
        {
            var c = clones[i];
            int[] directionWeights = { -1, -1, -1, -1, 0, 0, 1, -2, -2, +2, +2 };
            int totalWeight = directionWeights.Length;
            int choice = GameManager.Instance.Tick % totalWeight;
            int dx = 0, dy = 0;

            switch (directionWeights[choice])
            {
                case -2: dx = -1; break;
                case -1: dy = -1; break;
                case 0: break;
                case +1: dy = +1; break;
                case +2: dx = +1; break;
            }

            int nx = c.x + dx;
            int ny = c.y + dy;

            if (nx <= 0 || nx >= GameManager.Width - 1)
                dx = -dx;
            if (ny <= 0 || ny >= GameManager.Height - 1)
                dy = -dy;

            nx = c.x + dx;
            ny = c.y + dy;

            if (MapManager.Instance.GetCharAt(ny, nx) == ' ')
            {
                clones[i] = (nx, ny);
            }
            else
            {
                clones.RemoveAt(i);
            }
        }
    }
}