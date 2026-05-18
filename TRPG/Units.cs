using Game.Interfaces;
using Game.Enums;
using Game.Data.Models;
using System.Transactions;
using Game.Actions;
using Game.Data;
using Utils;

namespace Game.Units
{
    public class Stat
    {

        // 직업과 레벨에 따른 기본 스탯
        public int BaseStat { get; private set; }

        // 원래 직업 초기 스탯
        private int growthStat;
        public int BonusStat { get; private set; }
        public int TotalStat { get { return BaseStat + BonusStat; } }

        public Stat(int baseStat)
        {
            BaseStat = baseStat;
            growthStat = baseStat;
            BonusStat = 0;
        }

        public void AddBonus(int bonus)
        {
            BonusStat += bonus;
        }

        public void RemoveBonus(int bonus)
        {
            BonusStat -= bonus;
        }

        // 레벨업 시 스탯 증가 계산
        public void UpdateStatByLevel(int level)
        {
            if (level > 1)
            {
                BaseStat = (int)(growthStat * (1 + 0.1 * (level - 1)));
            }
            else
            {
                BaseStat = growthStat;
            }
        }
    }
    public abstract class Unit
    {
        public string Name { get; protected set; }
        public Stat MaxHp { get; protected set; }
        public int CurrentHp { get; protected set; }
        public Stat Atk { get; protected set; }
        public Stat Def { get; protected set; }
        public Stat Spd { get; protected set; }
        public Stat Crit { get; protected set; }

        public UnitState State { get; protected set; }
        public Unit(string name, int maxHealth, int attack, int defense, int speed, int critical)
        {
            Name = name;
            MaxHp = new Stat(maxHealth);
            CurrentHp = MaxHp.TotalStat;
            Atk = new Stat(attack);
            Def = new Stat(defense);
            Spd = new Stat(speed);
            Crit = new Stat(critical);
            State = UnitState.Alive;
        }

        // 각 유닛은 턴마다 어떤 행동을 할지 결정한다.
        public void ExecuteBattleAction(BattleAction action, Unit target)
        {
            action.Execute(this, target);
        }

        public void TakeDamage(int dmg)
        {
            if (dmg == 0)
            {
                Console.WriteLine($"{Name}은(는)공격을 완전히 막아냈다.");
            }
            else
            {
                CurrentHp = Math.Max(0, CurrentHp - dmg);

                Console.WriteLine($"{Name}이(가) {dmg}의 피해를 입었다! 남은 HP: {CurrentHp}/{MaxHp.TotalStat}");
                
                if (CurrentHp <= 0)
                {
                    Die();
                }
            }
        }

        public virtual void Die()
        {
            State = UnitState.Dead;
            Console.WriteLine($"{Name}이(가) 사망했다.");
        }
    }

    public class Player : Unit
    {
        private int level = 1;
        private int exp = 0;
        public string JobId { get; }
        public Player(string name, JobData job) : base(name, job.MaxHpBonus, job.AtkBonus, job.DefBonus, job.SpdBonus, job.CritBonus) 
        {
            JobId = job.Id;
        }

        public override void Die()
        {
            Console.WriteLine($"당신은 전투에서 패배했습니다...");
            State = UnitState.Dead;
        }

        public void GainExp(int amount)
        {
            exp += amount;
            Console.WriteLine($"{amount}의 경험치를 획득했다!");
            CheckLevelUp();
            Console.WriteLine($"현재 경험치: {exp}/{ExpTable.GetExpForLevel(level+1)}");
        }
        private void CheckLevelUp()
        {
            while (exp >= ExpTable.GetExpForLevel(level + 1))
            {
                exp -= ExpTable.GetExpForLevel(level + 1);
                level++;
                Console.WriteLine($"레벨업! 현재 레벨: {level}");
                // 레벨업 시 스탯 증가
                MaxHp.UpdateStatByLevel(level);
                Atk.UpdateStatByLevel(level);
                Def.UpdateStatByLevel(level);
                Spd.UpdateStatByLevel(level);
                Crit.UpdateStatByLevel(level);
            }
        }

        // 아이템 구현을 아직 못해서 임시로 스테이지 클리어 시마다 체력 회복하는 보상 시스템을 만들었음.
        public void HealAfterStageClear()
        {
            int healAmount = (int)(MaxHp.TotalStat * 0.3); // 최대 체력의 30% 회복
            CurrentHp = Math.Min(MaxHp.TotalStat, CurrentHp + healAmount);
        }

    }

    public class Monster : Unit
    {
        // 몬스터 데이터 참조용 필드
        public MonsterData Data { get; }
        public int ExpReward { get; }
        public Monster(MonsterData monsterData, int stage)
                : base(monsterData.Id, 
                        Stagescaling.ScaleHp(monsterData.MaxHp, stage), 
                        Stagescaling.ScaleAtk(monsterData.Atk, stage), 
                        Stagescaling.ScaleDef(monsterData.Def, stage), 
                        Stagescaling.ScaleSpd(monsterData.Spd, stage), 
                        monsterData.Crit)
        {
            Data = monsterData;
            ExpReward = Stagescaling.ScaleExp(monsterData.Exp, stage);
        }

        public override void Die()
        {
            Console.WriteLine($"{Name}을(를) 물리쳤다!");
            State = UnitState.Dead;
        }
    }
}
