using Microsoft.ML;
using static BuildPrediction.MLModel1;

namespace BuildPrediction
{
    public class BuildModelScoreService
    {
        public float GetScoreForBuildModel(List<ModelInput> inputModels, MLContext mlContext, string modelPath)
        {
            var mlModel = mlContext.Model.Load(modelPath, out var _);
            var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            return GetInputModelsValue(inputModels, modelPath, predictionEngine);
        }

        float GetInputModelsValue(List<ModelInput> inputModels, string modelPath, PredictionEngine<ModelInput, ModelOutput> predictionEngine)
        {
            var sequenceValue = 0f;
            var maxValue = 0f;

            Console.Write($"{modelPath}: ");

            foreach (var inputModel in inputModels)
            {
                var result = predictionEngine.Predict(inputModel);

                var score = 0f;
                if (result.PredictedLabel == 1)
                {
                    score = result.Score.Max();
                }
                sequenceValue += score;
                maxValue += 1;

                Console.Write($"[{inputModel.Frame}, {result.PredictedLabel}, {score}] ");
            }
            Console.WriteLine();

            var overall = sequenceValue / maxValue;
            Console.WriteLine($"Score: {overall}");
            return overall;
        }
    }
}
