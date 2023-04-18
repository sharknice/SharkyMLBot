using Microsoft.ML;
using Sharky;
using SharkyMLDataManager;
using System.Diagnostics;

namespace BuildPrediction
{
    public class BuildModelTrainingManager
    {
        public string BuildModelsDirectory { get; set; }

        public BuildModelTrainingManager(string buildModelsDirectory = $"data/BuildModels")
        {
            BuildModelsDirectory = buildModelsDirectory;
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

            Console.WriteLine("Loading ML JSON Data");
            var mlDataFileService = new MLDataFileService();

            var raceGroups = mlDataFileService.MLGameData.GroupBy(g => string.Join(" ", g.Game.MyRace));
            foreach (var raceGroup in raceGroups)
            {
                var matchupGroups = raceGroup.GroupBy(g => string.Join(" ", g.Game.EnemyRace));
                SaveGroupBuilds(stopwatch, mlContext, $"{BuildModelsDirectory}/{raceGroup.Key}/Race", matchupGroups);

                var enemyIdGroups = raceGroup.GroupBy(g => string.Join(" ", g.Game.EnemyId));
                SaveGroupBuilds(stopwatch, mlContext, $"{BuildModelsDirectory}/{raceGroup.Key}/EnemyId", enemyIdGroups);
            }

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
            var buildString = string.Join(" ", game.PlannedBuildSequence.Select(g => g));

            var mlGameData = mlDataFileService.MLGameData.Where(g => g.Game.MyRace == game.MyRace && string.Join(" ", g.Game.PlannedBuildSequence.Select(g => g)) == buildString);

            var raceGroups = mlGameData.Where(g => g.Game.EnemyRace == game.EnemyRace).GroupBy(g => string.Join(" ", g.Game.EnemyRace));
            SaveGroupBuilds(stopwatch, mlContext, $"{BuildModelsDirectory}/{game.MyRace}/Race", raceGroups);

            var enemyIdGroups = mlGameData.Where(g => g.Game.EnemyId == game.EnemyId).GroupBy(g => string.Join(" ", g.Game.EnemyId));
            SaveGroupBuilds(stopwatch, mlContext, $"{BuildModelsDirectory}/{game.MyRace}/EnemyId", enemyIdGroups);

            stopwatch.Stop();
            Console.WriteLine($"Updated {buildString} build models against {game.EnemyRace} and {game.EnemyId} in {stopwatch.Elapsed}");
        }

        private static void SaveGroupBuilds(Stopwatch stopwatch, MLContext mlContext, string directory, IEnumerable<IGrouping<string, MLGameData>> groups)
        {
            foreach (var group in groups)
            {
                var buildGroups = group.GroupBy(g => string.Join(" ", g.Game.PlannedBuildSequence.Select(g => g)));
                SaveBuilds(stopwatch, mlContext, $"{directory}/{group.Key}", buildGroups);
            }
        }

        private static void SaveBuilds(Stopwatch stopwatch, MLContext mlContext, string directory, IEnumerable<IGrouping<string, MLGameData>> buildGroups)
        {
            Directory.CreateDirectory(directory);

            foreach (var group in buildGroups)
            {
                var flatFrameDataConverter = new FlatFrameDataConverter();
                var convertedData = flatFrameDataConverter.GetFlatFrameData(group);

                var dataByFrame = convertedData.GroupBy(d => d.Frame);
                foreach (var frameGroup in dataByFrame)
                {
                    var trainingDataView = mlContext.Data.LoadFromEnumerable(frameGroup);
                    if (frameGroup.Any(g => g.Result == 1) && frameGroup.Any(g => g.Result == 2))
                    {
                        try
                        {
                            Directory.CreateDirectory(directory);
                            Directory.CreateDirectory($"{directory}/{group.Key}");
                            var modelPath = $"{directory}/{group.Key}/{frameGroup.Key}.zip";
                            var trainedModel = MLModel1.RetrainPipeline(mlContext, trainingDataView);
                            mlContext.Model.Save(trainedModel, trainingDataView.Schema, modelPath);
                            Console.WriteLine($"{modelPath}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Problem training model for {group.Key}/{frameGroup.Key}");
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }
    }
}
