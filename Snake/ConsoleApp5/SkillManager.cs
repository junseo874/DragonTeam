using SnakeGameRefactored;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SnakeGameRefactored.SkillManager;
namespace SnakeGameRefactored
{ // SkillManager: 스킬 관련 처리
    public static class SkillManager
    {
        public enum SkillType { Grow, Clone, DoubleShot, AutoShot, Explosion, Heal, SpeedUp, ContinuousShot, OrbitAsteroid, CircleShot }

        public class Skill
        {
            public SkillType Type;
            public int Level;
            public Skill(SkillType type)
            {
                Type = type;
                Level = 1;
            }
            public void Upgrade() => Level++;
        }

        public static bool HasSkill(SkillType type)
        {
            return GameManager.AcquiredSkills.Any(s => s.Type == type);
        }

        public static int GetSkillLevel(SkillType type)
        {
            var skill = GameManager.AcquiredSkills.FirstOrDefault(s => s.Type == type);
            return skill?.Level ?? 0;
        }

        public static void CheckLevelUp()
        {
            if (GameManager.Exp >= (GameManager.Level + 1) * 5)
            {
                GameManager.Level++;
                ShowSkillChoices();
                GameManager.FixedLengthLevel = GameManager.Level + 1;
                GameManager.FixedBodyLength = GameManager.FixedLengthLevel;
                if (BossManager.Asteroids.Count < 30 && HasSkill(SkillType.OrbitAsteroid))
                {
                    int baseRadius = 5;
                    int extraRadius = GetSkillLevel(SkillType.OrbitAsteroid);
                    int radius = baseRadius + extraRadius;
                    int ringCount = 8;
                    CreateAsteroidRing(ringCount, radius);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        public static void ShowSkillChoices()
        {
            Console.Clear();
            Console.WriteLine($"=== LEVEL {GameManager.Level} UP! ===");
            Console.WriteLine("Choose a skill to learn or upgrade:");
            var allSkills = Enum.GetValues(typeof(SkillType)).Cast<SkillType>().ToList();
            var randomSkills = allSkills.OrderBy(x => GameManager.Rand.Next()).Take(3).ToList();
            for (int i = 0; i < 3; i++)
            {
                var skill = randomSkills[i];
                int currentLevel = GetSkillLevel(skill);
                Console.WriteLine($"{i + 1}. {skill} (Lv.{currentLevel})");
            }
            Console.Write("Select (1-3): ");
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key >= ConsoleKey.D1 && key <= ConsoleKey.D3)
                {
                    int idx = (int)(key - ConsoleKey.D1);
                    ApplySkill(randomSkills[idx]);
                    break;
                }
            }
        }

        public static void ApplySkill(SkillType type)
        {
            var existing = GameManager.AcquiredSkills.FirstOrDefault(s => s.Type == type);
            if (existing != null)
            {
                existing.Upgrade();
                Console.WriteLine($"\n{type} upgraded to Lv.{existing.Level}!");
            }
            else
            {
                GameManager.AcquiredSkills.Add(new Skill(type));
                Console.WriteLine($"\nNew skill learned: {type} (Lv.1)!");
            }
            System.Threading.Thread.Sleep(1000);
        }

        public static void CreateAsteroidRing(int count, int radius)
        {
            for (int i = 0; i < count; i++)
            {
                double angle = Math.PI * 2 * i / count;
                int x = Player.Snake[0].x + (int)(Math.Cos(angle) * radius);
                int y = Player.Snake[0].y + (int)(Math.Sin(angle) * radius * 0.6);
                BulletManager.Asteroids.Add((x, y, angle, radius));
            }
        }
    }
}
