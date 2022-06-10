using BuildPrediction.Managers;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.Managers;
using SharkyMLDataManager;

namespace BuildPrediction
{
    public class DefaultSharkyMLModule
    {
        public FrameDataGatherer FrameDataGatherer { get; set; }
        public MLDataFileService MLDataFileService { get; set; }
        public BuildModelTrainingManager BuildModelTrainingManager { get; set; }
        public GameDataToModelInputConverter GameDataToModelInputConverter { get; set; }
        public BuildModelScoreService BuildModelScoreService { get; set; }
        public MLBuildManager MLBuildManager { get; set; }
        public IBuildDecisionService MLBuildDecisionService { get; set; }

        public DefaultSharkyMLModule(DefaultSharkyBot defaultSharkyBot)
        {
            var buildModelsDirectory = $"data/BuildModels";

            FrameDataGatherer = new FrameDataGatherer(defaultSharkyBot);
            MLDataFileService = new MLDataFileService();
            BuildModelTrainingManager = new BuildModelTrainingManager(buildModelsDirectory);
            GameDataToModelInputConverter = new GameDataToModelInputConverter();
            BuildModelScoreService = new BuildModelScoreService();
            MLBuildManager = new MLBuildManager(defaultSharkyBot, FrameDataGatherer, MLDataFileService, BuildModelTrainingManager);
            MLBuildDecisionService = new MLBuildDecisionService(defaultSharkyBot, MLDataFileService, GameDataToModelInputConverter, BuildModelTrainingManager, BuildModelScoreService, buildModelsDirectory);
        }

        public DefaultSharkyBot ActivateMachineLearning(DefaultSharkyBot defaultSharkyBot)
        {
            defaultSharkyBot.BuildDecisionService = MLBuildDecisionService;
            defaultSharkyBot.BuildManager.BuildDecisionService = MLBuildDecisionService;

            MLBuildManager.BuildDecisionService = MLBuildDecisionService;
            defaultSharkyBot.Managers.RemoveAll(m => m.GetType() == typeof(BuildManager));
            defaultSharkyBot.Managers.Add(MLBuildManager);

            return defaultSharkyBot;
        }
    }
}
