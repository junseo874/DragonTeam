using System;
using System.Collections.Generic;

class BossManager
{
    public static BossManager Instance = new BossManager();
    private bool bossActive = false;
    private (int x, int y) bossPosition = (0, 0);
    private int bossHealth = 0;
    private int bossSkillCooldown = 0;
    private List<(int x, int y, int dirX, int dirY)> bossBullets = new List<(int x, int y, int dirX, int dirY)>();
    private List<(int x, int y, int dirX, int dirY, int step)> chargeAttacks = new List<(int x, int y, int dirX, int dirY, int step)>();
    private Random rand = new Random();

    private BossManager() { }

    public void Initialize()
    {
        bossActive = false;
        bossHealth = 0;
        bossPosition = (0, 0);
        bossSkillCooldown = 0;
        bossBullets.Clear();
        chargeAttacks.Clear();
    }

    public void Update()
    {
        if (!bossActive && GameManager.Instance.Tick >= 1000)
        {
            SpawnBoss();
        }
        else if (bossActive)
        {
            UpdateBossBullets();
            UpdateChargeAttacks();
            if (bossSkillCooldown > 0)
                bossSkillCooldown--;
            if (bossSkillCooldown == 0)
            {
                UseRandomBossSkill();
                bossSkillCooldown = 50;
            }
        }
    }

    private void SpawnBoss()
    {
        bossActive = true;
        bossHealth = 30;
        int centerX = GameManager.Width / 2;
        int centerY = 3;

        // 맵 초기화
        for (int y = 0; y < GameManager.Height; y++)
        {
            for (int x = 0; x < GameManager.Width; x++)
            {
                if (MapManager.Instance.GetCharAt(y, x) == '#')
                    MapManager.Instance.SetCharAt(y, x, ' ');
            }
        }

        // 벽 재설정
        for (int y = 0; y < GameManager.Height; y++)
        {
            MapManager.Instance.SetCharAt(y, 0, '#');
            MapManager.Instance.SetCharAt(y, GameManager.Width - 1, '#');
        }
        for (int x = 0; x < GameManager.Width; x++)
        {
            MapManager.Instance.SetCharAt(0, x, '#');
            MapManager.Instance.SetCharAt(GameManager.Height - 1, x, '#');
        }

        // 보스 그리기
        for (int dy = -3; dy <= 2; dy++)
        {
            for (int dx = -3; dx <= 3; dx++)
            {
                double dist = Math.Sqrt(dx * dx + (dy * 2) * (dy * 2));
                if (dist < 3.5)
                {
                    int drawX = centerX + dx;
                    int drawY = centerY + dy;
                    if (drawX > 0 && drawX < GameManager.Width - 1 && drawY > 0 && drawY < GameManager.Height - 1)
                        MapManager.Instance.SetCharAt(drawY, drawX, 'B');
                }
            }
        }

        bossPosition = (centerX, centerY);
    }

    private void UseRandomBossSkill()
    {
        int skill = rand.Next(2); // 0 또는 1
        switch (skill)
        {
            case 0: // 360도 탄환 발사
                int bulletCount = 32;
                double angleStep = Math.PI * 2 / bulletCount;
                double radius = 8.0;
                for (int i = 0; i < bulletCount; i++)
                {
                    double angle = angleStep * i;
                    int dx = (int)Math.Round(Math.Cos(angle));
                    int dy = (int)Math.Round(Math.Sin(angle));
                    int startX = bossPosition.x + (int)(Math.Cos(angle) * radius);
                    int startY = bossPosition.y + (int)(Math.Sin(angle) * radius * 0.5f);
                    bossBullets.Add((startX, startY, dx, dy));
                }
                break;

            case 1: // 돌진 스킬
                int bossX = bossPosition.x;
                int bossY = bossPosition.y;
                int targetX = Player.Instance.Body[0].x;
                int targetY = Player.Instance.Body[0].y;
                int dx1 = 0, dy1 = 0;

                if (targetX > bossX) dx1 = 1;
                else if (targetX < bossX) dx1 = -1;
                if (targetY > bossY) dy1 = 1;
                else if (targetY < bossY) dy1 = -1;

                int randomStep = rand.Next(10, 31);
                for (int step = 1; step <= randomStep; step++)
                {
                    int cx = bossX + dx1 * step;
                    int cy = bossY + dy1 * step;
                    if (cx > 0 && cx < GameManager.Width - 1 && cy > 0 && cy < GameManager.Height - 1)
                        MapManager.Instance.SetCharAt(cy, cx, '+');
                }

                chargeAttacks.Add((bossX, bossY, dx1, dy1, randomStep));
                break;
        }
    }

