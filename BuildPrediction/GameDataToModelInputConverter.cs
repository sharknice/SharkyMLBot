using Sharky;
using SharkyMLDataManager;
using static BuildPrediction.MLModel1;

namespace BuildPrediction
{
    public class GameDataToModelInputConverter
    {
        public List<ModelInput> GetModelInputs(MLGameData gameData, string mapName)
        {
            var modelInputs = new List<ModelInput>();

            if (gameData.MLFramesData == null) { return modelInputs; }
            foreach (var frameData in gameData.MLFramesData)
            {
                var data = frameData.Value;
                if (data?.EnemyUnitCounts == null || data.EnemyUnitCounts.Sum(e => e.Value) < 1 || data.DetectedEnemyStrategies == null || data.ActiveEnemyStrategies == null) { continue; }

                var modelInput = GetModelInput(gameData, frameData, mapName);
                modelInputs.Add(modelInput);
            }

            return modelInputs;
        }

        public ModelInput? GetModelInput(MLGameData gameData, KeyValuePair<int, MLFrameData> frameData, string mapName)
        {
            var data = frameData.Value;

            var modelInput = new ModelInput()
            {
                MapName = mapName,
                EnemySelectedRace = (float)gameData.Game.EnemySelectedRace,
                EnemyRace = (float)gameData.Game.EnemyRace,
                MySelectedRace = (float)gameData.Game.MySelectedRace,
                MyRace = (float)gameData.Game.MyRace,
                Frame = frameData.Key,
                ProxyDetected = data.DetectedEnemyStrategies.Contains("Proxy"),
                ProxyActive = data.ActiveEnemyStrategies.Contains("Proxy"),
                ENEMY_PROTOSS_NEXUS = GetEnemyUnitCount(data, UnitTypes.PROTOSS_NEXUS),
                ENEMY_PROTOSS_OBSERVER = GetEnemyUnitCount(data, UnitTypes.PROTOSS_OBSERVER),
                ENEMY_PROTOSS_ORACLE = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ORACLE),
                ENEMY_PROTOSS_ORACLESTASISTRAP = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ORACLESTASISTRAP),
                ENEMY_PROTOSS_PHOENIX = GetEnemyUnitCount(data, UnitTypes.PROTOSS_PHOENIX),
                ENEMY_PROTOSS_PHOTONCANNON = GetEnemyUnitCount(data, UnitTypes.PROTOSS_PHOTONCANNON),
                ENEMY_PROTOSS_PROBE = GetEnemyUnitCount(data, UnitTypes.PROTOSS_PROBE),
                ENEMY_PROTOSS_PYLON = GetEnemyUnitCount(data, UnitTypes.PROTOSS_PYLON),
                ENEMY_PROTOSS_PYLONOVERCHARGED = GetEnemyUnitCount(data, UnitTypes.PROTOSS_PYLONOVERCHARGED),
                ENEMY_PROTOSS_ROBOTICSBAY = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ROBOTICSBAY),
                ENEMY_PROTOSS_MOTHERSHIPCORE = GetEnemyUnitCount(data, UnitTypes.PROTOSS_MOTHERSHIPCORE),
                ENEMY_PROTOSS_ROBOTICSFACILITY = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ROBOTICSFACILITY),
                ENEMY_PROTOSS_SHIELDBATTERY = GetEnemyUnitCount(data, UnitTypes.PROTOSS_SHIELDBATTERY),
                ENEMY_PROTOSS_STALKER = GetEnemyUnitCount(data, UnitTypes.PROTOSS_STALKER),
                ENEMY_PROTOSS_STARGATE = GetEnemyUnitCount(data, UnitTypes.PROTOSS_STARGATE),
                ENEMY_PROTOSS_TEMPEST = GetEnemyUnitCount(data, UnitTypes.PROTOSS_TEMPEST),
                ENEMY_PROTOSS_TEMPLARARCHIVE = GetEnemyUnitCount(data, UnitTypes.PROTOSS_TEMPLARARCHIVE),
                ENEMY_PROTOSS_TWILIGHTCOUNCIL = GetEnemyUnitCount(data, UnitTypes.PROTOSS_TWILIGHTCOUNCIL),
                ENEMY_PROTOSS_VOIDRAY = GetEnemyUnitCount(data, UnitTypes.PROTOSS_VOIDRAY),
                ENEMY_PROTOSS_WARPGATE = GetEnemyUnitCount(data, UnitTypes.PROTOSS_WARPGATE),
                ENEMY_PROTOSS_WARPPRISM = GetEnemyUnitCount(data, UnitTypes.PROTOSS_WARPPRISM),
                ENEMY_PROTOSS_WARPPRISMPHASING = GetEnemyUnitCount(data, UnitTypes.PROTOSS_WARPPRISMPHASING),
                ENEMY_PROTOSS_SENTRY = GetEnemyUnitCount(data, UnitTypes.PROTOSS_SENTRY),
                ENEMY_PROTOSS_MOTHERSHIP = GetEnemyUnitCount(data, UnitTypes.PROTOSS_MOTHERSHIP),
                ENEMY_PROTOSS_INTERCEPTOR = GetEnemyUnitCount(data, UnitTypes.PROTOSS_INTERCEPTOR),
                ENEMY_PROTOSS_IMMORTAL = GetEnemyUnitCount(data, UnitTypes.PROTOSS_IMMORTAL),
                ENEMY_PROTOSS_ADEPT = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ADEPT),
                ENEMY_PROTOSS_ADEPTPHASESHIFT = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ADEPTPHASESHIFT),
                ENEMY_PROTOSS_ARCHON = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ARCHON),
                ENEMY_PROTOSS_ASSIMILATOR = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ASSIMILATOR),
                ENEMY_PROTOSS_CARRIER = GetEnemyUnitCount(data, UnitTypes.PROTOSS_CARRIER),
                ENEMY_PROTOSS_COLOSSUS = GetEnemyUnitCount(data, UnitTypes.PROTOSS_COLOSSUS),
                ENEMY_PROTOSS_CYBERNETICSCORE = GetEnemyUnitCount(data, UnitTypes.PROTOSS_CYBERNETICSCORE),
                ENEMY_PROTOSS_DARKSHRINE = GetEnemyUnitCount(data, UnitTypes.PROTOSS_DARKSHRINE),
                ENEMY_PROTOSS_DARKTEMPLAR = GetEnemyUnitCount(data, UnitTypes.PROTOSS_DARKTEMPLAR),
                ENEMY_PROTOSS_DISRUPTOR = GetEnemyUnitCount(data, UnitTypes.PROTOSS_DISRUPTOR),
                ENEMY_PROTOSS_DISRUPTORPHASED = GetEnemyUnitCount(data, UnitTypes.PROTOSS_DISRUPTORPHASED),
                ENEMY_PROTOSS_FLEETBEACON = GetEnemyUnitCount(data, UnitTypes.PROTOSS_FLEETBEACON),
                ENEMY_PROTOSS_FORGE = GetEnemyUnitCount(data, UnitTypes.PROTOSS_FORGE),
                ENEMY_PROTOSS_GATEWAY = GetEnemyUnitCount(data, UnitTypes.PROTOSS_GATEWAY),
                ENEMY_PROTOSS_HIGHTEMPLAR = GetEnemyUnitCount(data, UnitTypes.PROTOSS_HIGHTEMPLAR),
                ENEMY_PROTOSS_ZEALOT = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ZEALOT),
                ENEMY_PROTOSS_ASSIMILATORRICH = GetEnemyUnitCount(data, UnitTypes.PROTOSS_ASSIMILATOR),
            };

            return modelInput;
        }

        int GetEnemyUnitCount(MLFrameData? data, UnitTypes unitType)
        {
            if (data?.EnemyUnitCounts == null) { return 0; }

            if (data.EnemyUnitCounts.TryGetValue(unitType, out var result))
            {
                return result;
            }
            return 0;
        }
    }
}
