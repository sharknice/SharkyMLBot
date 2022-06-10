using SC2APIProtocol;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.EnemyPlayer;

namespace BuildPrediction
{
    public class TrainingBuildDecisionService : RecentBuildDecisionService
    {
        /// <summary>
        /// goes through every available build in order
        /// </summary>
        /// <param name="defaultSharkyBot"></param>
        public TrainingBuildDecisionService(DefaultSharkyBot defaultSharkyBot) : base(defaultSharkyBot)
        {

        }

        public override List<string> GetBestBuild(EnemyPlayer enemyBot, List<List<string>> buildSequences, string map, List<EnemyPlayer> enemyBots, Race enemyRace, Race myRace)
        {
            Console.WriteLine($"Choosing training build against {enemyBot.Name} - {enemyBot.Id} on {map}");

            var lastGame = enemyBot.Games.Where(g => g.MyRace == myRace && g.EnemySelectedRace == enemyRace).FirstOrDefault();
            if (lastGame != null)
            {
                var lastBuildString = string.Join(" ", lastGame.PlannedBuildSequence);
                if (lastGame != null)
                {
                    Console.WriteLine($"{(Result)lastGame.Result} last game with: {lastBuildString}");

                    var index = buildSequences.FindIndex(b => lastBuildString == string.Join(" ", b));
                    if (index + 1 < buildSequences.Count())
                    {
                        var sequence = buildSequences[index + 1];
                        Console.WriteLine($"Training with: {string.Join(" ", sequence)}");
                        return sequence;
                    }
                }
            }

            var firstSequence = buildSequences.FirstOrDefault();
            Console.WriteLine($"Training with: {string.Join(" ", firstSequence)}");
            return firstSequence;
        }
    }
}