    private void UpdateBossBullets()
    {
        for (int i = bossBullets.Count - 1; i >= 0; i--)
        {
            var (x, y, dx, dy) = bossBullets[i];
            int nx = x + dx;
            int ny = y + dy;

            if (nx <= 0 || nx >= GameManager.Width - 1 || ny <= 0 || ny >= GameManager.Height - 1)
            {
                bossBullets.RemoveAt(i);
                continue;
            }

            if (Player.Instance.Body.Exists(part => part.x == nx && part.y == ny))
            {
                GameManager.Instance.IsRunning = false;
                return;
            }

            bossBullets[i] = (nx, ny, dx, dy);

            for (int j = 1; j < Player.Instance.Body.Count; j++)
            {
                if (Player.Instance.Body[j].x == nx && Player.Instance.Body[j].y == ny)
                {
                    Player.Instance.Body.RemoveRange(j, Player.Instance.Body.Count - j);
                    bossBullets.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private void UpdateChargeAttacks()
    {
        for (int i = chargeAttacks.Count - 1; i >= 0; i--)
        {
            var (x, y, dirX, dirY, step) = chargeAttacks[i];

            if (step > 0)
            {
                chargeAttacks[i] = (x, y, dirX, dirY, step - 1);
            }
            else
            {
                int nx = x + dirX;
                int ny = y + dirY;
                bool hitWall = false;

                for (int dy = -3; dy <= 2; dy++)
                {
                    for (int dx = -3; dx <= 3; dx++)
                    {
                        double dist = Math.Sqrt(dx * dx + (dy * 2) * (dy * 2));
                        if (dist < 3.5)
                        {
                            int checkX = nx + dx;
                            int checkY = ny + dy;
                            if (checkX <= 0 || checkX >= GameManager.Width - 1 || checkY <= 0 || checkY >= GameManager.Height - 1)
                            {
                                hitWall = true;
                                break;
                            }
                            if (MapManager.Instance.GetCharAt(checkY, checkX) == '#')
                                MapManager.Instance.SetCharAt(checkY, checkX, ' ');
                        }
                    }
                    if (hitWall) break;
                }

                if (hitWall)
                {
                    chargeAttacks.RemoveAt(i);
                    continue;
                }

                if (Player.Instance.Body.Exists(part => part.x == nx && part.y == ny))
                {
                    GameManager.Instance.IsRunning = false;
                    return;
                }

                for (int j = 1; j < Player.Instance.Body.Count; j++)
                {
                    if (Player.Instance.Body[j].x == nx && Player.Instance.Body[j].y == ny)
                    {
                        Player.Instance.Body.RemoveRange(j, Player.Instance.Body.Count - j);
                        break;
                    }
                }

                DrawBossAt(nx, ny);

                if (step == 0)
                {
                    int newStep = -1;
                    chargeAttacks[i] = (nx, ny, dirX, dirY, newStep);
                }
                else if (step > -10)
                {
                    chargeAttacks[i] = (nx, ny, dirX, dirY, step - 1);
                }
                else
                {
                    chargeAttacks.RemoveAt(i);
                }
            }
        }
    }

    private void DrawBossAt(int x, int y)
    {
        ClearBossArea();

        for (int dy = -3; dy <= 2; dy++)
        {
            for (int dx = -3; dx <= 3; dx++)
            {
                double dist = Math.Sqrt(dx * dx + (dy * 2) * (dy * 2));
                if (dist < 3.5)
                {
                    int bx = x + dx;
                    int by = y + dy;
                    if (bx > 0 && bx < GameManager.Width - 1 && by > 0 && by < GameManager.Height - 1)
                        MapManager.Instance.SetCharAt(by, bx, 'B');
                }
            }
        }

        bossPosition = (x, y);
    }

    private void ClearBossArea()
    {
        int centerX = bossPosition.x;
        int centerY = bossPosition.y;

        for (int dy = -3; dy <= 2; dy++)
        {
            for (int dx = -3; dx <= 3; dx++)
            {
                double dist = Math.Sqrt(dx * dx + (dy * 2) * (dy * 2));
                if (dist < 3.5)
                {
                    int bx = centerX + dx;
                    int by = centerY + dy;
                    MapManager.Instance.SetCharAt(by, bx, ' ');
                }
            }
        }
    }

    public void RenderBossHealthBar()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write($"BOSS HP: ");
        for (int i = 0; i < bossHealth; i++) Console.Write("❤");
        for (int i = bossHealth; i < 30; i++) Console.Write("♡");
    }

    public void CheckBossHit()
    {
        if (!bossActive || bossHealth <= 0) return;

        int centerX = bossPosition.x;
        int centerY = bossPosition.y;

        List<(int x, int y)> bossArea = new List<(int x, int y)>();

        for (int dy = -3; dy <= 2; dy++)
        {
            for (int dx = -3; dx <= 3; dx++)
            {
                double dist = Math.Sqrt(dx * dx + (dy * 2) * (dy * 2));
                if (dist < 3.5)
                {
                    int bx = centerX + dx;
                    int by = centerY + dy;
                    bossArea.Add((bx, by));
                }
            }
        }

        for (int i = 0; i < Player.Instance.Body.Count; i++)
        {
            var part = Player.Instance.Body[i];
            if (bossArea.Contains((part.x, part.y)))
            {
                if (i == 0)
                {
                    GameManager.Instance.IsRunning = false;
                    return;
                }
                else
                {
                    Player.Instance.Body.RemoveRange(i, Player.Instance.Body.Count - i);
                    break;
                }
            }
        }
    }
}