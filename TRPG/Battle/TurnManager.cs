using Game.Units;

namespace Game.Battle
{
    /*
    [TurnManager]
    전투 시 턴 순서를 관리하는 역할을 하는 클래스
     */
    class TurnManager
    {
        private Queue<Unit> turnQueue = new Queue<Unit>();
        private int actedCount = 0; // 현재 턴에서 행동한 유닛 수를 나타냄
        private int AliveUnitCount = 0; // 현재 턴에서 살아있는 유닛 수를 나타냄
        public bool IsRoundFinished { get; private set; } = false; // 라운드가 끝났는지 여부를 나타냄

        public void IncreaseActedCount()
        {
            actedCount++;
        }

        public void TurnSetup(List<Unit> units)
        {
            // 턴 순서를 스피드에 따라 결정
            // 리스트에 있는 유닛들을 스피드 순으로 정렬하여 큐에 넣는다 
            List<Unit> sortedUnits = units.OrderByDescending(u => u.Spd.TotalStat).ToList();
            turnQueue = new Queue<Unit>(sortedUnits);
            AliveUnitCount = turnQueue.Count;
        }

        // 다음 턴을 진행하는 메서드
        // 현재 턴인 유닛을 큐에서 꺼냄
        // 만약 큐가 비어있다면 null을 반환
        public Unit? NextTurn()
        {
            if (turnQueue.Count == 0)
            {
                return null; // 턴이 없으면 null 반환
            }

            Unit currentUnit = turnQueue.Dequeue();
            turnQueue.Enqueue(currentUnit);

            return currentUnit;
        }

        public void RemoveUnit(Unit unit)
        {
            Queue<Unit> newQueue = new Queue<Unit>();

            foreach (Unit u in turnQueue)
            {
                if (u != unit)
                {
                    newQueue.Enqueue(u);
                }
            }

            turnQueue = newQueue;
            AliveUnitCount = turnQueue.Count;
        }

        public void CheckRoundFinished()
        {
            if (actedCount >= AliveUnitCount)
            {
                IsRoundFinished = true;
                actedCount = 0; // 행동한 유닛 수 초기화
            }
            else
            {
                IsRoundFinished = false;
            }
        }
    }
}
