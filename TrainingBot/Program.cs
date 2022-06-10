using BuildPrediction;
using SC2APIProtocol;
using Sharky;
using Sharky.DefaultBot;
using SharkyMLBot;

// this bot can be used to gather ML data
// it repeatedly loops through the build list, playing each build once before moving on to the next

Console.WriteLine("Starting ML Training Bot");

var gameConnection = new GameConnection();
var defaultSharkyBot = new DefaultSharkyBot(gameConnection);

var buildChoices = new ProtossBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Protoss] = buildChoices.DefaultBuildChoices;

var defaultSharkyMLModule = new DefaultSharkyMLModule(defaultSharkyBot);
defaultSharkyMLModule.MLBuildDecisionService = new TrainingBuildDecisionService(defaultSharkyBot); // loops through the build list, tries each build once
defaultSharkyBot = defaultSharkyMLModule.ActivateMachineLearning(defaultSharkyBot);

var bot = defaultSharkyBot.CreateBot(defaultSharkyBot.Managers, defaultSharkyBot.DebugService);

var myRace = Race.Protoss;
if (args.Length == 0)
{
    gameConnection.RunSinglePlayer(bot, @"HardwireAIE.SC2Map", myRace, Race.Protoss, Difficulty.VeryHard, AIBuild.Macro, 0).Wait();
}
else
{
    gameConnection.RunLadder(bot, myRace, args).Wait();
}