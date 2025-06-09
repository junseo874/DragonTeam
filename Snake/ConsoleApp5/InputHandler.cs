using SnakeGameRefactored;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SnakeGameRefactored.SkillManager;

namespace SnakeGameRefactored
{
    // InputHandler: 입력 처리
    public static class InputHandler
    {
        public static void HandleInput()
        {
            if (!Console.KeyAvailable) return;
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow when Player.DirY != 1: Player.DirX = 0; Player.DirY = -1; break;
                case ConsoleKey.DownArrow when Player.DirY != -1: Player.DirX = 0; Player.DirY = 1; break;
                case ConsoleKey.LeftArrow when Player.DirX != 1: Player.DirX = -1; Player.DirY = 0; break;
                case ConsoleKey.RightArrow when Player.DirX != -1: Player.DirX = 1; Player.DirY = 0; break;
                case ConsoleKey.Spacebar:
                    //if (Player.Snake.Count > 1)
                    //{
                    //    if (Player.Snake.Count > GameManager.FixedBodyLength)
                    //    {
                    //        int bulletCount = 1;
                    //        if (SkillManager.HasSkill(SkillType.DoubleShot))
                    //        {
                    //            bulletCount = SkillManager.GetSkillLevel(SkillType.DoubleShot) + 1;
                    //            bulletCount = Math.Min(bulletCount, 7);
                    //        }
                    //        int half = bulletCount / 2;
                    //        for (int i = 0; i < bulletCount; i++)
                    //        {
                    //            int offsetX = i - half;
                    //            BulletManager.Bullets.Add((Player.Snake[0].x + offsetX, Player.Snake[0].y));
                    //        }
                    //        if (SkillManager.HasSkill(SkillType.ContinuousShot))
                    //        {
                    //            int level = SkillManager.GetSkillLevel(SkillType.ContinuousShot);
                    //            foreach (var b in BulletManager.Bullets.ToArray())
                    //            {
                    //                BulletManager.ContinuousBullets[b] = level;
                    //            }
                    //        }
                            
                    //        if (SkillManager.HasSkill(SkillType.ContinuousShot))
                    //        {
                    //            int level = SkillManager.GetSkillLevel(SkillType.ContinuousShot);
                    //            foreach (var b in BulletManager.Bullets.ToArray())
                    //            {
                    //                BulletManager.ContinuousBullets[b] = level;
                    //            }
                    //        }
                    //        Player.Snake.RemoveAt(Player.Snake.Count - 1);
                    //    }
                    //}
                    break;
                case ConsoleKey.C:
                    //if (SkillManager.HasSkill(SkillType.Clone) && CloneManager.CloneCooldown <= 0)
                    //{
                    //    bool spawned = false;
                    //    for (int dy = -1; dy <= 1; dy++)
                    //    {
                    //        for (int dx = -1; dx <= 1; dx++)
                    //        {
                    //            int nx = Player.Snake[0].x + dx;
                    //            int ny = Player.Snake[0].y + dy;
                    //            if (nx > 0 && nx < GameManager.Width - 1 && ny > 0 && ny < GameManager.Height - 1 &&
                    //                MapManager.Map[ny, nx] == ' ')
                    //            {
                    //                CloneManager.Clones.Add((nx, ny));
                    //                CloneManager.CloneCooldown = 100;
                    //                spawned = true;
                    //                break;
                    //            }
                    //        }
                    //        if (spawned) break;
                    //    }
                    //}
                    break;
            }
        }
    }
}
