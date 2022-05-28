using BuildPrediction;
using LINQtoCSV;
using Microsoft.ML;
using SharkyMLDataManager;

Console.WriteLine("Hello, World!");

var mlContext = new MLContext();

Console.WriteLine("Loading JSON Data");
var mlDataFileService = new MLDataFileService();

Console.WriteLine("Converting JSON Data to Flat Data");
var flatFrameDataConverter = new FlatFrameDataConverter();
var convertedData = flatFrameDataConverter.GetFlatFrameData(mlDataFileService.MLGameData);

Console.WriteLine("Saving Flat Data");
var cc = new CsvContext();
cc.Write(convertedData, "flatdata.csv");
Console.WriteLine("Done");

// TODO: create CSV with all the data that can use either code or the model builder to build a model from

// TODO: convert this to FlatFrameData or something

// for predicting what build would work best against this game.  we only want enemy units, not our own?
// what we are really looking for is our build that would have won against the enemy build for the given game
// maybe look at first few minutes of data?

//var trainingDataView = mlContext.Data.LoadFromEnumerable<MLGameData>(mlData); 

//var pipeline = mlContext.Transforms.Concatenate("Features", "MLFramesData")
//                .Append(mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "Game", featureColumnName: "Features"));

//Console.WriteLine("Training the model");
//ITransformer trainedModel = pipeline.Fit(trainingDataView);
//Console.WriteLine("Finish training");