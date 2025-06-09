using System;

class Renderer
{
    public static void Render()
    {
        Console.SetCursorPosition(0, 0);

        // 맵 복제
        char[,] buffer = MapManager.Instance.CloneMap();

        // 플레이어 몸통 그리기
        foreach (var part in Player.Instance.Body)
        {
            buffer[part.y, part.x] = '@';
        }

        // 탄환 그리기
        foreach (var bullet in BulletManager.Instance.Bullets)
        {
            buffer[bullet.y, bullet.x] = '|';
        }

        // 화면 출력
        for (int y = 0; y < GameManager.Height; y++)
        {
            for (int x = 0; x < GameManager.Width; x++)
            {
                Console.Write(buffer[y, x] == '\0' ? ' ' : buffer[y, x]);
            }
            Console.WriteLine();
        }

        // 상태 정보 출력!
        Console.WriteLine($"EXP: {Player.Instance.Exp} | Level: {Player.Instance.Level} | Length: {Player.Instance.Body.Count} | Time: {GameManager.Instance.Tick / 10}s");
    }
}