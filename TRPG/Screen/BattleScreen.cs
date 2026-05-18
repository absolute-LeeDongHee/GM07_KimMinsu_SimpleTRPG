

using Game.Units;
using Game.Battle;
using Game.Enums;
using Game.Input;
using Game.Actions;

namespace Game.Screen
{
    public class BattleScreen
    {
        private Player player;
        private StageManager stageManager;
        private List<Monster> enemies = new();

        public BattleScreen(Player player, StageManager stageManager)
        {
            this.player = player;
            this.stageManager = stageManager;
        }

        public void ShowBattle()
        {
            while (player.State == UnitState.Alive)
            {
                enemies = stageManager.CreateEnemies();

                RenderBattle(enemies, -1); // 초기 렌더링, 없으면 스테이지 넘어가기 전에 선빵을 맞는 경우가 있었음...

                BattleManager battleManager = new BattleManager(player, enemies, SelectAction, SelectTarget);
                battleManager.StartBattle();

                if (player.State == UnitState.Dead)
                {
                    ShowGameOver();
                    break;
                }
                else
                {
                    ShowStageClear();
                    stageManager.NextStage();
                }
            }
        }

        private void ShowGameOver()
        {
            Console.WriteLine();
            Console.WriteLine("게임 오버");
            Console.WriteLine($"도달한 스테이지: {stageManager.CurrentStage}");
            Console.WriteLine("아무 키나 누르면 메인 메뉴로 돌아갑니다.");
            Console.ReadKey(true);
        }

        private void ShowStageClear()
        {
            Console.WriteLine($"스테이지 {stageManager.CurrentStage} 클리어!");
            Console.WriteLine("아무 키나 누르면 다음 스테이지로 진행합니다.");
            Console.ReadKey(true);
        }

        public BattleAction? SelectAction()
        {
            string[] actions = { "공격", "스킬", "아이템" };
            int selectedIndex = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=====스테이지 {stageManager.CurrentStage}=====");
                Console.WriteLine();
                
                RenderMonster(enemies, -1); // 행동 선택 시에는 적 선택이 아니므로 -1을 매개변수로 전달하여 커서가 보이지 않도록 함

                Console.WriteLine();
                Console.WriteLine("=================================================");
                Console.WriteLine($"{player.Name}");
                Console.WriteLine($"HP: {player.CurrentHp}/{player.MaxHp.TotalStat}");
                Console.WriteLine("=================================================");
                Console.WriteLine();

                for (int i = 0; i < actions.Length; i++)
                {
                    string cursor = i == selectedIndex ? ">" : " ";
                    Console.WriteLine($"{cursor} {actions[i]}");
                }
                KeyInput key = InputManager.ReadKeyboard();
                switch (key)
                {
                    case KeyInput.Up:
                        selectedIndex = (selectedIndex - 1 + actions.Length) % actions.Length;
                        break;
                    case KeyInput.Down:
                        selectedIndex = (selectedIndex + 1) % actions.Length;
                        break;
                    case KeyInput.Confirm:
                        BattleAction action = DecisionAction(selectedIndex);
                        return action;
                    case KeyInput.Cancel:
                        return null;
                }
            }
        }

        public Monster? SelectTarget(List<Monster> enemies)
        {
            int selectedIndex = 0;
            while (true)
            {
                RenderBattle(enemies, selectedIndex);

                KeyInput key = InputManager.ReadKeyboard();
                switch (key)
                {
                    case KeyInput.Left:
                        selectedIndex = (selectedIndex - 1 + enemies.Count) % enemies.Count;
                        break;
                    case KeyInput.Right:
                        selectedIndex = (selectedIndex + 1) % enemies.Count;
                        break;
                    case KeyInput.Confirm:
                        return enemies[selectedIndex];
                    case KeyInput.Cancel:
                        return null;
                }
            }
        }

        private BattleAction DecisionAction(int idx)
        {
            switch (idx)
            {
                case 0:
                    return new BasicAttack();
                case 1:
                    return new UseSkill();
                case 2:
                    // 아이템 선택 화면으로 이동
                    return new UseItem();
                default:
                    return null;
            }
        }

        private void RenderBattle(List<Monster> enemies, int idx)
        {
            Console.Clear();

            Console.WriteLine($"=====스테이지 {stageManager.CurrentStage}=====");
            Console.WriteLine();

            RenderMonster(enemies, idx);
            Console.WriteLine();

            Console.WriteLine("=================================================");
            Console.WriteLine($"{player.Name}");
            Console.WriteLine($"HP: {player.CurrentHp}/{player.MaxHp.TotalStat}");
            Console.WriteLine("=================================================");
        }

        // 전투 주인 적을 가로로 나열하여 렌더링하는 함수
        private void RenderMonster(List<Monster> enemies, int selectedIndex)
        {
            int columnWidth = 24;

            for (int i = 0; i < enemies.Count; i++)
            {
                string arrow = i == selectedIndex ? "v" : " ";
                Console.Write(CenterText(arrow, columnWidth));
            }
            Console.WriteLine();

            // 몬스터의 이름을 가운데 정렬하여 출력한다
            foreach (Monster monster in enemies)
            {
                Console.Write(CenterText(monster.Name, columnWidth));
            }
            Console.WriteLine();

            // HP도 가운데 정렬
            foreach (Monster monster in enemies)
            {
                string hpText = $"HP: {monster.CurrentHp}/{monster.MaxHp.TotalStat}";
                Console.Write(CenterText(hpText, columnWidth));
            }
            Console.WriteLine();

            Console.WriteLine();

            // 몬스터 스프라이트 렌더링
            int maxSpriteHeight = enemies.Max(m => m.Data.Sprite.Length);

            for (int lineIndex = 0; lineIndex < maxSpriteHeight; lineIndex++)
            {
                foreach (Monster monster in enemies)
                {
                    string line = "";

                    if (lineIndex < monster.Data.Sprite.Length)
                    {
                        line = monster.Data.Sprite[lineIndex];
                    }

                    Console.Write(CenterText(line, columnWidth));
                }

                Console.WriteLine();
            }
        }

        // 가운데 정렬
        private string CenterText(string text, int width)
        {
            if (text.Length >= width)
                return text.Substring(0, width);

            int leftPadding = (width - text.Length) / 2;
            int rightPadding = width - text.Length - leftPadding;

            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }


    }
}
