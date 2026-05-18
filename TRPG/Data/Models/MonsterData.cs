using System;
using Game.Interfaces;

namespace Game.Data.Models
{
    [Serializable]
    public class MonsterData : IHasId
    {
        public string Id { get; set; }
        public int MaxHp { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Spd { get; set; }
        public int Crit { get; set; }
        public int Exp { get; set; }

        public string[] Sprite { get; set; }
    }
}
