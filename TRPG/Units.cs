using Game.Interfaces;
using Game.Enums;
using Game.Data.Models;
using System.Transactions;
using Game.Actions;
using Game.Data;
using Utils;
using Game.Battle;

namespace Game.Units
{
    public class Stat
    {
        public int BaseStat { get; private set; }
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

    public class Player : Unit, IUsableSkill
    {
        private int level = 1;
        private int exp = 0;
        public int MaxMp { get; private set; }
        public int CurrentMp { get; private set; }
        public string JobId { get; }
        public string[] SkillIds { get; private set; }
        public BuffController BuffController { get; }

        public Player(string name, JobData job) : base(name, job.MaxHpBonus, job.AtkBonus, job.DefBonus, job.SpdBonus, job.CritBonus)
        {
            JobId = job.Id;
            SkillIds = job.SkillIds;
            MaxMp = job.MaxMpBonus;
            CurrentMp = MaxMp;
            BuffController = new BuffController();
        }

        public override void Die()
        {
            Console.WriteLine("당신은 전투에서 패배했습니다...");
            State = UnitState.Dead;
        }

        public void GainExp(int amount)
        {
            exp += amount;
            Console.WriteLine($"{amount}의 경험치를 획득했다!");
            CheckLevelUp();
            Console.WriteLine($"현재 경험치: {exp}/{ExpTable.GetExpForLevel(level + 1)}");
        }

        private void CheckLevelUp()
        {
            while (exp >= ExpTable.GetExpForLevel(level + 1))
            {
                exp -= ExpTable.GetExpForLevel(level + 1);
                level++;
                Console.WriteLine($"레벨업! 현재 레벨: {level}");
                MaxHp.UpdateStatByLevel(level);
                Atk.UpdateStatByLevel(level);
                Def.UpdateStatByLevel(level);
                Spd.UpdateStatByLevel(level);
                Crit.UpdateStatByLevel(level);
            }
        }

        public void HealAfterStageClear()
        {
            int healAmount = (int)(MaxHp.TotalStat * 0.3);
            CurrentHp = Math.Min(MaxHp.TotalStat, CurrentHp + healAmount);
        }

        public bool UseSkill(int cost)
        {
            if (CurrentMp >= cost)
            {
                CurrentMp -= cost;
                return true;
            }

            Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없다.");
            return false;
        }

        public void TickBuffs()
        {
            BuffController.Tick();
        }
    }

    public class Monster : Unit
    {
        public MonsterData Data { get; }
        public int ExpReward { get; }

        public Monster(MonsterData monsterData, int stage)
            : base(monsterData.Id,
                    StageScaling.ScaleHp(monsterData.MaxHp, stage),
                    StageScaling.ScaleAtk(monsterData.Atk, stage),
                    StageScaling.ScaleDef(monsterData.Def, stage),
                    StageScaling.ScaleSpd(monsterData.Spd, stage),
                    monsterData.Crit)
        {
            Data = monsterData;
            ExpReward = StageScaling.ScaleExp(monsterData.Exp, stage);
        }

        public override void Die()
        {
            Console.WriteLine($"{Name}을(를) 물리쳤다!");
            State = UnitState.Dead;
        }
    }
}