using System.Collections.Generic;

class BulletManager
{
    public static BulletManager Instance = new BulletManager();
    private List<(int x, int y)> bullets = new List<(int x, int y)>();
    private Dictionary<(int x, int y), int> continuousBullets = new Dictionary<(int x, int y), int>();

    private BulletManager() { }

    public void Initialize()
    {
        bullets.Clear();
        continuousBullets.Clear();
    }

    public void AddBullet(int x, int y)
    {
        bullets.Add((x, y));
    }
    public List<(int x, int y)> Bullets
    {
        get
        {
            return new List<(int x, int y)>(bullets); // 새로운 리스트로 복사하여 반환
        }
    }

    // 탄환 목록을 반환하는 메서드
    public List<(int x, int y)> GetBullets()
    {
        return bullets.ToList(); // 현재 탄환 목록 복사본 반환
    }

    // 연속 발사 탄환 추가 메서드
    public void AddContinuousBullet((int x, int y) position, int level)
    {
        if (!continuousBullets.ContainsKey(position))
        {
            continuousBullets[position] = level; // 새로운 위치에 연속 발사 탄환 추가
        }
    }

    public void Update()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            var b = bullets[i];
            if (b.y <= 1 || MapManager.Instance.GetCharAt(b.y - 1, b.x) == '#')
            {
                if (b.y > 1 && MapManager.Instance.GetCharAt(b.y - 1, b.x) == '#')
                    MapManager.Instance.SetCharAt(b.y - 1, b.x, ' ');
                bullets.RemoveAt(i);
            }
            else
            {
                bullets[i] = (b.x, b.y - 1);
            }
        }

        // 연속 발사 탄환 업데이트!!
        if (continuousBullets.Count > 0)
        {
            var toRemove = new List<(int x, int y)>();
            var toAdd = new List<(int x, int y)>();

            foreach (var pair in continuousBullets)
            {
                var b = pair.Key;
                int count = pair.Value;

                if (count > 0)
                {
                    toAdd.Add((b.x, b.y));
                    continuousBullets[b] = count - 1;
                }
                else
                {
                    toRemove.Add(b);
                }
            }

            foreach (var b in toRemove)
            {
                continuousBullets.Remove(b);
            }

            bullets.AddRange(toAdd);
        }
    }
}