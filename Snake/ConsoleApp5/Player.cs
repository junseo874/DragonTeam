using SnakeGameRefactored;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SnakeGameRefactored.SkillManager;
namespace SnakeGameRefactored
{ // Player: 스네이크 관련 로직
    public static class Player
    {
        public static List<(int x, int y)> Snake = new List<(int x, int y)>();
        public static int DirX { get; set; } = 1;
        public static int DirY { get; set; } = 0;

        public static void MoveSnake()
        {
            var newHead = (x: Snake[0].x + DirX, y: Snake[0].y + DirY);

            if (newHead.x <= 0 || newHead.x >= GameManager.Width - 1 ||
                newHead.y <= 0 || newHead.y >= GameManager.Height - 1)
            {
                newHead.x = Math.Max(1, Math.Min(GameManager.Width - 2, newHead.x));
                newHead.y = Math.Max(1, Math.Min(GameManager.Height - 2, newHead.y));
            }

            if (MapManager.Map[newHead.y, newHead.x] == '#' ||
                Snake.Exists(part => part.x == newHead.x && part.y == newHead.y))
            {
                GameManager.Running = false;
                return;
            }

            if (MapManager.Map[newHead.y, newHead.x] == '*')
            {
                GameManager.Exp++;
                MapManager.Map[newHead.y, newHead.x] = ' ';
                Snake.Insert(0, newHead);
                //if (SkillManager.HasSkill(SkillType.Heal))
                //{
                //    int healCount = SkillManager.GetSkillLevel(SkillType.Heal);
                //    for (int i = 0; i < healCount; i++)
                //    {
                //        Snake.Add((Snake[^1].x - DirX, Snake[^1].y - DirY));
                //    }
                //}
            }
            else
            {
                Snake.Insert(0, newHead);
                if (Snake.Count > GameManager.FixedBodyLength)
                    Snake.RemoveAt(Snake.Count - 1);
            }
        }
    }
}
