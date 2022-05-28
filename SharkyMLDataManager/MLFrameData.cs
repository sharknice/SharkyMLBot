using Sharky;

namespace SharkyMLDataManager
{
    public class MLFrameData
    {
        public Dictionary<int, string>? BuildHistory { get; set; }
        public Dictionary<int, string>? EnemyStrategyHistory { get; set; }
        public List<string>? ActiveEnemyStrategies { get; set; }
        public List<string>? DetectedEnemyStrategies { get; set; }
        public Dictionary<UnitTypes, int>? SelfUnitCounts { get; set; }
        public Dictionary<UnitTypes, int>? EnemyUnitCounts { get; set; }
        public MLBaseData? SelfBaseData { get; set; }
        public MLBaseData? EnemyBaseData { get; set; }
    }
}
