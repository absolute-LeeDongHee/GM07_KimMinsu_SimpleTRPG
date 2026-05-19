using System;
using Game.Interfaces;

namespace Game.Data.Models
{
    [Serializable]
    public class JobData : IHasId
    {
        public string Id { get; set; }
        public int MaxHpBonus { get; set; }

        public int MaxMpBonus { get; set; }
        public int AtkBonus { get; set; }
        public int DefBonus { get; set; }
        public int SpdBonus { get; set; }
        public int CritBonus { get; set; }
        public string[] SkillIds { get; set; }
    }
}
