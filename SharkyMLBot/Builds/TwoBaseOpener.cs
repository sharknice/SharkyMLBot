using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.MicroTasks;

namespace SharkyMLBot.Builds
{
    public class TwoBaseOpener : ProtossSharkyBuild
    {
        bool Scouted;
        WorkerScoutTask? WorkerScoutTask;
        ProxyScoutTask? ProxyScoutTask;

        public TwoBaseOpener(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
        {
        }

        public override void StartBuild(int frame)
        {
            base.StartBuild(frame);

            BuildOptions.StrictGasCount = true;

            ChronoData.ChronodUnits = new HashSet<UnitTypes>
            {
                UnitTypes.PROTOSS_PROBE,
            };

            Scouted = false;
            WorkerScoutTask = (WorkerScoutTask)MicroTaskData.MicroTasks["WorkerScoutTask"];
            ProxyScoutTask = (ProxyScoutTask)MicroTaskData.MicroTasks["ProxyScoutTask"];
        }

        public override void OnFrame(ResponseObservation observation)
        {
            var frame = (int)observation.Observation.GameLoop;

            if (UnitCountService.Count(UnitTypes.PROTOSS_PYLON) > 0)
            {
                if (!Scouted)
                {
                    if (WorkerScoutTask != null)
                    {
                        WorkerScoutTask.Enable();
                    }
                    if (ProxyScoutTask != null)
                    {
                        ProxyScoutTask.Enable();
                    }
                    Scouted = true;
                }

                if (UnitCountService.Completed(UnitTypes.PROTOSS_PYLON) > 0)
                {
                    if (MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] < 1)
                    {
                        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] = 1;
                    }
                    if (MacroData.DesiredGases < 1)
                    {
                        MacroData.DesiredGases = 1;
                    }
                }
            }

            if (UnitCountService.EquivalentTypeCount(UnitTypes.PROTOSS_GATEWAY) > 0)
            {
                SendProbeForNexus(frame);

                if (MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] < 2)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = 2;
                }
            }
            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.PROTOSS_GATEWAY) > 0)
            {
                if (MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] < 1)
                {
                    MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
                }
                MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ZEALOT] = UnitCountService.UnitsDoneAndInProgressCount(UnitTypes.PROTOSS_ZEALOT) + 1;
            }
            if (UnitCountService.Count(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0)
            {
                if (UnitCountService.Completed(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ZEALOT] = 0;
                    MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_STALKER] = UnitCountService.UnitsDoneAndInProgressCount(UnitTypes.PROTOSS_STALKER) + 1;
                }
                else
                {
                    MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ZEALOT] = UnitCountService.UnitsDoneAndInProgressCount(UnitTypes.PROTOSS_ZEALOT) + 1;
                }
            }
        }

        public override bool Transition(int frame)
        {
            return UnitCountService.Count(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0 && UnitCountService.Count(UnitTypes.PROTOSS_NEXUS) > 1;
        }
    }
}
