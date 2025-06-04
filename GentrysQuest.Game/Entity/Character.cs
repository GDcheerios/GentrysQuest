using GentrysQuest.Game.IO;

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
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
    }

    public override void Die()
    {
        base.Die();
    }

    public override void UpdateStats()
    {
        Stats.ResetAdditionalValues();
        int level = Experience.Level.Current.Value;
        int starRating = StarRating.Value;

        Stats.Health.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 1000, Stats.Health.Point, 250) +
            CalculatePointBenefit(level * 50, Stats.Health.Point, 25) +
            CalculatePointBenefit(starRating * 15, Stats.Health.Point, 25)
        );

        Stats.Attack.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 8, Stats.Attack.Point, 5) +
            CalculatePointBenefit(level * 1, Stats.Attack.Point, 4) +
            CalculatePointBenefit(starRating, Stats.Attack.Point, 3)
        );

        Stats.Defense.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 6, Stats.Defense.Point, 4) +
            CalculatePointBenefit(level * 1, Stats.Defense.Point, 2) +
            CalculatePointBenefit(starRating, Stats.Defense.Point, 3)
        );

        Stats.CritRate.SetDefaultValue(
            CalculatePointBenefit(0, Stats.CritRate.Point, 5)
        );

        Stats.CritDamage.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 5, Stats.CritDamage.Point, 5) +
            CalculatePointBenefit(starRating, Stats.CritDamage.Point, 2)
        );

        Stats.Speed.SetDefaultValue(
            1 +
            CalculatePointBenefit(0, Stats.Speed.Point, 0.2)
        );

        Stats.AttackSpeed.SetDefaultValue(
            CalculatePointBenefit(0, Stats.AttackSpeed.Point, 0.3)
        );

        Stats.RegenSpeed.SetDefaultValue(
            1 +
            CalculatePointBenefit(0, Stats.RegenSpeed.Point, 1)
        );

        Stats.RegenStrength.SetDefaultValue(
            CalculatePointBenefit(Difficulty * 1, Stats.RegenStrength.Point, 1)
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

    public JsonCharacter ToJson()
    {
        JsonCharacter jsonEntity = new JsonCharacter
        {
            Name = Name,
            Level = Experience.CurrentLevel(),
            StarRating = StarRating.Value,
            ID = ID,
            CurrentXp = Experience.CurrentXp(),
            CurrentWeapon = Weapon?.ToJson()
        };
        JsonArtifact[] artifacts = new JsonArtifact[5];
        for (int i = 0; i < 5; i++) artifacts[i] = Artifacts.Get()[i].ToJson();
        jsonEntity.Artifacts = artifacts;
        return jsonEntity;
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
