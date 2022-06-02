using BuildPrediction;
using SC2APIProtocol;
using Sharky.DefaultBot;
using Sharky.Managers;
using SharkyMLDataManager;

namespace SharkyMLBot.Managers
{
    public class MLBuildManager : BuildManager
    {
        public override bool NeverSkip { get => true; } // make sure saving learning data never gets skipped

        protected Dictionary<int, MLFrameData> MLFramesData { get; set; }
        FrameDataGatherer FrameDataGatherer;
        MLDataFileService MLDataFileService;
        BuildModelTrainingManager BuildModelTrainingManager;

        int LearningDataUpdateRate;

        public MLBuildManager(DefaultSharkyBot defaultSharkyBot, FrameDataGatherer frameDataGatherer, MLDataFileService mLDataFileService, BuildModelTrainingManager buildModelTrainingManager) : base(defaultSharkyBot)
        {
            MLFramesData = new Dictionary<int, MLFrameData>();

            FrameDataGatherer = frameDataGatherer;
            MLDataFileService = mLDataFileService;
            BuildModelTrainingManager = buildModelTrainingManager;

            LearningDataUpdateRate = 1344; // 1 minute
        }

        public override void OnStart(ResponseGameInfo gameInfo, ResponseData data, ResponsePing pingResponse, ResponseObservation observation, uint playerId, string opponentId)
        {
            MLFramesData = new Dictionary<int, MLFrameData>();

            base.OnStart(gameInfo, data, pingResponse, observation, playerId, opponentId);

            // TODO: Use ML to get the counter build to the last game
        }

        public override void OnEnd(ResponseObservation observation, Result result)
        {
            Console.WriteLine($"Build Sequence: {string.Join(" ", BuildHistory.Select(b => b.Value.ToString()))}");

            var game = GetGame(observation, result);
            EnemyPlayerService.SaveGame(game);

            var learningGameData = new MLGameData { Game = game, MLFramesData = MLFramesData };
            MLDataFileService.SaveGame(learningGameData);

            BuildModelTrainingManager.UpdateBuildModels(game);
        }

        public override IEnumerable<SC2APIProtocol.Action> OnFrame(ResponseObservation observation)
        {
            var frame = (int)observation.Observation.GameLoop;
            if (frame % LearningDataUpdateRate == 0)
            {
                MLFramesData[frame] = FrameDataGatherer.GetFrameData(BuildHistory, EnemyStrategyHistory.History);          
            }

            return base.OnFrame(observation);
        }
    }
}
