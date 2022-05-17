using Newtonsoft.Json;
using System.Text;

namespace SharkyMLBot.ML
{
    public class MLDataFileService
    {
        public List<MLGameData> MLGameData { get; private set; }

        string LearningDataFolder { get; set; }

        public MLDataFileService()
        {
            LearningDataFolder = Directory.GetCurrentDirectory() + "/data/ml/";
            MLGameData = LoadGames();
        }

        List<MLGameData> LoadGames()
        {
            var games = new List<MLGameData>();
            if (Directory.Exists(LearningDataFolder))
            {
                foreach (var fileName in Directory.GetFiles(LearningDataFolder))
                {
                    using (StreamReader file = File.OpenText(fileName))
                    {
                        var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
                        var game = serializer.Deserialize(file, typeof(MLGameData)) as MLGameData;
                        if (game != null)
                        {
                            games.Add(game);
                        }
                    }
                }
            }
            return games;
        }

        public void SaveGame(MLGameData game)
        {
            string json = JsonConvert.SerializeObject(game, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            });
            if (!Directory.Exists(LearningDataFolder))
            {
                Directory.CreateDirectory(LearningDataFolder);
            }
            File.WriteAllText(LearningDataFolder + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".json", json, Encoding.UTF8);
        }
    }
}
