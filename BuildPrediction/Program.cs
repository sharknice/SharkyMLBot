using BuildPrediction;
using Microsoft.ML;
using Sharky.Builds.BuildChoosing;
using SharkyMLDataManager;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var buildModelManager = new BuildModelManager();
buildModelManager.UpdateBuildModels();


//At end of game update data and train ML model for that build
//ML build chooser, run each build against last game, use winning build with highest confidence 


// TODO: train a model for each build
// predict result column
// ignore enemyId and build columns

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