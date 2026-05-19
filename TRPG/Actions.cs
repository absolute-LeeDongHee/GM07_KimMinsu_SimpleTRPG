using Game.Data.Models;
using Game.Interfaces;
using Game.Units;
using System.Numerics;
using Utils;

namespace Game.Actions
{
    public abstract class BattleAction
    {
        public virtual void Execute(Unit actor, Unit target)
        {
            // 단일 공격, 버프
        }

        public virtual void Execute(Unit actor, Unit target, List<Monster> enemies)
        {
            // 범위 공격
        }
    }

    public class BasicAttack : BattleAction, IRequiresTarget
    {
        public override void Execute(Unit actor, Unit target)
        {
            bool isCritical = BattleUtils.IsCriticalHit(actor);
            int dmg = BattleUtils.CalculateDmg(actor, target, isCritical);

            if (isCritical)
            {
                Console.WriteLine($"{actor.Name}의 치명타 공격!");
            }
            else
            {
                Console.WriteLine($"{actor.Name}의 공격!");
            }

            target.TakeDamage(dmg);
        }
    }

    public class SingleTargetAttack : BattleAction, IRequiresTarget
    {
        private SkillData skillData;
        public SingleTargetAttack(SkillData skillData)
        {
            this.skillData = skillData;
        }
        public override void Execute(Unit actor, Unit target)
        {

            if (actor is Player player)
            {
                if (!player.UseSkill(skillData.Cost))
                {
                    Console.WriteLine($"{actor.Name}은(는) MP가 부족하여 {skillData.Id}을(를) 사용할 수 없다!");
                    return;
                }
                bool isCritical = BattleUtils.IsCriticalHit(actor);
                Console.WriteLine($"{actor.Name}이(가 {skillData.Id}를 사용했다!");
                if (isCritical)
                {
                    Console.WriteLine($"{actor.Name}의 치명타!");
                }
                int dmg = BattleUtils.SkillDamage(actor, target, skillData.PowerMultiplier, isCritical);
                target.TakeDamage(dmg);
            }
            return;
        }
    }

    public class AllTargetAttack : BattleAction, IAllTarget
    {
        private SkillData skillData;
        public AllTargetAttack(SkillData skillData)
        {
            this.skillData = skillData;
        }
        public override void Execute(Unit actor, Unit target, List<Monster> enemies)
        {
            if (actor is Player player)
            {
                if (!player.UseSkill(skillData.Cost))
                {
                    Console.WriteLine($"{actor.Name}은(는) MP가 부족하여 {skillData.Id}을(를) 사용할 수 없다!");
                    return;
                }
                Console.WriteLine($"{actor.Name}이(가 {skillData.Id}를 사용했다!");
                bool isCritical = BattleUtils.IsCriticalHit(actor);
                Console.WriteLine($"{actor.Name}이(가 {skillData.Id}를 사용했다!");
                if (isCritical)
                {
                    Console.WriteLine($"{actor.Name}의 치명타!");
                }
                int dmg = (int)(actor.Atk.TotalStat * skillData.PowerMultiplier);
                foreach (Monster m in enemies)
                {
                    m.TakeDamage(dmg);
                }
            }
        }
    }

    public class BuffSkillAction : BattleAction
    {
        private readonly SkillData skillData;

        public BuffSkillAction(SkillData skillData)
        {
            this.skillData = skillData;
        }

        public override void Execute(Unit actor, Unit target)
        {
            if (actor is not Player player || !player.UseSkill(skillData.Cost))
            {
                Console.WriteLine($"{actor.Name}은(는) MP가 부족하여 {skillData.Id}을(를) 사용할 수 없다!");
                return;
            }

            Console.WriteLine($"{actor.Name}이(가) {skillData.Id}을(를) 사용했다!");
            ApplyBuff(player);
        }

        private void ApplyBuff(Player player)
        {
            // SkillData 클래스의 필드는 모든 스킬 데이터를 보관해야 해서 
            // 특정 스킬에만 사용하는 필드까지 포함되어 있음.
            // 이 경우에는 사용하지 않는 필드를 null로 설정하는데
            // null은 0을 의미하는 것이 아니기 때문에 null 병합 연산자로 0을 기본값으로 설정해서 처리.
            int duration = skillData.Duration ?? 0;
            if (duration <= 0)
            {
                return;
            }

            if (skillData.DefMultiplier.HasValue)
            {
                int amount = (int)(player.Def.BaseStat * (skillData.DefMultiplier.Value - 1.0));
                player.BuffController.AddBuff(player.Def, amount, duration, skillData.Id);
            }

            if (skillData.CritMultiplier.HasValue)
            {
                int amount = (int)(player.Crit.BaseStat * (skillData.CritMultiplier.Value - 1.0));
                player.BuffController.AddBuff(player.Crit, amount, duration, skillData.Id);
            }
        }
    }

    public class UseItem : BattleAction
    {
        public override void Execute(Unit actor, Unit target)
        {
            Console.WriteLine($"{actor.Name}이(가) 아이템을 사용했다!");
        }
    }
}