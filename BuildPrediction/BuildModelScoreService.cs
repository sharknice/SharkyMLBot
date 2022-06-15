using Microsoft.ML;
using static BuildPrediction.MLModel1;

namespace BuildPrediction
{
    public class BuildModelScoreService
    {
        public float GetScoreForBuild(List<ModelInput> inputModels, MLContext mlContext, string buildPath)
        {
            var sequenceValue = 0f;
            var maxValue = 0f;

            Console.Write($"{buildPath}: ");

            foreach (var inputModel in inputModels)
            {
                var modelPath = $"{buildPath}/{inputModel.Frame}.zip";
                if (File.Exists(modelPath))
                {
                    var score = GetInputModelValue(inputModel, modelPath, mlContext);
                    sequenceValue += score;
                    maxValue += 1;
                }
            }
            Console.WriteLine();

            var overall = sequenceValue / maxValue;
            Console.WriteLine($"Score: {overall}");
            return overall;
        }

        float GetInputModelValue(ModelInput inputModel, string modelPath, MLContext mlContext)
        {
            var mlModel = mlContext.Model.Load(modelPath, out var _);
            var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            var result = predictionEngine.Predict(inputModel);

            var score = 0f;
            if (result.PredictedLabel == 1)
            {
                score = result.Score.Max();
            }
            Console.Write($"[{inputModel.Frame}, {result.PredictedLabel}, {score}] ");
            return score;
        }
    }
}
