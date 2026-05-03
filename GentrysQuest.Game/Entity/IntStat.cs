namespace GentrysQuest.Game.Entity
{
    public class IntStat : Stat
    {
        public IntStat(string name, StatType statType)
            : base(name, statType)
        {
            // This is how it's meant to be! :)
        }

        public override double GetDefault() => (int)base.GetDefault();
        public override double GetAdditional() => (int)base.GetAdditional();
        public override double GetCurrent() => (int)base.GetCurrent();

        public override double Total()
        {
            return (int)base.Total();
        }
    }
}
