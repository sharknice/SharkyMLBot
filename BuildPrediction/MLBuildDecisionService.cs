using Microsoft.ML;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds.BuildChoosing;
using Sharky.Chat;
using Sharky.EnemyPlayer;
using SharkyMLDataManager;
using static BuildPrediction.MLModel1;

namespace BuildPrediction
{
    public class MLBuildDecisionService : RecentBuildDecisionService
    {
        public string BuildModelsDirectory { get; set; }

        MLDataFileService MLDataFileService { get; set; }

        public MLBuildDecisionService(ChatService chatService, EnemyPlayerService enemyPlayerService, RecordService recordService, BuildMatcher buildMatcher, MLDataFileService mLDataFileService)
            : base(chatService, enemyPlayerService, recordService, buildMatcher)
        {
            MLDataFileService = mLDataFileService;
            BuildModelsDirectory = $"data/BuildModels";
        }

        public override List<string> GetBestBuild(EnemyPlayer enemyBot, List<List<string>> buildSequences, string map, List<EnemyPlayer> enemyBots, Race enemyRace, Race myRace)
        {
            List<string> debugMessage = new List<string>();
            debugMessage.Add($"Choosing build against {enemyBot.Name} - {enemyBot.Id} on {map}");
            Console.WriteLine($"Choosing build against {enemyBot.Name} - {enemyBot.Id} on {map}");

            var lastGame = enemyBot.Games.Where(g => g.EnemyRace == enemyRace).FirstOrDefault();
            if (lastGame != null)
            {
                Console.WriteLine($"{(Result)lastGame.Result} last game with: {string.Join(" ", lastGame.Builds.Values)}");
                if (lastGame.Result == (int)Result.Victory)
                {
                    Console.WriteLine("Won last game, using same build");
                    var sequence = buildSequences.FirstOrDefault(b => BuildMatcher.MatchesBuildSequence(lastGame, b));
                    if (sequence != null)
                    {
                        Console.WriteLine($"choice: {string.Join(" ", sequence)}");
                        return sequence;
                    }
                }
                else
                {
                    var lastGameMlData = MLDataFileService.MLGameData.FirstOrDefault(g => g.Game.DateTime == lastGame.DateTime && g.Game.EnemyId == lastGame.EnemyId);
                    if (lastGameMlData != null)
                    {
                        var counter = GetBestCounterBuild(buildSequences, lastGameMlData, myRace);
                        if (counter != null)
                        {
                            return counter;
                        }
                    }
                }
            }

            Console.WriteLine("Did not find anything good. Using non-ML build choice");
            return base.GetBestBuild(enemyBot, buildSequences, map, enemyBots, enemyRace, myRace);
        }

        List<string>? GetBestCounterBuild(List<List<string>> buildSequences, MLGameData gameData, Race myRace)
        {
            if (gameData.Game == null) { return null; }
            var lastGame = gameData.Game;
            var directory = $"{BuildModelsDirectory}/Race/{lastGame.EnemyRace}";

            List<string> bestSequence = null;
            var bestSequenceValue = 0f;

            foreach (var buildSequence in buildSequences)
            {
                var buildString = string.Join(" ", buildSequence);
                var modelPath = $"{directory}/{buildString}.zip";
                if (File.Exists(modelPath))
                {
                    // TODO: put this into it's own service
                    var sequenceValue = 0f;
                    var maxValue = 0f;

                    Console.WriteLine($"Predicting {buildString}");
                    if (gameData.MLFramesData == null) { continue; }
                    foreach (var frameData in gameData.MLFramesData)
                    {                        
                        var data = frameData.Value;
                        if (data?.EnemyUnitCounts == null || data.EnemyUnitCounts.Sum(e => e.Value) < 1 || data.DetectedEnemyStrategies == null || data.ActiveEnemyStrategies == null) { continue; }

                        var mlContext = new MLContext();
                        ITransformer mlModel = mlContext.Model.Load(modelPath, out var _);
                        var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
                        var modelInput = new ModelInput()
                        {
                            MapName = gameData.Game.MapName,
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
                        var result = predictionEngine.Predict(modelInput);
                        Console.WriteLine($"Frame {frameData.Key}");
                        Console.WriteLine($"Result: {result.PredictedLabel}, Score: {string.Join(" ", result.Score)}");

                        maxValue += 1;
                        // TODO: figure out which score is for winning, and always add that on, not sure if there is a way to do that, might have to only add the highest one if predictedlabel is win
                        sequenceValue += result.Score[0]; // not sure what result.score is, this code could be wrong
                    }

                    var overall = sequenceValue / maxValue;
                    if (overall > 0 && overall > bestSequenceValue)
                    {
                        bestSequenceValue = overall;
                        bestSequence = buildSequence;
                    }
                }

            }

            if (bestSequenceValue > .75f)
            {
                return bestSequence;
            }

            return null; // if nothing good just do it the old way
        }

        int GetEnemyUnitCount(MLFrameData? data, UnitTypes unitType)
        {
            if (data.EnemyUnitCounts.TryGetValue(unitType, out var result))
            {
                return result;
            }
            return 0;
        }
    }
}
