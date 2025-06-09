class WallManager
{
    public static WallManager Instance = new WallManager();
    private int wallDropDelay = 50;
    private int minWallDropDelay = 10;

    private WallManager() { }

    public void Initialize()
    {
        wallDropDelay = 50;
    }

    public void Update()
    {
        if (GameManager.Instance.Tick % 300 == 0 && wallDropDelay > minWallDropDelay)
        {
            wallDropDelay -= 2;
        }

        if (GameManager.Instance.Tick % wallDropDelay == 0)
        {
            for (int y = GameManager.Height - 2; y >= 1; y--)
            {
                for (int x = 1; x < GameManager.Width - 1; x++)
                {
                    if (MapManager.Instance.GetCharAt(y - 1, x) == '#')
                    {
                        MapManager.Instance.SetCharAt(y, x, '#');
                        MapManager.Instance.SetCharAt(y - 1, x, ' ');
                    }
                }
            }

            for (int x = 1; x < GameManager.Width - 1; x++)
                MapManager.Instance.SetCharAt(1, x, '#');
        }
    }
}