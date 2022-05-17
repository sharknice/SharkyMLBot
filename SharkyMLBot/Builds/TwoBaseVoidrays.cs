using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using SharkyMLBot.Builds.Services;

namespace SharkyMLBot.Builds
{
    public class TwoBaseVoidrays : ProtossSharkyBuild
    {
        TechUpService TechUpService;

        public TwoBaseVoidrays(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner, TechUpService techUpService) : base(defaultSharkyBot, counterTransitioner)
        {
            TechUpService = techUpService;
        }

        public override void StartBuild(int frame)
        {
            base.StartBuild(frame);

            ChronoData.ChronodUnits = new HashSet<UnitTypes>
            {
                UnitTypes.PROTOSS_VOIDRAY,
            };
        }

        public override void OnFrame(ResponseObservation observation)
        {
            var frame = (int)observation.Observation.GameLoop;

            TechUpService.GetCyberneticsCore();

            if (UnitCountService.Completed(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0)
            {
                if (MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_STARGATE] < 3)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_STARGATE] = 3;
                }
            }

            if (UnitCountService.Completed(UnitTypes.PROTOSS_STARGATE) > 0)
            {
                MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_VOIDRAY] = UnitCountService.UnitsDoneAndInProgressCount(UnitTypes.PROTOSS_VOIDRAY) + 1;
                MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ORACLE] = 1;
            }

            if (MacroData.Minerals > 1000)
            {
                if (MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] <= UnitCountService.Count(UnitTypes.PROTOSS_NEXUS))
                {
                    MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = UnitCountService.Count(UnitTypes.PROTOSS_NEXUS) + 1;
                }
            }
        }
    }
}
