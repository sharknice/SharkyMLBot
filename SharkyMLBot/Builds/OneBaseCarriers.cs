using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.MicroTasks;

namespace SharkyMLBot.Builds
{
    public class OneBaseCarriers : ProtossSharkyBuild
    {
        ForceFieldRampTask? ForceFieldRampTask;

        bool Scouted;
        WorkerScoutTask? WorkerScoutTask;
        ProxyScoutTask? ProxyScoutTask;

        public OneBaseCarriers(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
        {
        }

        public override void StartBuild(int frame)
        {
            base.StartBuild(frame);

            BuildOptions.StrictGasCount = true;
            BuildOptions.StrictWorkerCount = true;
            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_PROBE] = 23;

            ChronoData.ChronodUnits = new HashSet<UnitTypes>
            {
                UnitTypes.PROTOSS_ZEALOT,
                UnitTypes.PROTOSS_CARRIER,
            };
            ChronoData.ChronodUpgrades = new HashSet<Upgrades>
            {
                Upgrades.PROTOSSAIRWEAPONSLEVEL1,
                Upgrades.PROTOSSAIRWEAPONSLEVEL2,
                Upgrades.PROTOSSAIRWEAPONSLEVEL3,
                Upgrades.PROTOSSAIRARMORSLEVEL1,
                Upgrades.PROTOSSAIRARMORSLEVEL2,
                Upgrades.PROTOSSAIRARMORSLEVEL3,
            };

            MacroData.DefensiveBuildingMaximumDistance = 5;

            ForceFieldRampTask = (ForceFieldRampTask)MicroTaskData[typeof(ForceFieldRampTask).Name];

            Scouted = false;
            WorkerScoutTask = (WorkerScoutTask)MicroTaskData[typeof(WorkerScoutTask).Name];
            ProxyScoutTask = (ProxyScoutTask)MicroTaskData[typeof(ProxyScoutTask).Name];
        }

        void Opening(int frame)
        {
            SendProbeForFirstGateway(frame);
            SendProbeForCyberneticsCore(frame);

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

                    if (UnitCountService.EquivalentTypeCount(UnitTypes.PROTOSS_GATEWAY) > 0)
                    {
                        ChronoData.ChronodUnits.Add(UnitTypes.PROTOSS_PROBE);

                        BuildOptions.StrictGasCount = false;

                        if (UnitCountService.EquivalentTypeCompleted(UnitTypes.PROTOSS_GATEWAY) > 0)
                        {
                            if (MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] < 1)
                            {
                                MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
                            }
                        }
                    }

                    if (UnitCountService.Count(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0)
                    {
                        if (MacroData.DesiredTechCounts[UnitTypes.PROTOSS_FORGE] < 1)
                        {
                            MacroData.DesiredTechCounts[UnitTypes.PROTOSS_FORGE] = 1;
                        }

                        if (UnitCountService.Completed(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0)
                        {
                            if (ForceFieldRampTask != null && !ForceFieldRampTask.Enabled)
                            {
                                ForceFieldRampTask.Enable();
                            }

                            if (MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_STARGATE] < 1)
                            {
                                MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_STARGATE] = 1;
                            }

                            if (UnitCountService.Completed(UnitTypes.PROTOSS_MOTHERSHIP) > 0)
                            {
                                MacroData.DesiredUpgrades[Upgrades.PROTOSSAIRWEAPONSLEVEL1] = true;
                            }
                        }
                    }

                    if (UnitCountService.Count(UnitTypes.PROTOSS_STARGATE) > 0)
                    {
                        if (MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_SHIELDBATTERY] < 2)
                        {
                            MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_SHIELDBATTERY] = 2;
                        }
                        if (MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_SENTRY] < 1)
                        {
                            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_SENTRY] = 1;
                        }

                        if (UnitCountService.Completed(UnitTypes.PROTOSS_STARGATE) > 0)
                        {
                            if (MacroData.DesiredTechCounts[UnitTypes.PROTOSS_FLEETBEACON] < 1)
                            {
                                MacroData.DesiredTechCounts[UnitTypes.PROTOSS_FLEETBEACON] = 1;
                            }

                            if (UnitCountService.Count(UnitTypes.PROTOSS_FLEETBEACON) > 0)
                            {
                                if (MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ORACLE] < 1)
                                {
                                    MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ORACLE] = 1;
                                }

                                if (UnitCountService.Completed(UnitTypes.PROTOSS_FLEETBEACON) > 0)
                                {
                                    MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ORACLE] = 0;
                                    if (MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_CARRIER] < 12)
                                    {
                                        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_CARRIER] = 12;
                                    }
                                    if (MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_MOTHERSHIP] < 1)
                                    {
                                        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_MOTHERSHIP] = 1;
                                    }
                                }
                            }
                        }
                    }

                    if (UnitCountService.Completed(UnitTypes.PROTOSS_FORGE) > 0 && UnitCountService.Count(UnitTypes.PROTOSS_STARGATE) > 0)
                    {
                        if (MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_PHOTONCANNON] < 1)
                        {
                            MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_PHOTONCANNON] = 1;
                        }
                        if (UnitCountService.Count(UnitTypes.PROTOSS_FLEETBEACON) > 0)
                        {
                            MacroData.ProtossMacroData.DesiredPylonsAtDefensivePoint = 3;
                            MacroData.DefensiveBuildingMaximumDistance = 10;

                            if (MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_PHOTONCANNON] < 2)
                            {
                                MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_PHOTONCANNON] = 2;
                            }

                            if (UnitCountService.Count(UnitTypes.PROTOSS_CARRIER) > 0 && MacroData.Minerals > 550 && UnitCountService.Count(UnitTypes.PROTOSS_PHOTONCANNON) < 8)
                            {
                                if (MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_PHOTONCANNON] <= UnitCountService.Count(UnitTypes.PROTOSS_PHOTONCANNON))
                                {
                                    MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_PHOTONCANNON]++;
                                }
                                if (MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_SHIELDBATTERY] <= UnitCountService.Count(UnitTypes.PROTOSS_SHIELDBATTERY))
                                {
                                    MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.PROTOSS_SHIELDBATTERY]++;
                                }
                                MacroData.DefensiveBuildingMaximumDistance = 15;
                            }
                        }
                    }
                }
            }
        }

        public override void OnFrame(ResponseObservation observation)
        {
            var frame = (int)observation.Observation.GameLoop;

            Opening(frame);
        }
    }
}
