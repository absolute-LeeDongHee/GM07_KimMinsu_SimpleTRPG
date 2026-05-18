using Game.Battle;
using Game.Data;
using Game.Data.Models;
using Game.Enums;
using Game.Screen;
using Game.Units;
using Game.Screen;

namespace Game
{
    public class GameApp
    {
        private Dictionary<string, JobData> jobs = new();
        private Dictionary<string, MonsterData> monsters = new();

        public void Run()
        {
            LoadData();
            bool isRunning = true;
            while (isRunning)
            {
                MainMenuScreen mainMenu = new MainMenuScreen();
                MainMenuOpt opt = mainMenu.Show();
                switch (opt)
                {
                    case MainMenuOpt.NewGame:
                        StartNewGame();
                        break;
                    case MainMenuOpt.Continue:
                        LoadGame();
                        break;
                    case MainMenuOpt.Exit:
                        isRunning = false;
                        break;
                }
            }
        }

        private void LoadData()
        {
            jobs = DataLoader.LoadData<JobData>("Data/Json/Jobs.json");
            monsters = DataLoader.LoadData<MonsterData>("Data/Json/Monsters.json");
        }

        private void StartNewGame()
        {
            NameInputScreen nameInput = new NameInputScreen();
            JobSelectScreen jobSelect = new JobSelectScreen();
            string name = nameInput.InputName();
            JobData selectedJob = jobSelect.Show(jobs);
            if (selectedJob != null)
            {
                Player player = new Player(name, selectedJob);
                StageManager stageManager = new StageManager(monsters);
                BattleScreen battleScreen = new BattleScreen(player, stageManager);
                battleScreen.ShowBattle();
            }
        }

        private void LoadGame()
        {
            Console.WriteLine("게임 불러오기 기능은 아직 구현되지 않았습니다.");
            Console.ReadKey();
        }
    }
}
