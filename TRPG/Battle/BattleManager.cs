using Game.Units;
using Game.Actions;
using Game.Enums;
using Game.Interfaces;

namespace Game.Battle
{
    /*
    [BattleManager]
    전반적인 전투 로직을 관리하는 클래스
    */
    class BattleManager
    {
        private Player player;
        private List<Monster> currentEnemies; // 현재 전투에 참여 중인 적 리스트
        private TurnManager turnManager;
        private Func<BattleAction>? playerActionFunc; // 플레이어 행동 선택 함수.
        private Func<List<Monster>, Monster?> selectTargetFunc; // 공격 대상 선택 함수. 선택한 타겟을 BattleScreen에서 전달받아서 처리.

        public BattleManager(Player player, List<Monster> enemies,
            Func<BattleAction> playerActionFunc,
            Func<List<Monster>, Monster?> selectTargetFunc)
        {
            this.player = player;
            this.currentEnemies = enemies;
            turnManager = new TurnManager();
            this.playerActionFunc = playerActionFunc;
            this.selectTargetFunc = selectTargetFunc;
        }

        public void StartBattle()
        {
            // 전투 시작 순서를 정하기 위해 플레이어와 적들을 Unit 타입으로 묶어서 리스트에 추가
            // 그 후 TurnManager의 TurnSetup 메서드에 전달하여 턴 순서를 결정
            List<Unit> units = new List<Unit>();
            units.Add(player);
            units.AddRange(currentEnemies);
            turnManager.TurnSetup(units);

            // 간단한 턴제 전투 로직
            while (player.State == UnitState.Alive && currentEnemies.Count > 0)
            {
                // 턴을 진행할 유닛을 TurnManager에서 가져온다
                Unit? currentUnit = turnManager.NextTurn();
                bool isActed = TurnProcess(currentUnit);

                if (isActed)
                {
                    turnManager.IncreaseActedCount();
                    
                }

                turnManager.CheckRoundFinished();

                if (turnManager.IsRoundFinished)
                {
                    // 라운드가 끝날 때마다 버프 지속 시간 감소
                    player.BuffController.Tick();
                }

                // 턴 종료 후 죽은 몬스터가 있는지 확인하여 TurnManager에서 제거
                foreach (Monster m in currentEnemies)
                {
                    if (m.State == UnitState.Dead)
                    {
                        turnManager.RemoveUnit(m);
                        player.GainExp(m.ExpReward);
                    }
                }
                currentEnemies.RemoveAll(m => m.State == UnitState.Dead);

                if (player.State == UnitState.Dead)
                {
                    break;
                }
                else if (currentEnemies.Count == 0)
                {
                    Console.WriteLine("모든 적을 처치했습니다! 승리!");
                    player.HealAfterStageClear();
                    break;
                }
            }
        }

        public bool TurnProcess(Unit? unit)
        {
            if (unit == null || unit.State == UnitState.Dead)
            {
                return false;
            }

            if (unit is Player p)
            {
                // 플레이어가 행동을 선택함.
                BattleAction? action = playerActionFunc?.Invoke();

                if (action == null)
                {
                    return false;
                }

                // 행동이 타겟이 필요한 행동인지 확인하여 타겟 선택 함수 호출
                if (action is IRequiresTarget)
                {
                    Monster? target = selectTargetFunc?.Invoke(currentEnemies);
                    if (target != null)
                    {
                        p.ExecuteBattleAction(action, target);
                        Console.WriteLine();
                        Thread.Sleep(1000);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (action is IAllTarget)
                {
                    List<Monster> targets = currentEnemies;
                    action.Execute(p, null, targets);
                    Console.WriteLine();
                    Thread.Sleep(1000);
                    return true;
                }
                // 타겟이 필요 없는 행동인 경우 바로 실행
                // 아마 포션 사용, 힐, 버프 행동 이런 애들이 될듯
                else
                {
                    p.ExecuteBattleAction(action, p);
                    Console.WriteLine();
                    Thread.Sleep(500);

                    return true;
                }
            }
            else if (unit is Monster monster)
            {
                // 몬스터의 행동은 일단 기본 공격으로...
                monster.ExecuteBattleAction(new BasicAttack(), player);
                Console.WriteLine();
                Thread.Sleep(1000);
                
                return true;
            }

            return false;
        }
    }
}