using SnakeGameRefactored;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 테스트
namespace SnakeGameRefactored
{ // CloneManager: 복제체 관련 처리
    public static class CloneManager
    {
        public static List<(int x, int y)> Clones = new List<(int x, int y)>();
        public static int CloneCooldown = 0;

        public static void UpdateClones()
        {
            for (int i = Clones.Count - 1; i >= 0; i--)
            {
                var c = Clones[i];
                int[] directionWeights = { -1, -1, -1, -1, 0, 0, 1, -2, -2, +2, +2 };
                int totalWeight = directionWeights.Length;
                int choice = GameManager.Rand.Next(totalWeight);
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
                bool hitWall = false;
                if (nx <= 0 || nx >= GameManager.Width - 1)
                {
                    dx = -dx;
                    nx = c.x + dx;
                    hitWall = true;
                }
                if (ny <= 0 || ny >= GameManager.Height - 1)
                {
                    dy = -dy;
                    ny = c.y + dy;
                    hitWall = true;
                }
                if (nx > 0 && nx < GameManager.Width - 1 && ny > 0 && ny < GameManager.Height - 1)
                {
                    if (MapManager.Map[ny, nx] == '#')
                        MapManager.Map[ny, nx] = ' ';
                    Clones[i] = (nx, ny);
                }
                else if (!hitWall)
                {
                    Clones.RemoveAt(i);
                }
            }
            if (CloneCooldown > 0) CloneCooldown--;
        }
    }
}
