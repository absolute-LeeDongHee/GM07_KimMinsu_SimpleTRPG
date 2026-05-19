using Game.Units;

namespace Utils
{
    #region 전투 관련 유틸리티
    public static class BattleUtils
    {
        private static Random rand = new Random();
        
        public static int CalculateDmg(Unit attacker, Unit defender, bool isCritical)
        {
            int damage = Math.Max(0, attacker.Atk.TotalStat - defender.Def.TotalStat);
            if (isCritical)
            {
                damage = (int)(damage * 1.5f);
            }
            return damage;
        }

        public static int SkillDamage(Unit attacker, Unit defender, double? powerMultiplier, bool isCritical)
        {
            // 가독성을 위해 스킬 공격은 데미지를 먼저 계산.
            int damage = (int)(attacker.Atk.TotalStat * powerMultiplier) - defender.Def.TotalStat;

            if (isCritical)
            {
                damage = (int)(damage * 1.5f);
            }

            return Math.Max(0, damage);
        }

        public static bool IsCriticalHit(Unit attacker)
        {
            return rand.Next(0, 100) < attacker.Crit.TotalStat;
        }
    }
    #endregion
    #region 몬스터 생성 관련 유틸리티
    public static class StageScaling
    {
        public static int ScaleHp(int baseValue, int stage)
        {
            return ScaleStat(baseValue, stage, 0.08);
        }

        public static int ScaleAtk(int baseValue, int stage)
        {
            return ScaleStat(baseValue, stage, 0.05);
        }

        public static int ScaleDef(int baseValue, int stage)
        {
            return ScaleStat(baseValue, stage, 0.04);
        }

        public static int ScaleSpd(int baseValue, int stage)
            {
                return ScaleStat(baseValue, stage, 0.02);
        }

        public static int ScaleExp(int baseValue, int stage)
        {
            return ScaleStat(baseValue, stage, 0.10);
        }

        // 스테이지에 따른 몬스터 스탯 증가 로직
        private static int ScaleStat(int baseValue, int stage, double growthRate)
        {
            double scale = 1.0 + (stage - 1) * growthRate;
            return Math.Max(1, (int)(baseValue * scale));
        }
    }
    #endregion

}

