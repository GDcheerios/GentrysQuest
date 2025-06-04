namespace GentrysQuest.Game.Entity
{
    /// <summary>
    /// Stat management class /// </summary>
    public class Stats
    {
        public readonly Stat Health = new IntStat("Health", StatType.Health, 100, false);
        public readonly Stat Attack = new IntStat("Attack", StatType.Attack, 10);
        public readonly Stat Defense = new IntStat("Defense", StatType.Defense, 100);
        public readonly Stat CritRate = new IntStat("CritRate", StatType.CritRate, 1);
        public readonly Stat CritDamage = new IntStat("CritDamage", StatType.CritDamage, 20);
        public readonly Stat Speed = new("Speed", StatType.Speed, 1);
        public readonly Stat AttackSpeed = new("AttackSpeed", StatType.AttackSpeed, 1);
        public readonly Stat RegenSpeed = new("RegenSpeed", StatType.RegenSpeed, 0);
        public readonly Stat RegenStrength = new IntStat("RegenStrength", StatType.RegenStrength, 1);
        public readonly Stat Tenacity = new IntStat("Tenacity", StatType.Tenacity, 3);
        private readonly Stat[] statGrouping;

        public Stats()
        {
            statGrouping =
            [
                Health,
                Attack,
                Defense,
                CritRate,
                CritDamage,
                Speed,
                AttackSpeed,
                RegenSpeed,
                RegenStrength,
                Tenacity
            ];
        }

        public Stat GetStat(string name)
        {
            foreach (Stat stat in statGrouping)
            {
                if (name == stat.Name) return stat;
            }

            return null;
        }

        public void Boost(int percent)
        {
            foreach (Stat stat in statGrouping) stat.Add(stat.GetPercentFromDefault(percent));
        }

        public Stat[] GetStats() => statGrouping;

        /// <summary>
        /// Restores all stat values to original value
        /// </summary>
        public void Restore()
        {
            foreach (Stat stat in statGrouping) stat.RestoreValue();
        }

        /// <summary>
        /// Resets all additional values
        /// </summary>
        public void ResetAdditionalValues()
        {
            foreach (Stat stat in statGrouping)
            {
                stat.ResetAdditionalValue();
            }
        }

        public int GetPointTotal()
        {
            int points = 1;

            foreach (Stat stat in statGrouping)
            {
                points += stat.Point;
            }

            return points;
        }

        public override string ToString()
        {
            return $"{Health}\n"
                   + $"{Attack}\n"
                   + $"{Defense}\n"
                   + $"{CritRate}\n"
                   + $"{CritDamage}\n"
                   + $"{Speed}\n"
                   + $"{AttackSpeed}";
        }
    }
}
