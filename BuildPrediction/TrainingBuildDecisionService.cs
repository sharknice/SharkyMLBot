using SC2APIProtocol;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.EnemyPlayer;

namespace BuildPrediction
{
    public class TrainingBuildDecisionService : RecentBuildDecisionService
    {
        public int TimesPerLoop { get; set; }

        /// <summary>
        /// goes through every available build in order
        /// </summary>
        /// <param name="defaultSharkyBot"></param>
        /// <param name="timesPerLoop">how many times to play a build in a row before going on to the next</param>
        public TrainingBuildDecisionService(DefaultSharkyBot defaultSharkyBot, int timesPerLoop = 1) : base(defaultSharkyBot)
        {
            TimesPerLoop = timesPerLoop;
        }

        public override List<string> GetBestBuild(EnemyPlayer enemyBot, List<List<string>> buildSequences, string map, List<EnemyPlayer> enemyBots, Race enemyRace, Race myRace)
        {
            Console.WriteLine($"Choosing training build against {enemyBot.Name} - {enemyBot.Id} on {map}");

            var lastGame = enemyBot.Games.Where(g => g.MyRace == myRace && g.EnemySelectedRace == enemyRace).FirstOrDefault();
            if (lastGame != null)
            {
                var lastBuildString = string.Join(" ", lastGame.PlannedBuildSequence);
                Console.WriteLine($"{(Result)lastGame.Result} last game with: {lastBuildString}");
                var count = 0;
                foreach (var game in enemyBot.Games.Where(g => g.MyRace == myRace && g.EnemySelectedRace == enemyRace))
                {
                    if (lastBuildString == string.Join(" ", game.PlannedBuildSequence))
                    {
                        count++;
                        if (count >= TimesPerLoop)
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Trained {count} out of {TimesPerLoop}");
                        Console.WriteLine($"Continuing training with: {lastBuildString}");
                        return lastGame.PlannedBuildSequence;
                    }
                }

                var index = buildSequences.FindIndex(b => lastBuildString == string.Join(" ", b));
                if (index + 1 < buildSequences.Count())
                {
                    var sequence = buildSequences[index + 1];
                    Console.WriteLine($"Training with: {string.Join(" ", sequence)}");
                    return sequence;
                }
            }

            var firstSequence = buildSequences.FirstOrDefault();
            Console.WriteLine($"Training with: {string.Join(" ", firstSequence)}");
            return firstSequence;
        }
    }
}
