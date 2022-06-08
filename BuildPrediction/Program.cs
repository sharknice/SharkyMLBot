using BuildPrediction;
//using LINQtoCSV;
using SharkyMLDataManager;

Console.WriteLine("Hello, World!");

var mlDataFileService = new MLDataFileService();
var groups = mlDataFileService.MLGameData.GroupBy(g => string.Join(" ", g.Game.Builds.Select(g => g.Value)));
foreach (var group in groups)
{
    Console.WriteLine(group.Key);

    Console.WriteLine("Converting JSON Data to Flat Data");
    var flatFrameDataConverter = new FlatFrameDataConverter();
    var convertedData = flatFrameDataConverter.GetFlatFrameData(group);

    //var cc = new CsvContext();
    //cc.Write(convertedData, $"data/{group.Key}.csv");
}

var buildModelManager = new BuildModelTrainingManager($"data/BuildModels");
buildModelManager.UpdateBuildModels();