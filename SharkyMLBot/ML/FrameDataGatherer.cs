using SC2APIProtocol;
using Sharky;
using Sharky.DefaultBot;

namespace SharkyMLBot.ML
{
    public class FrameDataGatherer
    {
        EnemyData EnemyData;
        BaseData BaseData;
        ActiveUnitData ActiveUnitData;

        UnitCountService UnitCountService;

        public FrameDataGatherer(DefaultSharkyBot defaultSharkyBot)
        {
            EnemyData = defaultSharkyBot.EnemyData;
            BaseData = defaultSharkyBot.BaseData;
            ActiveUnitData = defaultSharkyBot.ActiveUnitData;

            UnitCountService = defaultSharkyBot.UnitCountService;
        }

        public MLFrameData GetFrameData(Dictionary<int, string> buildHistory, Dictionary<int, string> enemyStrategyHistory)
        {
            var activeEnemyStrategies = EnemyData.EnemyStrategies.Values.Where(s => s.Active).Select(detectedStrategy => detectedStrategy.Name()).ToList();
            var detectedEnemyStrategies = EnemyData.EnemyStrategies.Values.Where(s => s.Detected).Select(detectedStrategy => detectedStrategy.Name()).ToList();

            // TODO: enemy mined minerals and mined gas is not right, snapshots are messing it up, need to somehow better estimate it or save counts
            // mabye just remove them or just don't use them until something is figured out

            // TODO: maybe adjust the data gather rate to like 10 seconds, but only use every 1 minute?

            // TODO: should only do enemy strategy proxy? or include distance building is away?
            // TODO: should include buildings in progress separate from completed buildings?

            var enemyGeysers = BaseData.EnemyBases.SelectMany(b => b.VespeneGeysers.Where(u => u.Alliance == Alliance.Enemy));
            var emptyEnemyGeysers = BaseData.EnemyBases.SelectMany(b => b.VespeneGeysers.Where(u => u.Alliance != Alliance.Enemy));
            var enemyMinedGas = (2250 * enemyGeysers.Count()) - enemyGeysers.Sum(g => g.VespeneContents);
            var enemyMinerals = BaseData.EnemyBases.SelectMany(b => b.MineralFields);
            var enemyMinedMinerals = (10800 * BaseData.EnemyBases.Count()) - enemyMinerals.Sum(g => g.MineralContents);
            var enemyWorkerCount = UnitCountService.EnemyCount(UnitTypes.TERRAN_SCV) + UnitCountService.EnemyCount(UnitTypes.PROTOSS_PROBE) + UnitCountService.EnemyCount(UnitTypes.ZERG_DRONE);
            var enemyBaseData = new MLBaseData { BaseCount = BaseData.EnemyBases.Count(), BuiltGeyserCount = enemyGeysers.Count(), OpenGeyserCount = emptyEnemyGeysers.Count(), MinedGas = enemyMinedGas, MinedMinerals = enemyMinedMinerals, MineralPatchCount = enemyMinerals.Count(), WorkerCount = enemyWorkerCount };

            var selfGeysers = BaseData.SelfBases.SelectMany(b => b.VespeneGeysers.Where(u => u.Alliance == Alliance.Self));
            var emptySelfGeysers = BaseData.SelfBases.SelectMany(b => b.VespeneGeysers.Where(u => u.Alliance != Alliance.Self));
            var selfMinedGas = (2250 * selfGeysers.Count()) - selfGeysers.Sum(g => g.VespeneContents);
            var selfMinerals = BaseData.SelfBases.SelectMany(b => b.MineralFields);
            var selfMinedMinerals = (10800 * BaseData.SelfBases.Count()) - selfMinerals.Sum(g => g.MineralContents);
            var selfWorkerCount = UnitCountService.Count(UnitTypes.TERRAN_SCV) + UnitCountService.Count(UnitTypes.PROTOSS_PROBE) + UnitCountService.Count(UnitTypes.ZERG_DRONE);
            var selfBaseData = new MLBaseData { BaseCount = BaseData.SelfBases.Count(), BuiltGeyserCount = selfGeysers.Count(), OpenGeyserCount = emptySelfGeysers.Count(), MinedGas = selfMinedGas, MinedMinerals = selfMinedMinerals, MineralPatchCount = selfMinerals.Count(), WorkerCount = selfWorkerCount };

            var enemyUnitCounts = new Dictionary<UnitTypes, int>();
            var enemyUnits = ActiveUnitData.EnemyUnits.Values.GroupBy(e => e.Unit.UnitType);
            foreach (var group in enemyUnits)
            {
                enemyUnitCounts[(UnitTypes)group.Key] = group.Count();
            }

            var selfUnitCounts = new Dictionary<UnitTypes, int>();
            var selfUnits = ActiveUnitData.SelfUnits.Values.GroupBy(e => e.Unit.UnitType);
            foreach (var group in selfUnits)
            {
                selfUnitCounts[(UnitTypes)group.Key] = group.Count();
            }

            return new MLFrameData { ActiveEnemyStrategies = activeEnemyStrategies, DetectedEnemyStrategies = detectedEnemyStrategies, BuildHistory = new Dictionary<int, string>(buildHistory), EnemyBaseData = enemyBaseData, EnemyStrategyHistory = new Dictionary<int, string>(enemyStrategyHistory), EnemyUnitCounts = enemyUnitCounts, SelfBaseData = selfBaseData, SelfUnitCounts = selfUnitCounts };
        }
    }
}
