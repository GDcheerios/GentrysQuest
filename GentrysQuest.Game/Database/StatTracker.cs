using System;
using System.Collections.Generic;
using System.Linq;

namespace GentrysQuest.Game.Database
{
    public class StatTracker
    {
        public List<IStatistic> Stats { get; private set; } = new();
        public readonly ScoreStatistic ScoreStatistic;

        public StatTracker()
        {
            ScoreStatistic = new ScoreStatistic();
            Stats.Add(ScoreStatistic);
            Stats.Add(new Statistic(StatTypes.Hits, 10));
            Stats.Add(new Statistic(StatTypes.Damage, 0.1f));
            Stats.Add(new MaxStatistic(StatTypes.MostDamage));
            Stats.Add(new Statistic(StatTypes.Crits, 20));
            Stats.Add(new Statistic(StatTypes.Kills, 100));
            Stats.Add(new Statistic(StatTypes.DamageTaken, 0.1f));
            Stats.Add(new MaxStatistic(StatTypes.MostDamageTaken));
            Stats.Add(new Statistic(StatTypes.HitsTaken, 2));
            Stats.Add(new MaxStatistic(StatTypes.ConsecutiveCrits));
            Stats.Add(new Statistic(StatTypes.CritsTaken));
            Stats.Add(new Statistic(StatTypes.Deaths));
            Stats.Add(new MaxStatistic(StatTypes.MoneySpentOnce));
            Stats.Add(new MaxStatistic(StatTypes.MoneyGainedOnce));
            Stats.Add(new Statistic(StatTypes.MoneySpent, 1));
            Stats.Add(new Statistic(StatTypes.MoneyGained));
            Stats.Add(new Statistic(StatTypes.HealthGained, 0.5f));
            Stats.Add(new MaxStatistic(StatTypes.HealthGainedOnce));
        }

        private void addToStatPattern(IStatistic statistic, int amount)
        {
            ScoreStatistic.Add(statistic.ScoreReward * amount);
            statistic.Add(amount);
        }

        private void addToStatPattern(IStatistic statistic)
        {
            ScoreStatistic.Add(statistic.ScoreReward);
            statistic.Add();
        }

        public StatTracker(List<IStatistic> stats) => this.Stats = stats;
        public IStatistic GetStat(int index) => Stats[index];
        public IStatistic GetStat(StatTypes type) => Stats.FirstOrDefault(t => t.StatType == type);
        public void AddToStat(StatTypes type, int amount) => addToStatPattern(GetStat(type), amount);
        public void AddToStat(StatTypes type) => addToStatPattern(GetStat(type));

        /// <summary>
        /// Merge two StatTrackers together to combine values
        /// </summary>
        /// <param name="otherStats">The other stat tracker</param>
        public void Merge(StatTracker otherStats)
        {
            for (int statIndex = 0; statIndex < Stats.Count; statIndex++)
            {
                Stats[statIndex].Result(otherStats.GetStat(statIndex));
            }
        }

        /// <summary>
        /// Get the best values from two StatTrackers
        /// </summary>
        /// <param name="otherStats">Other stat tracker</param>
        /// <returns>A new StatTracker with the best values for each stat</returns>
        public StatTracker GetBest(StatTracker otherStats)
        {
            List<IStatistic> newStats = new();

            for (int statIndex = 0; statIndex < Stats.Count; statIndex++)
            {
                newStats.Add(compare(Stats[statIndex], otherStats.GetStat(statIndex)));
            }

            return new StatTracker(newStats);
        }

        private static IStatistic compare(IStatistic stat1, IStatistic stat2)
        {
            return (stat1.Value > stat2.Value) ? stat1 : stat2;
        }

        public List<IStatistic> GetStats() => Stats;

        /// <summary>
        /// Logs stat summary to the console
        /// </summary>
        public void Log()
        {
            for (int statIndex = 0; statIndex < Stats.Count; statIndex++)
            {
                Console.WriteLine(Stats[statIndex].Summary());
            }
        }
    }
}
