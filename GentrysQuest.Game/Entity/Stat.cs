using System;
using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Utils;
using osu.Framework.Bindables;

namespace GentrysQuest.Game.Entity
{
    public class Stat
    {
        public readonly string Name;
        public StatType StatType;
        public StatType Type => StatType;

        /// <summary>
        /// This is the bonus stat calculation variable.
        /// Use this in entities UpdateStats method to determine its effect on the calculation.
        /// </summary>
        public int Point;

        public bool IsPercent { get; private set; }

        public Bindable<double> Default { get; set; } = new();
        public Bindable<double> Minimum { get; set; } = new();
        public Bindable<double> Current { get; set; } = new();
        public Bindable<double> Additional { get; set; } = new();

        private readonly Dictionary<string, Func<Stat, double, double>> totalModifiers = new();
        protected double LastTotal = 0;

        public Stat(string name, StatType statType, double minimumValue)
        {
            Name = name;
            StatType = statType;
            Minimum.Value = minimumValue;
            Current.Value = Total();

            IsPercent = statType switch
            {
                StatType.CritRate or StatType.CritDamage => true,
                _ => IsPercent
            };
        }

        public virtual void Recalculate()
        {
            double nextTotal = CalculateTotal();

            Current.Value = ClampCurrent(nextTotal, nextTotal);
        }

        public virtual void ResetAdditionalValue() => Additional.Value = 0;

        public virtual void RestoreValue() => Current.Value = Total();

        public virtual void UpdateCurrentValue(double updateDifference)
        {
            double potentialChange = Current.Value + updateDifference;
            Current.Value = ClampCurrent(potentialChange, Total());
        }

        public virtual void SetDefaultValue(double value)
        {
            Default.Value = ClampDefault(value);
            Recalculate();
        }

        public void Add(double value)
        {
            Additional.Value = TransformAdditional(Additional.Value + value);
            Recalculate();
        }

        public virtual double GetDefault() => Math.Round(Default.Value, 2);
        public virtual double GetAdditional() => Math.Round(Additional.Value, 2);
        public virtual double GetCurrent() => Math.Round(Current.Value, 2);

        public virtual void SetAdditional(double value)
        {
            Additional.Value = TransformAdditional(value);
            Recalculate();
        }

        public double GetPercentFromDefault(float percent) => MathBase.GetPercent(Default.Value, percent);
        public double GetPercentFromAdditional(float percent) => MathBase.GetPercent(Additional.Value, percent);
        public double GetPercentFromTotal(float percent) => MathBase.GetPercent(Total(), percent);

        public virtual double Total() => CalculateTotal();

        public void AddTotalModifier(string key, Func<Stat, double, double> modifier) => totalModifiers[key] = modifier;

        public bool RemoveTotalModifier(string key) => totalModifiers.Remove(key);

        public void ClearTotalModifiers() => totalModifiers.Clear();

        protected virtual double CalculateBaseTotal() => Default.Value + Additional.Value;

        protected virtual double TransformAdditional(double value) => value;

        protected virtual double ClampDefault(double value) => value < Minimum.Value ? Minimum.Value : value;

        protected virtual double ClampCurrent(double value, double maxValue)
        {
            if (value > maxValue) return maxValue;
            if (value < 0) return 0;

            return value;
        }

        protected virtual double RoundTotal(double value) => Math.Round(value, 2);

        public double CalculateTotal()
        {
            double total = CalculateBaseTotal();

            total = totalModifiers.Values.Aggregate(total, (current, modifier) => modifier(this, current));

            LastTotal = total;
            return RoundTotal(total);
        }

        public override string ToString() => $"{Name}: {Default.Value} + {Additional.Value} ({Total()})";
    }
}
