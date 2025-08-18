using System;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Utils;
using osu.Framework.Bindables;

namespace GentrysQuest.Game.Entity
{
    public class Buff
    {
        public StatType StatType { get; private set; }
        public bool IsPercent { get; private set; }
        public int Level { get; private set; } = 1;
        public Bindable<double> Value { get; private set; } = new Bindable<double>(0);
        public EntityBase ParentEntity;

        public Buff(EntityBase parentEntity)
        {
            ParentEntity = parentEntity;
            StatType = GetRandomStat();
            IsPercent = MathBase.RandomBool();
            updateStats();
        }

        public Buff(EntityBase parentEntity, StatType statType, bool isPercent)
        {
            ParentEntity = parentEntity;
            StatType = statType;
            IsPercent = isPercent;
            updateStats();
        }

        public Buff(EntityBase parentEntity, StatType statType)
        {
            ParentEntity = parentEntity;
            StatType = statType;
            IsPercent = MathBase.RandomBool();
            updateStats();
        }

        public Buff(double amount, StatType statType, bool isPercent)
        {
            Value.Value = amount;
            StatType = statType;
            IsPercent = isPercent;
        }

        public Buff(JsonBuff jsonBuff) => LoadJson(jsonBuff);

        public JsonBuff ToJson()
        {
            return new JsonBuff
            {
                BuffID = StatType,
                IsPercent = IsPercent,
                Level = Level
            };
        }

        public void LoadJson(JsonBuff jsonBuff)
        {
            StatType = jsonBuff.BuffID;
            IsPercent = jsonBuff.IsPercent;
            Level = jsonBuff.Level;
            updateStats();
        }

        private void updateStats()
        {
            double value = 0;
            double starRating = 0;
            double level = 0;
            double percentDiffer = 1;

            switch (StatType)
            {
                case StatType.Health:
                    value = 100;
                    starRating = 125;
                    level = 150;
                    percentDiffer = 65;
                    break;

                case StatType.Attack:
                    value = 10;
                    starRating = 10;
                    level = 5;
                    percentDiffer = 1.5;
                    break;

                case StatType.Defense:
                    value = 2;
                    starRating = 5;
                    level = 5;
                    percentDiffer = 1.7;
                    break;

                case StatType.CritRate:
                    value = 2;
                    starRating = 2;
                    level = 1;
                    IsPercent = false;
                    break;

                case StatType.CritDamage:
                    value = 2;
                    starRating = 2;
                    level = 2;
                    IsPercent = false;
                    break;

                case StatType.Speed:
                    value = 0.05;
                    starRating = 0.025;
                    level = 0.06;
                    IsPercent = false;
                    break;

                case StatType.AttackSpeed:
                    value = 0.05;
                    starRating = 0.025;
                    level = 0.06;
                    IsPercent = false;
                    break;

                case StatType.RegenSpeed:
                    value = 0.1;
                    starRating = 0.2;
                    level = 0.05;
                    IsPercent = false;
                    break;

                case StatType.RegenStrength:
                    value = 1;
                    starRating = 1;
                    level = 1;
                    IsPercent = false;
                    break;

                case StatType.Tenacity:
                    value = 0;
                    starRating = 1;
                    level = 1;
                    IsPercent = false;
                    break;
            }

            if (!IsPercent) percentDiffer = 1;
            handleValue(value, starRating, level, percentDiffer);
        }

        private void handleValue(double value, double starRating, double level, double percentDiffer)
        {
            if (ParentEntity == null) return;

            switch (ParentEntity)
            {
                case Weapon.Weapon weapon:
                    setValue(Level * (weapon.Difficulty + 1));
                    break;

                default:
                    setValue(((ParentEntity.StarRating.Value * starRating) + ((Level - 1) * level) + value) / percentDiffer);
                    break;
            }
        }

        private void setValue(double value)
        {
            Value.Value = Math.Round(value, 2);
        }

        public void Improve()
        {
            Level++;
            updateStats();
        }

        public static StatType GetRandomStat()
        {
            Random random = new Random();
            Array values = Enum.GetValues(typeof(StatType));
            StatType randomType = (StatType)values.GetValue(random.Next(values.Length))!;
            return randomType;
        }
    }
}
