using BuildPrediction;
using Microsoft.ML;
using Sharky.Builds.BuildChoosing;
using SharkyMLDataManager;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var buildModelManager = new BuildModelTrainingManager();
buildModelManager.UpdateBuildModels();




//ML build chooser, run each build against last game, use winning build with highest confidence 


// what we are really looking for is our build that would have won against the enemy build for the given game
// maybe look at first few minutes of data?
