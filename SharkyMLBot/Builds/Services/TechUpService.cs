using Sharky;
using Sharky.DefaultBot;

namespace SharkyMLBot.Builds.Services
{
    public class TechUpService
    {
        UnitCountService UnitCountService;
        MacroData MacroData;

        public TechUpService(DefaultSharkyBot defaultSharkyBot)
        {
            MacroData = defaultSharkyBot.MacroData;
            UnitCountService = defaultSharkyBot.UnitCountService;
        }

        public void GetCyberneticsCore()
        {
            if (UnitCountService.Count(UnitTypes.PROTOSS_PYLON) > 0)
            {
                if (UnitCountService.Completed(UnitTypes.PROTOSS_PYLON) > 0)
                {
                    if (MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] < 1)
                    {
                        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] = 1;
                    }
                }
            }

            if (UnitCountService.EquivalentTypeCompleted(UnitTypes.PROTOSS_GATEWAY) > 0)
            {
                if (MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] < 1)
                {
                    MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
                }
            }
        }
    }
}
