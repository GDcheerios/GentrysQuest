using System;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Location;

namespace GentrysQuest.Game.Database
{
    public class Statistic
    {
        public Statistic(StatTypes statType, int amount = 0)
        {
            StatType = statType;
            ScoreReward = 0;
            Value = amount;

            switch (statType)
            {
                case StatTypes.Heal:
                    Name = "heal";
                    ScoreReward = 1;
                    break;

                case StatTypes.PlayerDamage:
                    Name = "player_damage";
                    ScoreReward = 1;
                    break;

                case StatTypes.EnemyDamage:
                    Name = "enemy_damage";
                    ScoreReward = 2;
                    break;

                case StatTypes.Kill:
                    Name = "kill";
                    ScoreReward = 100;
                    break;

                case StatTypes.Death:
                    Name = "death";
                    break;

                case StatTypes.MoneyGained:
                    Name = "money_gained";
                    ScoreReward = 1;
                    break;

                case StatTypes.MoneySpent:
                    Name = "money_spent";
                    ScoreReward = 1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        public string Name { get; set; }
        public StatTypes StatType { get; }
        public float Value { get; set; } = 0;
        public Enemy Enemy { get; set; } = null;
        public Character Character { get; set; } = null;
        public Weapon Weapon { get; set; } = null;
        public Map Map { get; set; } = null;
        public int? Visitation { get; set; } = null;
        public StatusEffect StatusEffect { get; set; } = null;
        public int? Leaderboard { get; set; } = null;
        public int ScoreReward { get; set; }
    }
}
