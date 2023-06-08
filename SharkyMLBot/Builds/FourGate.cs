using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.MicroTasks;

namespace SharkyMLBot.Builds
{
    public class FourGate : ProtossSharkyBuild
    {
        SharkyUnitData SharkyUnitData;

        bool Scouted;
        WorkerScoutTask? WorkerScoutTask;
        ProxyScoutTask? ProxyScoutTask;

        public FourGate(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
        {
            SharkyUnitData = defaultSharkyBot.SharkyUnitData;
        }

        public override void StartBuild(int frame)
        {
            base.StartBuild(frame);

            BuildOptions.StrictGasCount = true;
            BuildOptions.StrictSupplyCount = true;
            BuildOptions.StrictWorkerCount = true;
            BuildOptions.StrictWorkersPerGas = true;
            BuildOptions.StrictWorkersPerGasCount = 0;

            ChronoData.ChronodUnits = new HashSet<UnitTypes>
            {
                UnitTypes.PROTOSS_ZEALOT
            };
            ChronoData.ChronodUpgrades = new HashSet<Upgrades>
            {
                Upgrades.WARPGATERESEARCH
            };

            AttackData.UseAttackDataManager = false;
            AttackData.CustomAttackFunction = false;
            AttackData.ArmyFoodAttack = 22;

            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_PROBE] = 23;

            Scouted = false;
            WorkerScoutTask = (WorkerScoutTask)MicroTaskData[typeof(WorkerScoutTask).Name];
            ProxyScoutTask = (ProxyScoutTask)MicroTaskData[typeof(ProxyScoutTask).Name];
        }

        void Opening(int frame)
        {
            SendProbeForFirstPylon(frame);

            if (MacroData.FoodUsed >= 14)
            {
                if (MacroData.DesiredPylons < 1)
                {
                    MacroData.DesiredPylons = 1;
                }
            }

            SendProbeForFirstGateway(frame);

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
                    if (UnitCountService.Count(UnitTypes.PROTOSS_ASSIMILATOR) < 2)
                    {
                        ChronoData.ChronodUnits.Add(UnitTypes.PROTOSS_PROBE);
                    }
                }
            }

            if (UnitCountService.EquivalentTypeCount(UnitTypes.PROTOSS_GATEWAY) > 0)
            {
                BuildOptions.StrictGasCount = false;
            }
            if (UnitCountService.Count(UnitTypes.PROTOSS_ASSIMILATOR) >= 2)
            {
                if (ChronoData.ChronodUnits.Contains(UnitTypes.PROTOSS_PROBE))
                {
                    ChronoData.ChronodUnits.Remove(UnitTypes.PROTOSS_PROBE);
                }
                SendProbeForSecondGateway(frame);
                if (MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] < 2)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] = 2;
                }

                if (UnitCountService.Count(UnitTypes.PROTOSS_GATEWAY) >= 2)
                {
                    if (MacroData.DesiredPylons < 2)
                    {
                        MacroData.DesiredPylons = 2;
                    }
                }
            }

            SendProbeForCyberneticsCore(frame);
            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.PROTOSS_GATEWAY) > 0)
            {
                if (MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] < 1)
                {
                    MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
                }
            }
            if (UnitCountService.Count(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0)
            {
                if (MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ZEALOT] < 5)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ZEALOT] = 5;
                }
                if (MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] < 4)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] = 4;
                }
            }

            if (UnitCountService.Completed(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0)
            {
                if (MacroData.DesiredPylons < 3)
                {
                    MacroData.DesiredPylons = 3;
                }

                MacroData.DesiredUpgrades[Upgrades.WARPGATERESEARCH] = true;

                if (MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_STALKER] < 5)
                {
                    MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_STALKER] = 5;
                }

                if (UnitCountService.Count(UnitTypes.PROTOSS_PYLON) >= 3)
                {
                    if (UnitCountService.EquivalentTypeCompleted(UnitTypes.PROTOSS_GATEWAY) >= 4)
                    {
                        if (MacroData.DesiredPylons < 5)
                        {
                            MacroData.DesiredPylons = 5;
                        }
                    }
                }
                if (SharkyUnitData.ResearchedUpgrades.Contains((uint)Upgrades.WARPGATERESEARCH) || UnitCountService.EquivalentTypeCount(UnitTypes.PROTOSS_GATEWAY) >= 4)
                {
                    if (UnitCountService.Count(UnitTypes.PROTOSS_PYLON) >= 5)
                    {
                        BuildOptions.StrictSupplyCount = false;
                    }
                    if (MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_STALKER] < 15)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_STALKER] = 15;
                    }
                    if (MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_SENTRY] < 1)
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_SENTRY] = 1;
                    }
                }
            }
        }

        void MidGame(int frame)
        {
            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.PROTOSS_GATEWAY) >= 4)
            {
                if (MacroData.VespeneGas > 400 && MacroData.Minerals < 200)
                {
                    if (MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_SENTRY] <= UnitCountService.Count(UnitTypes.PROTOSS_SENTRY))
                    {
                        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_SENTRY]++;
                    }
                }
                else
                {
                    MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_SENTRY] = UnitCountService.Count(UnitTypes.PROTOSS_SENTRY);
                }

                if (UnitCountService.Count(UnitTypes.PROTOSS_STALKER) >= MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_STALKER])
                {
                    MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_STALKER]++;
                }
            }

            if (!AttackData.UseAttackDataManager)
            {
                if (UnitCountService.Count(UnitTypes.PROTOSS_STALKER) > 12 || MacroData.Frame > SharkyOptions.FramesPerSecond * 4 * 60)
                {
                    AttackData.UseAttackDataManager = true;
                }
            }

            if (MacroData.FoodUsed >= 80)
            {
                if (MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] < 2)
                {
                    MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = 2;
                }
            }
        }

        void ManageGasWorkers(int frame)
        {
            if (SharkyUnitData.ResearchedUpgrades.Contains((uint)Upgrades.WARPGATERESEARCH))
            {
                BuildOptions.StrictWorkersPerGas = false;
            }
            else if (BuildOptions.StrictWorkersPerGas && UnitCountService.Completed(UnitTypes.PROTOSS_ASSIMILATOR) > 0)
            {
                BuildOptions.StrictWorkersPerGasCount = (int)System.Math.Ceiling((UnitCountService.Count(UnitTypes.PROTOSS_PROBE) - 17) / 2f);
                if (BuildOptions.StrictWorkersPerGasCount > 3)
                {
                    BuildOptions.StrictWorkersPerGasCount = 3;
                }
                if (BuildOptions.StrictWorkersPerGasCount < 0)
                {
                    BuildOptions.StrictWorkersPerGasCount = 0;
                }
            }
        }

        public override void OnFrame(ResponseObservation observation)
        {
            var frame = (int)observation.Observation.GameLoop;

            Opening(frame);
            MidGame(frame);
            ManageGasWorkers(frame);
        }
    }
}
