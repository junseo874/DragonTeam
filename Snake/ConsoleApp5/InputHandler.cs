using System;

class InputHandler
{
    public static void HandleInput()
    {
        if (!Console.KeyAvailable) return; // 키 입력이 없으면 리턴

        var key = Console.ReadKey(true).Key; // 키 입력 받기

        // 플레이어 방향 설정
        switch (key)
        {
            case ConsoleKey.UpArrow when Player.Instance.DirY != 1:
                Player.Instance.DirX = 0;
                Player.Instance.DirY = -1;
                break;

            case ConsoleKey.DownArrow when Player.Instance.DirY != -1:
                Player.Instance.DirX = 0;
                Player.Instance.DirY = 1;
                break;

            case ConsoleKey.LeftArrow when Player.Instance.DirX != 1:
                Player.Instance.DirX = -1;
                Player.Instance.DirY = 0;
                break;

            case ConsoleKey.RightArrow when Player.Instance.DirX != -1:
                Player.Instance.DirX = 1;
                Player.Instance.DirY = 0;
                break;

            case ConsoleKey.Spacebar: // 스페이스바: 탄환 발사
                ShootBullet();
                break;

            case ConsoleKey.C: // C: 복제체 생성
                SpawnClone();
                break;
        }
    }

    private static void ShootBullet()
    {
        if (Player.Instance.Body.Count > 1) // 몸통이 2개 이상일 때만 발사 가능
        {
            if (Player.Instance.Body.Count > SkillManager.Instance.FixedBodyLength)
            {
                int bulletCount = 1;

                // DoubleShot 스킬이 있는 경우 탄환 수 증가
                if (SkillManager.Instance.HasSkill(SkillType.DoubleShot))
                {
                    int doubleShotLevel = SkillManager.Instance.GetSkillLevel(SkillType.DoubleShot);
                    bulletCount = doubleShotLevel + 1;
                    bulletCount = Math.Min(bulletCount, 7); // 최대 7개로 제한
                }

                int half = bulletCount / 2;

                for (int i = 0; i < bulletCount; i++)
                {
                    int offsetX = i - half;
                    BulletManager.Instance.AddBullet(Player.Instance.Body[0].x + offsetX, Player.Instance.Body[0].y);
                }

                // ContinuousShot 스킬이 있는 경우 연속 발사 추가
                if (SkillManager.Instance.HasSkill(SkillType.ContinuousShot))
                {
                    int continuousShotLevel = SkillManager.Instance.GetSkillLevel(SkillType.ContinuousShot);
                    var bullets = BulletManager.Instance.GetBullets();
                    if (bullets != null) // Null 체크 추가
                    {
                        foreach (var bullet in bullets)
                        {
                            BulletManager.Instance.AddContinuousBullet(bullet, continuousShotLevel);
                        }
                    }
                }

                // 탄환 발사 시 몸통 줄이기
                Player.Instance.Body.RemoveAt(Player.Instance.Body.Count - 1);
            }
        }
    }

    private static void SpawnClone()
    {
        if (SkillManager.Instance.HasSkill(SkillType.Clone) && CloneManager.Instance.Cooldown <= 0)
        {
            bool spawned = false;

            for (int dy = -1; dy <= 1 && !spawned; dy++)
            {
                for (int dx = -1; dx <= 1 && !spawned; dx++)
                {
                    int nx = Player.Instance.Body[0].x + dx;
                    int ny = Player.Instance.Body[0].y + dy;

                    if (nx > 0 && nx < GameManager.Width - 1 &&
                        ny > 0 && ny < GameManager.Height - 1 &&
                        MapManager.Instance.GetCharAt(ny, nx) == ' ')
                    {
                        CloneManager.Instance.SpawnClone(nx, ny);
                        CloneManager.Instance.Cooldown = 100; // 쿨다운 설정
                        spawned = true;
                    }
                }
            }
        }
    }
}