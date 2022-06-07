using Sharky;
using SharkyMLDataManager;
using static BuildPrediction.MLModel1;

namespace BuildPrediction
{
    public class GameDataToModelInputConverter
    {
        public List<ModelInput> GetModelInputs(MLGameData gameData, string mapName)
        {
            var modelInputs = new List<ModelInput>();

            if (gameData.MLFramesData == null) { return modelInputs; }
            foreach (var frameData in gameData.MLFramesData)
            {
                var data = frameData.Value;
                if (data?.EnemyUnitCounts == null || data.EnemyUnitCounts.Sum(e => e.Value) < 1 || data.DetectedEnemyStrategies == null || data.ActiveEnemyStrategies == null) { continue; }

                var modelInput = GetModelInput(gameData, frameData, mapName);
                modelInputs.Add(modelInput);
            }

            return modelInputs;
        }

        public ModelInput? GetModelInput(MLGameData gameData, KeyValuePair<int, MLFrameData> frameData, string mapName)
        {
            var data = frameData.Value;

            var modelInput = new ModelInput()
            {
                MapName = mapName,
                EnemySelectedRace = (float)gameData.Game.EnemySelectedRace,
                EnemyRace = (float)gameData.Game.EnemyRace,
                MySelectedRace = (float)gameData.Game.MySelectedRace,
                MyRace = (float)gameData.Game.MyRace,
                Frame = frameData.Key,
                ProxyDetected = data.DetectedEnemyStrategies.Contains("Proxy"),
                ProxyActive = data.ActiveEnemyStrategies.Contains("Proxy"),
            };

            var type = typeof(ModelInput);
            foreach (var field in type.GetMethods())
            {
                var fieldInfo = type.GetMethod(field.Name);
                if (fieldInfo != null)
                {
                    if (field.Name.StartsWith("set_ENEMY_"))
                    {
                        var unitTypeString = field.Name.Replace("set_ENEMY_", "");
                        var unitType = Enum.Parse(typeof(UnitTypes), unitTypeString);
                        var count = GetEnemyUnitCount(data, (UnitTypes)unitType);
                        fieldInfo.Invoke(modelInput, new object[] { count });
                    }
                }
            }

            return modelInput;
        }

        int GetEnemyUnitCount(MLFrameData? data, UnitTypes unitType)
        {
            if (data?.EnemyUnitCounts == null) { return 0; }

            if (data.EnemyUnitCounts.TryGetValue(unitType, out var result))
            {
                return result;
            }
            return 0;
        }
    }
}
