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
    //ML build chooser, run each build against last game, use winning build with highest confidence 
    // what we are really looking for is our build that would have won against the enemy build for the given game
    // maybe look at first few minutes of data?

    public class MLBuildDecisionService : RecentBuildDecisionService
    {
        public string BuildModelsDirectory { get; set; }

        public MLBuildDecisionService(ChatService chatService, EnemyPlayerService enemyPlayerService, RecordService recordService, BuildMatcher buildMatcher)
            : base(chatService, enemyPlayerService, recordService, buildMatcher)
        {
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
                    // TODO: use DateTime of game along with enemyID to get the ML data

                    var counter = GetBestCounterBuild(buildSequences, lastGame, myRace);
                    if (counter != null)
                    {
                        return counter;
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

            foreach (var buildSequence in buildSequences)
            {
                var buildString = string.Join(" ", buildSequence);
                var modelPath = $"{directory}/{buildString}.zip";
                if (File.Exists(modelPath))
                {
                    // TODO: put this into it's own service
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
                            ProxyDetected = data.DetectedEnemyStrategies.Contains("Proxy").ToString(),
                            ProxyActive = data.ActiveEnemyStrategies.Contains("Proxy").ToString(),
                            ENEMY_PROTOSS_NEXUS = data.EnemyUnitCounts[UnitTypes.PROTOSS_NEXUS],
                            ENEMY_PROTOSS_OBSERVER = data.EnemyUnitCounts[UnitTypes.PROTOSS_OBSERVER],
                            ENEMY_PROTOSS_ORACLE = data.EnemyUnitCounts[UnitTypes.PROTOSS_ORACLE],
                            ENEMY_PROTOSS_ORACLESTASISTRAP = data.EnemyUnitCounts[UnitTypes.PROTOSS_ORACLESTASISTRAP],
                            ENEMY_PROTOSS_PHOENIX = data.EnemyUnitCounts[UnitTypes.PROTOSS_PHOENIX],
                            ENEMY_PROTOSS_PHOTONCANNON = data.EnemyUnitCounts[UnitTypes.PROTOSS_PHOTONCANNON],
                            ENEMY_PROTOSS_PROBE = data.EnemyUnitCounts[UnitTypes.PROTOSS_PROBE],
                            ENEMY_PROTOSS_PYLON = data.EnemyUnitCounts[UnitTypes.PROTOSS_PYLON],
                            ENEMY_PROTOSS_PYLONOVERCHARGED = data.EnemyUnitCounts[UnitTypes.PROTOSS_PYLONOVERCHARGED],
                            ENEMY_PROTOSS_ROBOTICSBAY = data.EnemyUnitCounts[UnitTypes.PROTOSS_ROBOTICSBAY],
                            ENEMY_PROTOSS_MOTHERSHIPCORE = data.EnemyUnitCounts[UnitTypes.PROTOSS_MOTHERSHIPCORE],
                            ENEMY_PROTOSS_ROBOTICSFACILITY = data.EnemyUnitCounts[UnitTypes.PROTOSS_ROBOTICSFACILITY],
                            ENEMY_PROTOSS_SHIELDBATTERY = data.EnemyUnitCounts[UnitTypes.PROTOSS_SHIELDBATTERY],
                            ENEMY_PROTOSS_STALKER = data.EnemyUnitCounts[UnitTypes.PROTOSS_STALKER],
                            ENEMY_PROTOSS_STARGATE = data.EnemyUnitCounts[UnitTypes.PROTOSS_STARGATE],
                            ENEMY_PROTOSS_TEMPEST = data.EnemyUnitCounts[UnitTypes.PROTOSS_TEMPEST],
                            ENEMY_PROTOSS_TEMPLARARCHIVE = data.EnemyUnitCounts[UnitTypes.PROTOSS_TEMPLARARCHIVE],
                            ENEMY_PROTOSS_TWILIGHTCOUNCIL = data.EnemyUnitCounts[UnitTypes.PROTOSS_TWILIGHTCOUNCIL],
                            ENEMY_PROTOSS_VOIDRAY = data.EnemyUnitCounts[UnitTypes.PROTOSS_VOIDRAY],
                            ENEMY_PROTOSS_WARPGATE = data.EnemyUnitCounts[UnitTypes.PROTOSS_WARPGATE],
                            ENEMY_PROTOSS_WARPPRISM = data.EnemyUnitCounts[UnitTypes.PROTOSS_WARPPRISM],
                            ENEMY_PROTOSS_WARPPRISMPHASING = data.EnemyUnitCounts[UnitTypes.PROTOSS_WARPPRISMPHASING],
                            ENEMY_PROTOSS_SENTRY = data.EnemyUnitCounts[UnitTypes.PROTOSS_SENTRY],
                            ENEMY_PROTOSS_MOTHERSHIP = data.EnemyUnitCounts[UnitTypes.PROTOSS_MOTHERSHIP],
                            ENEMY_PROTOSS_INTERCEPTOR = data.EnemyUnitCounts[UnitTypes.PROTOSS_INTERCEPTOR],
                            ENEMY_PROTOSS_IMMORTAL = data.EnemyUnitCounts[UnitTypes.PROTOSS_IMMORTAL],
                            ENEMY_PROTOSS_ADEPT = data.EnemyUnitCounts[UnitTypes.PROTOSS_ADEPT],
                            ENEMY_PROTOSS_ADEPTPHASESHIFT = data.EnemyUnitCounts[UnitTypes.PROTOSS_ADEPTPHASESHIFT],
                            ENEMY_PROTOSS_ARCHON = data.EnemyUnitCounts[UnitTypes.PROTOSS_ARCHON],
                            ENEMY_PROTOSS_ASSIMILATOR = data.EnemyUnitCounts[UnitTypes.PROTOSS_ASSIMILATOR],
                            ENEMY_PROTOSS_CARRIER = data.EnemyUnitCounts[UnitTypes.PROTOSS_CARRIER],
                            ENEMY_PROTOSS_COLOSSUS = data.EnemyUnitCounts[UnitTypes.PROTOSS_COLOSSUS],
                            ENEMY_PROTOSS_CYBERNETICSCORE = data.EnemyUnitCounts[UnitTypes.PROTOSS_CYBERNETICSCORE],
                            ENEMY_PROTOSS_DARKSHRINE = data.EnemyUnitCounts[UnitTypes.PROTOSS_DARKSHRINE],
                            ENEMY_PROTOSS_DARKTEMPLAR = data.EnemyUnitCounts[UnitTypes.PROTOSS_DARKTEMPLAR],
                            ENEMY_PROTOSS_DISRUPTOR = data.EnemyUnitCounts[UnitTypes.PROTOSS_DISRUPTOR],
                            ENEMY_PROTOSS_DISRUPTORPHASED = data.EnemyUnitCounts[UnitTypes.PROTOSS_DISRUPTORPHASED],
                            ENEMY_PROTOSS_FLEETBEACON = data.EnemyUnitCounts[UnitTypes.PROTOSS_FLEETBEACON],
                            ENEMY_PROTOSS_FORGE = data.EnemyUnitCounts[UnitTypes.PROTOSS_FORGE],
                            ENEMY_PROTOSS_GATEWAY = data.EnemyUnitCounts[UnitTypes.PROTOSS_GATEWAY],
                            ENEMY_PROTOSS_HIGHTEMPLAR = data.EnemyUnitCounts[UnitTypes.PROTOSS_HIGHTEMPLAR],
                            ENEMY_PROTOSS_ZEALOT = data.EnemyUnitCounts[UnitTypes.PROTOSS_ZEALOT],
                            ENEMY_PROTOSS_ASSIMILATORRICH = data.EnemyUnitCounts[UnitTypes.PROTOSS_ASSIMILATOR],
                        };
                        var result = predictionEngine.Predict(modelInput);
                        Console.WriteLine($"Frame {frameData.Key}");
                        Console.WriteLine($"Result: {result.PredictedLabel}, Score: {string.Join(" ", result.Score)}");
                    }
                    // TODO: determine if this is good somehow
                    // maybe add up the score of every win, use the highest one

                }

            }

            return null; // if nothing good just do it the old way
        }
    }
}
