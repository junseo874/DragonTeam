using SnakeGameRefactored;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameRefactored
{
    // ExpManager: 경험치 아이템 생성
    public static class ExpManager
    {
        public static void GenerateExpChar()
        {
            if (GameManager.Rand.NextDouble() < 0.1)
            {
                int x = GameManager.Rand.Next(1, GameManager.Width - 1);
                int y = GameManager.Rand.Next(GameManager.Height / 2, GameManager.Height - 2);
                if (MapManager.Map[y, x] == ' ')
                    MapManager.Map[y, x] = '*';
            }
        }
    }
}
