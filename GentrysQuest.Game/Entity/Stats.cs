namespace GentrysQuest.Game.Entity
{
    /// <summary>
    /// Stat management class /// </summary>
    public class Stats
    {
        public Stat Health = new HealthStat();
        public Stat Attack = new IntStat("Attack", StatType.Attack);
        public Stat Defense = new IntStat("Defense", StatType.Defense);
        public Stat CritRate = new IntStat("CritRate", StatType.CritRate);
        public Stat CritDamage = new IntStat("CritDamage", StatType.CritDamage);
        public Stat Speed = new("Speed", StatType.Speed);
        public Stat AttackSpeed = new("AttackSpeed", StatType.AttackSpeed);
        public Stat RegenSpeed = new("RegenSpeed", StatType.RegenSpeed);
        public Stat RegenStrength = new IntStat("RegenStrength", StatType.RegenStrength);
        public Stat Tenacity = new IntStat("Tenacity", StatType.Tenacity);
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

        public Stat GetStat(StatType statType)
        {
            foreach (Stat stat in statGrouping)
            {
                if (stat.Type == statType) return stat;
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
