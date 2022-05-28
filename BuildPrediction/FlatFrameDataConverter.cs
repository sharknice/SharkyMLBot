using Sharky;
using Sharky.EnemyStrategies;
using SharkyMLDataManager;

namespace BuildPrediction
{
    public class FlatFrameDataConverter
    {
        public List<FlatFrameData> GetFlatFrameData(List<MLGameData> mLGameDatas)
        {
            var flatFrames = new List<FlatFrameData>();
            foreach (var mLGameData in mLGameDatas)
            {
                flatFrames.AddRange(GetFlatFrameData(mLGameData));
            }
            return flatFrames;
        }

        public List<FlatFrameData> GetFlatFrameData(MLGameData mLGameData)
        {
            if (mLGameData?.MLFramesData == null || mLGameData.Game == null) { return new List<FlatFrameData>(); }
            
            var flatFrameDatas = new List<FlatFrameData>();
            var enemySeen = false;
            foreach(var mlFrameData in mLGameData.MLFramesData)
            {
                if (!enemySeen)
                {
                    enemySeen = mlFrameData.Value.EnemyUnitCounts != null && mlFrameData.Value.EnemyUnitCounts.Sum(e => e.Value) > 0;
                }
                if (enemySeen)
                {
                    flatFrameDatas.Add(GetFlatFrameData(mlFrameData, mLGameData.Game));
                }
            }
            
            return flatFrameDatas;
        }

        FlatFrameData GetFlatFrameData(KeyValuePair<int, MLFrameData> mlFrameData, Game game)
        {
            var flatFrameData = new FlatFrameData
            {
                Result = game.Result,

                EnemyId = game.EnemyId,
                MapName = game.MapName,
                Build = mlFrameData.Value.BuildHistory?.LastOrDefault().Value,

                EnemySelectedRace = (int)game.EnemySelectedRace,
                EnemyRace = (int)game.EnemyRace,
                MySelectedRace = (int)game.MySelectedRace,
                MyRace = (int)game.MyRace,

                Frame = mlFrameData.Key,

                ProxyDetected = mlFrameData.Value.DetectedEnemyStrategies != null && mlFrameData.Value.DetectedEnemyStrategies.Contains(nameof(Proxy)),
                ProxyActive = mlFrameData.Value.ActiveEnemyStrategies != null && mlFrameData.Value.ActiveEnemyStrategies.Contains(nameof(Proxy)),
            };

            var type = typeof(FlatFrameData);
            foreach (var field in type.GetFields())
            {
                var fieldInfo = type.GetField(field.Name);
                if (fieldInfo != null)
                {
                    if (field.Name.StartsWith("SELF_"))
                    {
                        var count = 0;
                        var unitTypeString = field.Name.Replace("SELF_", "");
                        var unitType = Enum.Parse(typeof(UnitTypes), unitTypeString);
                        if (mlFrameData.Value.SelfUnitCounts != null && mlFrameData.Value.SelfUnitCounts.TryGetValue((UnitTypes)unitType, out count))
                        {

                        }
                        fieldInfo.SetValue(flatFrameData, count);
                    }
                    else if (field.Name.StartsWith("ENEMY_"))
                    {
                        var count = 0;
                        var unitTypeString = field.Name.Replace("ENEMY_", "");
                        var unitType = Enum.Parse(typeof(UnitTypes), unitTypeString);
                        if (mlFrameData.Value.EnemyUnitCounts != null && mlFrameData.Value.EnemyUnitCounts.TryGetValue((UnitTypes)unitType, out count))
                        {

                        }
                        fieldInfo.SetValue(flatFrameData, count);
                    }
                }
            }

            return flatFrameData;
        }
    }
}
