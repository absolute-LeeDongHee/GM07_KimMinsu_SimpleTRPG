using System;
using Game.Interfaces;

namespace Game.Data.Models
{
    [Serializable]
    public class SkillData : IHasId
    {
        public string Id { get; set; }
        public string SkillType { get; set; }
        public string Description { get; set; }
        public double? PowerMultiplier { get; set; }
        public double? DefMultiplier { get; set; }
        public double? CritMultiplier { get; set; }
        public int Cost { get; set; }
        public string TargetType { get; set; }
        public int? Duration { get; set; }
    }
}