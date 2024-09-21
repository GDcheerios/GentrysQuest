using GentrysQuest.Game.Database;

namespace GentrysQuest.Game.Entity;

public class Character : Entity
{
    public ArtifactManager Artifacts { get; }

    public Character()
    {
        Artifacts = new ArtifactManager(this);
        Artifacts.OnChangeArtifact += UpdateStats;
    }

    public override void Damage(int amount)
    {
        base.Damage(amount);
        GameData.CurrentStats.AddToStat(StatTypes.DamageTaken, amount);
        GameData.CurrentStats.AddToStat(StatTypes.MostDamageTaken, amount);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        GameData.CurrentStats.AddToStat(StatTypes.HealthGained, (int)(amount * HealingModifier));
        GameData.CurrentStats.AddToStat(StatTypes.HealthGainedOnce, (int)(amount * HealingModifier));
    }

    public override void Die()
    {
        base.Die();
        GameData.CurrentStats.AddToStat(StatTypes.Deaths);
        GameData.CurrentStats.Log();
    }

    public override void UpdateStats()
    {
        Stats.ResetAdditionalValues();
        int level = Experience.Level.Current.Value;
        int starRating = StarRating.Value;

        Stats.Health.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 1000, Stats.Health.point, 250) +
            CalculatePointBenefit(level * 50, Stats.Health.point, 25) +
            CalculatePointBenefit(starRating * 15, Stats.Health.point, 25)
        );

        Stats.Attack.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 8, Stats.Attack.point, 5) +
            CalculatePointBenefit(level * 1, Stats.Attack.point, 4) +
            CalculatePointBenefit(starRating, Stats.Attack.point, 3)
        );

        Stats.Defense.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 6, Stats.Defense.point, 4) +
            CalculatePointBenefit(level * 1, Stats.Defense.point, 2) +
            CalculatePointBenefit(starRating, Stats.Defense.point, 3)
        );

        Stats.CritRate.SetDefaultValue(
            CalculatePointBenefit(0, Stats.CritRate.point, 5)
        );

        Stats.CritDamage.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 5, Stats.CritDamage.point, 5) +
            CalculatePointBenefit(starRating, Stats.CritDamage.point, 2)
        );

        Stats.Speed.SetDefaultValue(
            1 +
            CalculatePointBenefit(0, Stats.Speed.point, 0.2)
        );

        Stats.AttackSpeed.SetDefaultValue(
            CalculatePointBenefit(0, Stats.AttackSpeed.point, 0.3)
        );

        Stats.RegenSpeed.SetDefaultValue(
            1 +
            CalculatePointBenefit(0, Stats.RegenSpeed.point, 1)
        );

        Stats.RegenStrength.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 1, Stats.RegenStrength.point, 1)
        );

        if (Weapon != null) AddToStat(Weapon.Buff);

        foreach (Artifact artifact in Artifacts.Get())
        {
            if (artifact != null)
            {
                AddToStat(artifact.MainAttribute);

                foreach (Buff attribute in artifact.Attributes)
                {
                    AddToStat(attribute);
                }
            }
        }

        if (Stats.CritRate.Total() > 100) Stats.CritDamage.Add(100 - Stats.CritRate.Total());

        base.UpdateStats();
    }

    public Enemy CreateEnemy(WeaponChoices weaponChoices)
    {
        Enemy enemy = new Enemy();
        enemy.Name = Name;
        enemy.TextureMapping.Remove("Idle");
        enemy.TextureMapping.Add("Idle", TextureMapping.Get("Idle"));
        enemy.Secondary = Secondary;
        enemy.Utility = Utility;
        enemy.Ultimate = Ultimate;
        enemy.WeaponChoices = weaponChoices;
        return enemy;
    }
}
