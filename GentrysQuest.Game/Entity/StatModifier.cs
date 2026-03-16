namespace GentrysQuest.Game.Entity
{
    public class StatModifier
    {
        public StatType StatType { get; }
        public double Value { get; }
        public StatModifierOperation Operation { get; }

        public StatModifier(StatType statType, double value, StatModifierOperation operation)
        {
            StatType = statType;
            Value = value;
            Operation = operation;
        }

        public static StatModifier Flat(StatType statType, double value) =>
            new StatModifier(statType, value, StatModifierOperation.Flat);

        public static StatModifier PercentOfDefault(StatType statType, double percent) =>
            new StatModifier(statType, percent, StatModifierOperation.PercentOfDefault);
    }
}
