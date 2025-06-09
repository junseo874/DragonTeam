public enum SkillType
{
    Grow, Clone, DoubleShot, AutoShot, Explosion, Heal, SpeedUp, ContinuousShot, OrbitAsteroid, CircleShot
}

public class Skill
{
    public SkillType Type { get; }
    public int Level { get; private set; }

    public Skill(SkillType type)
    {
        Type = type;
        Level = 1;
    }

    public void Upgrade() => Level++;
}