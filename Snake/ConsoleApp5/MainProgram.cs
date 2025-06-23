using System;

class MainProgram
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // UTF-8 인코딩 설정
        Console.CursorVisible = false; // 커서 숨기기
        Console.SetWindowSize(GameManager.Width + 10, GameManager.Height + 10); // 창 크기 설정

        while (true)
        {
            // 시작 화면 표시
            GameManager.Instance.ShowStartScreen();

            // 게임 초기화
            GameManager.Instance.InitGame();

            // 테스트용 탄환 추가 (필요 시 주석 처리 가능)
            BulletManager.Instance.AddBullet(10, 20);
            BulletManager.Instance.AddBullet(15, 25);

            // 게임 루프
            while (GameManager.Instance.IsRunning)
            {
                // 입력 처리
                InputHandler.HandleInput();

                // 게임 업데이트
                GameManager.Instance.Update();

                // 레벨업 체크
                GameManager.Instance.CheckLevelUp();

                // 화면 렌더링
                Renderer.Render();

                // 스피드 스킬 적용
                int currentSpeedLevel = SkillManager.Instance.GetSkillLevel(SkillType.SpeedUp);
                int effectiveDelay = Math.Max(1, 100 - currentSpeedLevel * 20); // 최소 1ms
                System.Threading.Thread.Sleep(effectiveDelay);

                // 틱 증가
                GameManager.Instance.Tick++;
            }

            // 게임 오버 화면 표시!!
            if (!GameManager.Instance.ShowGameOver()) break;
        }
    }
}