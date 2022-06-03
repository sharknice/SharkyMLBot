using Microsoft.ML;
using SC2APIProtocol;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.EnemyPlayer;
using SharkyMLDataManager;
using static BuildPrediction.MLModel1;

namespace BuildPrediction
{
    public class MLBuildDecisionService : RecentBuildDecisionService
    {
        public string BuildModelsDirectory { get; set; }

        MLDataFileService MLDataFileService;
        GameDataToModelInputConverter GameDataToModelInputConverter;

        public MLBuildDecisionService(DefaultSharkyBot defaultSharkyBot, MLDataFileService mLDataFileService, GameDataToModelInputConverter gameDataToModelInputConverter)
            : base(defaultSharkyBot)
        {
            MLDataFileService = mLDataFileService;
            GameDataToModelInputConverter = gameDataToModelInputConverter;

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
                        var counter = GetBestCounterBuild(buildSequences, lastGameMlData, map);
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

        List<string>? GetBestCounterBuild(List<List<string>> buildSequences, MLGameData gameData, string mapName)
        {
            if (gameData.Game == null) { return null; }
            var lastGame = gameData.Game;
            var directory = $"{BuildModelsDirectory}/Race/{lastGame.EnemyRace}";

            var inputModels = GameDataToModelInputConverter.GetModelInputs(gameData, mapName);
            var mlContext = new MLContext();

            List<string> bestSequence = null;
            var bestSequenceValue = 0f;

            foreach (var buildSequence in buildSequences)
            {
                var buildString = string.Join(" ", buildSequence);
                var modelPath = $"{directory}/{buildString}.zip";
                if (File.Exists(modelPath))
                {
                    var mlModel = mlContext.Model.Load(modelPath, out var _);
                    var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

                    var sequenceValue = 0f;
                    var maxValue = 0f;

                    Console.WriteLine($"Predicting {buildString}");
                    
                    foreach (var inputModel in inputModels)
                    {                                            
                        var result = predictionEngine.Predict(inputModel);

                        var score = 0f;
                        if (result.PredictedLabel == 1)
                        {
                            score = result.Score.Max();
                        }
                        sequenceValue += score;
                        maxValue += 1;

                        Console.WriteLine($"Frame {inputModel.Frame}, Result: {result.PredictedLabel}, Score: {score}");
                    }

                    var overall = sequenceValue / maxValue;
                    Console.WriteLine($"{buildString}: {overall}");
                    if (overall > 0 && overall > bestSequenceValue)
                    {
                        bestSequenceValue = overall;
                        bestSequence = buildSequence;
                    }
                }
            }

            if (bestSequenceValue > .5f)
            {
                return bestSequence;
            }

            return null; // if nothing good just do it the old way
        }
    }
}
