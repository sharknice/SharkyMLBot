using BuildPrediction;
using LINQtoCSV;
using SharkyMLDataManager;

// You can use this to generate csv's with ml data which you can use with ML.NET Model Builder
// To use this you need files in the data/ml folder (set their properties to copy always)

Console.WriteLine("Creating Models");

var mlDataFileService = new MLDataFileService();
var groups = mlDataFileService.MLGameData.GroupBy(g => string.Join(" ", g.Game.PlannedBuildSequence.Select(g => g)));
foreach (var group in groups)
{
    Console.WriteLine(group.Key);

    Console.WriteLine("Converting JSON Data to Flat Data");
    var flatFrameDataConverter = new FlatFrameDataConverter();
    var convertedData = flatFrameDataConverter.GetFlatFrameData(group);

    var cc = new CsvContext();
    cc.Write(convertedData, $"data/{group.Key}.csv");
}

var buildModelManager = new BuildModelTrainingManager($"data/BuildModels");
buildModelManager.UpdateBuildModels();