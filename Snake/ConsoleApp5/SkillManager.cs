using System.Collections.Generic;
using System.Linq;

class SkillManager
{
    public static SkillManager Instance = new SkillManager();
    public List<Skill> AcquiredSkills { get; private set; } = new List<Skill>();
    public int FixedBodyLength { get; private set; }

    private SkillManager() { }

    public void Initialize()
    {
        AcquiredSkills.Clear();
        FixedBodyLength = 1;
    }

    public void ShowSkillChoices()
    {
        Console.Clear();
        Console.WriteLine($"=== LEVEL {Player.Instance.Level} UP! ===");
        Console.WriteLine("Choose a skill to learn or upgrade:");
        var randomSkills = Enum.GetValues(typeof(SkillType)).Cast<SkillType>().OrderBy(x => GameManager.Instance.Tick % 3).Take(3).ToList();
        for (int i = 0; i < 3; i++)
        {
            var skill = randomSkills[i];
            int currentLevel = AcquiredSkills.FirstOrDefault(s => s.Type == skill)?.Level ?? 0;
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

    public void ApplySkill(SkillType type)
    {
        var existing = AcquiredSkills.FirstOrDefault(s => s.Type == type);
        if (existing != null)
        {
            existing.Upgrade();
            Console.WriteLine($"{type} upgraded to Lv.{existing.Level}!");
        }
        else
        {
            AcquiredSkills.Add(new Skill(type));
            Console.WriteLine($"New skill learned: {type} (Lv.1)!");
        }
        System.Threading.Thread.Sleep(1000);
    }

    // 스킬 보유 여부 확인 메서드
    public bool HasSkill(SkillType type)
    {
        return AcquiredSkills.Any(s => s.Type == type);
    }

    // 스킬 레벨 확인 메서드
    public int GetSkillLevel(SkillType type)
    {
        var skill = AcquiredSkills.FirstOrDefault(s => s.Type == type);
        return skill?.Level ?? 0;
    }
}