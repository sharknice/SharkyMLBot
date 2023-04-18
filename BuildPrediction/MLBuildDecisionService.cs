using Microsoft.ML;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.EnemyPlayer;
using SharkyMLDataManager;

namespace BuildPrediction
{
    public class MLBuildDecisionService : RecentBuildDecisionService
    {
        public string BuildModelsDirectory { get; set; }

        MLDataFileService MLDataFileService;
        GameDataToModelInputConverter GameDataToModelInputConverter;
        BuildModelScoreService BuildModelScoreService;

        public MLBuildDecisionService(DefaultSharkyBot defaultSharkyBot, MLDataFileService mLDataFileService, GameDataToModelInputConverter gameDataToModelInputConverter, BuildModelTrainingManager buildModelTrainingManager, BuildModelScoreService buildModelScoreService, string buildModelsDirectory)
            : base(defaultSharkyBot)
        {
            MLDataFileService = mLDataFileService;
            GameDataToModelInputConverter = gameDataToModelInputConverter;

            BuildModelsDirectory = buildModelsDirectory;

            if (!Directory.Exists(BuildModelsDirectory))
            {
                buildModelTrainingManager.UpdateBuildModels();
            }
            BuildModelScoreService = buildModelScoreService;
        }

        public override List<string> GetBestBuild(EnemyPlayer enemyBot, List<List<string>> buildSequences, string map, List<EnemyPlayer> enemyBots, Race enemyRace, Race myRace)
        {
            Console.WriteLine($"Choosing build against {enemyBot.Name} - {enemyBot.Id} on {map}");

            var lastGame = enemyBot.Games.Where(g => g.MyRace == myRace && g.EnemySelectedRace == enemyRace).FirstOrDefault();
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
                    else
                    {
                        if (lastGame.PlannedBuildSequence != null)
                        {
                            Console.WriteLine($"Planned Build Sequence from last game not in list: {string.Join(" ", lastGame.PlannedBuildSequence)}");
                        }
                    }
                }
                else
                {
                    var lastGameMlData = MLDataFileService.MLGameData.FirstOrDefault(g => g.Game.DateTime == lastGame.DateTime && g.Game.EnemyId == lastGame.EnemyId);
                    if (lastGameMlData != null)
                    {
                        var consecutiveLosses = new List<Game>();
                        foreach (var game in enemyBot.Games.Where(g => g.MyRace == myRace && g.EnemySelectedRace == enemyRace))
                        {
                            if (game.Result == (int)Result.Defeat)
                            {
                                if (game.PlannedBuildSequence != null)
                                {
                                    Console.WriteLine($"Excluding: {string.Join(" ", game.PlannedBuildSequence)}");
                                }
                                else
                                {
                                    Console.WriteLine($"Excluding: {string.Join(" ", game.Builds)}");
                                }
                                consecutiveLosses.Add(game);
                            }
                            else
                            {
                                break;
                            }
                        }
                        var counter = GetBestCounterBuild(buildSequences.Where(b => !consecutiveLosses.Any(l => BuildMatcher.MatchesBuildSequence(l, b))), lastGameMlData, map, myRace);
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

        List<string>? GetBestCounterBuild(IEnumerable<List<string>> buildSequences, MLGameData gameData, string mapName, Race myRace)
        {
            if (gameData.Game == null) { return null; }
            var lastGame = gameData.Game;
            var enemyDirectory = $"{BuildModelsDirectory}/{myRace}/EnemyId/{lastGame.EnemyId}";
            var directory = $"{BuildModelsDirectory}/{myRace}/Race/{lastGame.EnemyRace}";
            var inputModels = GameDataToModelInputConverter.GetModelInputs(gameData, mapName);
            var mlContext = new MLContext();

            List<string> bestSequence = null;
            var bestSequenceValue = 0f;

            foreach (var buildSequence in buildSequences)
            {
                var buildString = string.Join(" ", buildSequence);
                var modelPath = $"{enemyDirectory}/{buildString}";
                if (!Directory.Exists(modelPath))
                {
                    modelPath = $"{directory}/{buildString}";
                }
                if (Directory.Exists(modelPath))
                {
                    try
                    {
                        var overall = BuildModelScoreService.GetScoreForBuild(inputModels, mlContext, modelPath);
                        if (overall > 0 && overall > bestSequenceValue)
                        {
                            bestSequenceValue = overall;
                            bestSequence = buildSequence;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Something is wrong with {modelPath}");
                        Console.WriteLine(e.Message);
                        ChatService.TagException();
                    }
                }
            }

            if (bestSequenceValue > .5f)
            {
                Console.WriteLine($"Best: {bestSequenceValue}, Build: {string.Join(" ", bestSequence)}");
                ChatService.Tag("MLBuildChoice");
                return bestSequence;
            }

            return null; // if nothing good just do it the old way
        }
    }
}
