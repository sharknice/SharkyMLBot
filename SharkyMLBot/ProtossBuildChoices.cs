using SC2APIProtocol;
using Sharky.Builds;
using Sharky.Builds.Protoss;
using Sharky.DefaultBot;
using SharkyMLBot.Builds;
using SharkyMLBot.Builds.Services;
using FourGate = SharkyMLBot.Builds.FourGate;

namespace SharkyMLBot
{
    public class ProtossBuildChoices
    {
        public BuildChoices DefaultBuildChoices { get; private set; }
        public BuildChoices TwoBaseVoidRaysBuildChoices { get; private set; }
        public BuildChoices FourGateBuildChoices { get; private set; }
        public BuildChoices OneBaseCarriersBuildChoices { get; private set; }

        public ProtossBuildChoices(DefaultSharkyBot defaultSharkyBot)
        {
            var counterTransitioner = new EmptyCounterTransitioner();
            var techUpService = new TechUpService(defaultSharkyBot);

            var twoBaseOpener = new TwoBaseOpener(defaultSharkyBot, counterTransitioner);
            var twoBaseVoidrays = new TwoBaseVoidrays(defaultSharkyBot, counterTransitioner, techUpService);
            var fourGate = new FourGate(defaultSharkyBot, counterTransitioner);
            var oneBaseCarriers = new OneBaseCarriers(defaultSharkyBot, counterTransitioner);

            // have the ML bot use this data and compare the last game played to determine the chance of victory each build has against it


            // maybe just start out with those 3 bulids to test the best strategy to start with prediction

            // TODO: maybe predict chance of winning with this build, chance of winning with other builds

            // multiple checks
            // analyze last game played, find best strategies against it and start with that
            // analyze right after scouting enemy base and switch builds if needed
            // analyze after new tech or enemystrategy discovered, tech structures count as tech, but also production structures like gateway, stargate, robotics facility, etc.
            // analyze every minute


            // TODO: do RL for deciding where to attack https://github.com/Sentdex/SC2RL like this tutorial but only for determining where to attack and when to attack

            var protossBuilds = new Dictionary<string, ISharkyBuild>
            {
                [twoBaseOpener.Name()] = twoBaseOpener,
                [twoBaseVoidrays.Name()] = twoBaseVoidrays,
                [fourGate.Name()] = fourGate,
                [oneBaseCarriers.Name()] = oneBaseCarriers,
            };

            var openingSquences = new List<List<string>>
            {
                new List<string> { twoBaseOpener.Name(), twoBaseVoidrays.Name() },
                new List<string> { fourGate.Name() },
                new List<string> { oneBaseCarriers.Name() },
            };

            var transitionSequences = new List<List<string>>
            {
                new List<string> { twoBaseVoidrays.Name() }
            };

            var protossBuildSequences = new Dictionary<string, List<List<string>>>
            {
                [Race.Terran.ToString()] = openingSquences,
                [Race.Zerg.ToString()] = openingSquences,
                [Race.Protoss.ToString()] = openingSquences,
                [Race.Random.ToString()] = openingSquences,
                ["Transition"] = transitionSequences,
            };

            DefaultBuildChoices = new BuildChoices { Builds = protossBuilds, BuildSequences = protossBuildSequences };

            var twoBaseVoidraysSequences = new List<List<string>>
            {
                new List<string> { twoBaseOpener.Name(), twoBaseVoidrays.Name() },
            };
            var twoBaseVoidraysBuildSequences = new Dictionary<string, List<List<string>>>
            {
                [Race.Terran.ToString()] = twoBaseVoidraysSequences,
                [Race.Zerg.ToString()] = twoBaseVoidraysSequences,
                [Race.Protoss.ToString()] = twoBaseVoidraysSequences,
                [Race.Random.ToString()] = twoBaseVoidraysSequences,
                ["Transition"] = twoBaseVoidraysSequences,
            };
            TwoBaseVoidRaysBuildChoices = new BuildChoices { Builds = protossBuilds, BuildSequences = twoBaseVoidraysBuildSequences };

            var fourGateSequences = new List<List<string>>
            {
                new List<string> { fourGate.Name() }
            };
            var fourGateBuildSequences = new Dictionary<string, List<List<string>>>
            {
                [Race.Terran.ToString()] = fourGateSequences,
                [Race.Zerg.ToString()] = fourGateSequences,
                [Race.Protoss.ToString()] = fourGateSequences,
                [Race.Random.ToString()] = fourGateSequences,
                ["Transition"] = fourGateSequences,
            };
            FourGateBuildChoices = new BuildChoices { Builds = protossBuilds, BuildSequences = fourGateBuildSequences };

            var oneBaseCarriersSequences = new List<List<string>>
            {
                new List<string> { oneBaseCarriers.Name() }
            };
            var oneBaseCarriersBuildSequences = new Dictionary<string, List<List<string>>>
            {
                [Race.Terran.ToString()] = oneBaseCarriersSequences,
                [Race.Zerg.ToString()] = oneBaseCarriersSequences,
                [Race.Protoss.ToString()] = oneBaseCarriersSequences,
                [Race.Random.ToString()] = oneBaseCarriersSequences,
                ["Transition"] = oneBaseCarriersSequences,
            };
            OneBaseCarriersBuildChoices = new BuildChoices { Builds = protossBuilds, BuildSequences = oneBaseCarriersBuildSequences };
        }
    }
}
