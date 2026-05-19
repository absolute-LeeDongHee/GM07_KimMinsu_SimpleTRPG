using System.Collections.Generic;
using System.ComponentModel;
using Game.Units;

namespace Game.Battle
{
    public class BuffController
    {
        private class ActiveBuff
        {
            public Stat TargetStat { get; }
            public int Amount { get; }
            public int RemainingTurns { get; private set; }
            public string SkillName { get; }

            public ActiveBuff(Stat targetStat, int amount, int duration, string skillName)
            {
                TargetStat = targetStat;
                Amount = amount;
                RemainingTurns = duration;
                SkillName = skillName;
            }

            public void Tick()
            {
                RemainingTurns--;
            }

            public void RefreshDuration(int duration)
            {
                RemainingTurns += duration;
            }
        }

        private List<ActiveBuff> activeBuffs = new List<ActiveBuff>();

        public void AddBuff(Stat targetStat, int amount, int duration, string skillName)
        {
            ActiveBuff? existBuff = activeBuffs.Find(buff => buff.SkillName == skillName);
            if (duration <= 0 || amount == 0)
            {
                return;
            }

            if (existBuff != null)
            {
                existBuff.RefreshDuration(duration+1);
                return;
            }

            // 턴이 끝날 때마다 버프 지속 시간이 감소됨.
            // 문제는 버프가 적용된 턴에도 지속 시간이 감소되어서
            // +1을 해서 적용된 턴에는 감소되지 않도록 처리함.
            targetStat.AddBonus(amount);
            activeBuffs.Add(new ActiveBuff(targetStat, amount, duration+1, skillName));
            Console.WriteLine($"{skillName} 효과 적용: +{amount}, {duration}턴");
        }

        public void Tick()
        {
            for (int i = activeBuffs.Count - 1; i >= 0; i--)
            {
                ActiveBuff buff = activeBuffs[i];
                buff.Tick();

                if (buff.RemainingTurns <= 0)
                {
                    buff.TargetStat.RemoveBonus(buff.Amount);
                    Console.WriteLine($"{buff.SkillName} 효과가 종료되었습니다.");
                    activeBuffs.RemoveAt(i);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}