using BuildPrediction;
using SC2APIProtocol;
using Sharky;
using Sharky.DefaultBot;
using Sharky.Managers;
using SharkyMLBot;
using SharkyMLBot.Managers;
using SharkyMLDataManager;

Console.WriteLine("Starting TwoBaseVoidrays SharkyMLBot");

var gameConnection = new GameConnection();
var defaultSharkyBot = new DefaultSharkyBot(gameConnection);

var buildChoices = new ProtossBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Protoss] = buildChoices.TwoBaseVoidRaysBuildChoices;

var frameDataGatherer = new FrameDataGatherer(defaultSharkyBot);
var mLDataFileService = new MLDataFileService();
var buildModelTrainingManager = new BuildModelTrainingManager();
var buildManager = new MLBuildManager(defaultSharkyBot, frameDataGatherer, mLDataFileService, buildModelTrainingManager);
defaultSharkyBot.Managers.RemoveAll(m => m.GetType() == typeof(BuildManager));
defaultSharkyBot.Managers.Add(buildManager);

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