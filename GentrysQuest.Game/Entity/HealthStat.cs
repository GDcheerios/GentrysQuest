namespace GentrysQuest.Game.Entity
{
    public class HealthStat : IntStat
    {
        private double previousMaxHealth;

        public HealthStat()
            : base("Health", StatType.Health) =>
            previousMaxHealth = Total();

        public override void Recalculate()
        {
            int newTotal = (int)CalculateTotal();
            int addedMaxHealth = newTotal - (int)previousMaxHealth;
            double nextCurrent = Current.Value;

            if (addedMaxHealth > 0)
                nextCurrent += addedMaxHealth;

            Current.Value = ClampCurrent(nextCurrent, newTotal);
            previousMaxHealth = newTotal;
        }
    }
}
