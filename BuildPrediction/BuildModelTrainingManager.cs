using Microsoft.ML;
using Sharky;
using SharkyMLDataManager;
using System.Diagnostics;

namespace BuildPrediction
{
    public class BuildModelTrainingManager
    {
        public string BuildModelsDirectory { get; set; }

        public BuildModelTrainingManager()
        {
            BuildModelsDirectory = $"data/BuildModels";
        }

        /// <summary>
        /// Updates all the build models
        /// </summary>
        public void UpdateBuildModels()
        {
            Console.WriteLine("Updating Build Models");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var mlContext = new MLContext();

            Console.WriteLine("Loading JSON Data");
            var mlDataFileService = new MLDataFileService();
            Console.WriteLine($"{stopwatch.Elapsed}");

            var raceGroups = mlDataFileService.MLGameData.GroupBy(g => string.Join(" ", g.Game.EnemyRace));
            SaveGroupBuilds(stopwatch, mlContext, $"{BuildModelsDirectory}/Race", raceGroups);

            var enemyIdGroups = mlDataFileService.MLGameData.GroupBy(g => string.Join(" ", g.Game.EnemyId));
            SaveGroupBuilds(stopwatch, mlContext, $"{BuildModelsDirectory}/EnemyId", enemyIdGroups);

            stopwatch.Stop();
            Console.WriteLine($"Done in {stopwatch.Elapsed}");
        }

        /// <summary>
        /// Updates the build models affected by this game 
        /// </summary>
        /// <param name="game"></param>
        public void UpdateBuildModels(Game game)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var mlContext = new MLContext();
            var mlDataFileService = new MLDataFileService();
            var buildString = string.Join(" ", game.Builds.Select(g => g.Value));

            var mlGameData = mlDataFileService.MLGameData.Where(g => string.Join(" ", g.Game.Builds.Select(g => g.Value)) == buildString);

            var raceGroups = mlGameData.Where(g => g.Game.EnemyRace == game.EnemyRace).GroupBy(g => string.Join(" ", g.Game.EnemyRace));
            SaveGroupBuilds(stopwatch, mlContext, $"{BuildModelsDirectory}/Race", raceGroups);

            var enemyIdGroups = mlGameData.Where(g => g.Game.EnemyId == game.EnemyId).GroupBy(g => string.Join(" ", g.Game.EnemyId));
            SaveGroupBuilds(stopwatch, mlContext, $"{BuildModelsDirectory}/EnemyId", enemyIdGroups);

            stopwatch.Stop();
            Console.WriteLine($"Updated {buildString} build models for {game.EnemyRace} and {game.EnemyId} in {stopwatch.Elapsed}");
        }

        private static void SaveGroupBuilds(Stopwatch stopwatch, MLContext mlContext, string directory, IEnumerable<IGrouping<string, MLGameData>> groups)
        {
            foreach (var group in groups)
            {
                Console.WriteLine(group.Key);
                var buildGroups = group.GroupBy(g => string.Join(" ", g.Game.Builds.Select(g => g.Value)));
                SaveBuilds(stopwatch, mlContext, $"{directory}/{group.Key}", buildGroups);
            }
        }

        private static void SaveBuilds(Stopwatch stopwatch, MLContext mlContext, string directory, IEnumerable<IGrouping<string, MLGameData>> buildGroups)
        {
            Directory.CreateDirectory(directory);

            foreach (var group in buildGroups)
            {
                Console.WriteLine(group.Key);

                Console.WriteLine("Converting JSON Data to Flat Data");
                var flatFrameDataConverter = new FlatFrameDataConverter();
                var convertedData = flatFrameDataConverter.GetFlatFrameData(group);
                Console.WriteLine($"{stopwatch.Elapsed}");

                Console.WriteLine("Generating Model");
                var trainingDataView = mlContext.Data.LoadFromEnumerable(convertedData);
                if (convertedData.Count > 0)
                {
                    var trainedModel = MLModel1.RetrainPipeline(mlContext, trainingDataView);
                    Console.WriteLine($"{stopwatch.Elapsed}");

                    Console.WriteLine("Saving Model");
                    System.IO.Directory.CreateDirectory("data/buildmodels");
                    mlContext.Model.Save(trainedModel, trainingDataView.Schema, $"{directory}/{group.Key}.zip");
                    Console.WriteLine($"{stopwatch.Elapsed}");
                }
            }
        }
    }
}
