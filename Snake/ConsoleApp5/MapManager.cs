using System;

class MapManager
{
    public static MapManager Instance = new MapManager();
    private char[,] map;

    public MapManager()
    {
        map = new char[GameManager.Height, GameManager.Width];
    }

    public void Initialize()
    {
        for (int y = 0; y < GameManager.Height; y++)
        {
            for (int x = 0; x < GameManager.Width; x++)
            {
                map[y, x] = ' ';
            }
        }

        for (int y = 0; y < GameManager.Height; y++)
        {
            map[y, 0] = '#';
            map[y, GameManager.Width - 1] = '#';
        }

        for (int x = 0; x < GameManager.Width; x++)
        {
            map[0, x] = '#';
            map[GameManager.Height - 1, x] = '#';
        }
    }

    public char GetCharAt(int y, int x)
    {
        return map[y, x];
    }

    public void SetCharAt(int y, int x, char value)
    {
        map[y, x] = value;
    }

    // CloneMap 메서드 추가
    public char[,] CloneMap()
    {
        char[,] clonedMap = new char[GameManager.Height, GameManager.Width];
        for (int y = 0; y < GameManager.Height; y++)
        {
            for (int x = 0; x < GameManager.Width; x++)
            {
                clonedMap[y, x] = map[y, x];
            }
        }
        return clonedMap;
    }
}