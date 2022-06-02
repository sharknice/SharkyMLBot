using Microsoft.ML;
using SharkyMLDataManager;
using System.Diagnostics;

namespace BuildPrediction
{
    public class BuildModelManager
    {
        public void UpdateBuildModels()
        {
            Console.WriteLine("Updating Build Models");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var mlContext = new MLContext();

            Console.WriteLine("Loading JSON Data");
            var mlDataFileService = new MLDataFileService();
            Console.WriteLine($"{stopwatch.Elapsed}");

            var buildModelsDirectory = $"data/BuildModels";

            var raceGroups = mlDataFileService.MLGameData.GroupBy(g => string.Join(" ", g.Game.EnemyRace));
            SaveGroupBuilds(stopwatch, mlContext, $"{buildModelsDirectory}/Race", raceGroups);

            var enemyIdGroups = mlDataFileService.MLGameData.GroupBy(g => string.Join(" ", g.Game.EnemyId));
            SaveGroupBuilds(stopwatch, mlContext, $"{buildModelsDirectory}/EnemyId", enemyIdGroups);

            stopwatch.Stop();
            Console.WriteLine($"Done in {stopwatch.Elapsed}");
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
