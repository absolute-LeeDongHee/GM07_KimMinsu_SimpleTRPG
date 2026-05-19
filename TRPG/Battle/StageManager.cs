using Game.Data.Models;
using Game.Units;
using System.Runtime.InteropServices;

namespace Game.Battle
{
    public class StageManager
    {
        private Dictionary<string, MonsterData> monsterDataDict;
        private readonly Dictionary<int, string[]> stageMonsterTable = new()
        {
            { 1, new[] { "Cheeze Slime" } },
            { 2, new[] { "Cheeze Slime", "Cheeze Slime" } },
            { 3, new[] { "Trap Bat" } },
            { 4, new[] { "Cheeze Slime", "Trap Bat" } },
            { 5, new[] { "Cat Witch" } },
            { 6, new[] { "Cheeze Slime", "Trap Bat", "Cheeze Slime" } },
            { 7, new[] { "Trap Bat", "Cat Witch" } },
            { 8, new[] { "Trap Bat", "Sewer Golem" } },
            { 9, new[] { "Trap Bat", "Cat Witch", "Cheeze Slime" } },
            { 10, new[] { "Cat Witch", "Sewer Golem" } },
        };

        public int CurrentStage { get; private set; } = 1;

        public StageManager(Dictionary<string, MonsterData> monsterDataDict)
        {
            this.monsterDataDict = monsterDataDict;
        }

        public List<Monster> CreateEnemies()
        {
            int stage = GetStagePattern();
            string[] monsterNames = stageMonsterTable[stage];

            List<Monster> monsters = new List<Monster>();
            foreach (string monsterId in monsterNames)
            {
                MonsterData monsterData = monsterDataDict[monsterId];
                Monster monster = new Monster(monsterData, CurrentStage);

                monsters.Add(monster);
            }

            return monsters;
        }
        public void NextStage()
        {
            CurrentStage++;
        }

        // 스테이지별로 정해진 몬스터 생성 패턴 key를 반환하는 메서드
        private int GetStagePattern()
        {
            return ((CurrentStage - 1) % stageMonsterTable.Count) + 1;
        }
    }
}
