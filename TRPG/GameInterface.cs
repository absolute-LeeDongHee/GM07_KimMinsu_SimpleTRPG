using Game.Units;

namespace Game.Interfaces
{
    #region 유닛 관련 인터페이스
    public interface IAttackable
    {
        public void Attack(Unit target);
    }
    public interface IDamageable
    {
        void TakeDamage(int amount);
    }

    public interface ISpritable
    {
        void ShowSprite();
    }
    #endregion

    #region 아이템 관련 인터페이스
    public interface IUsable
    {
        void Use();
    }

    public interface IEquipable
    {
        void Equip();
        void Unequip();
    }
    #endregion

    #region 데이터 로드 관련 인터페이스
    public interface IHasId
    {
        string Id { get; }
    }
    #endregion

    #region 전투 행동 관련 인터페이스
    public interface IRequiresTarget
    {
    }
    #endregion
}
