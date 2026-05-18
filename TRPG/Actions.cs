using Game.Interfaces;
using Game.Units;
using Utils;

namespace Game.Actions
{
    public abstract class BattleAction
    {
        public virtual void Execute(Unit actor, Unit target)
        {            
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

    public class UseSkill : BattleAction, IRequiresTarget
    {
        public override void Execute(Unit actor, Unit target)
        {
            Console.WriteLine($"{actor.Name}이(가) 스킬을 사용했다!");
            // 원래는 스킬 선택하고 스킬이 발동되어야 함...
        }
    }

    public class UseItem : BattleAction
    {
        public override void Execute(Unit actor, Unit target)
        {
            Console.WriteLine($"{actor.Name}이(가) 아이템을 사용했다!");
            // 아이템 선택이 되고 아이템 효과가 발동되어야 함
        }
    }
}
