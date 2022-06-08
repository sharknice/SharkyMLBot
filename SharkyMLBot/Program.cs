using BuildPrediction;
using SC2APIProtocol;
using Sharky;
using Sharky.DefaultBot;
using SharkyMLBot;

Console.WriteLine("Starting SharkyMLBot");

var gameConnection = new GameConnection();
var defaultSharkyBot = new DefaultSharkyBot(gameConnection);

var buildChoices = new ProtossBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Protoss] = buildChoices.DefaultBuildChoices;

var defaultSharkyMLModule = new DefaultSharkyMLModule(defaultSharkyBot);
defaultSharkyBot = defaultSharkyMLModule.ActivateMachineLearning(defaultSharkyBot);

var bot = defaultSharkyBot.CreateBot(defaultSharkyBot.Managers, defaultSharkyBot.DebugService);

var myRace = Race.Protoss;
if (args.Length == 0)
{
    gameConnection.RunSinglePlayer(bot, @"HardwireAIE.SC2Map", myRace, Race.Random, Difficulty.VeryHard, AIBuild.Macro, 0).Wait();
}
else
{
    gameConnection.RunLadder(bot, myRace, args).Wait();
}