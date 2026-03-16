using GentrysQuest.Game.IO;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Entity;

public class Character : Entity
{
    public ArtifactManager Artifacts { get; }

    public Character()
    {
        Artifacts = new ArtifactManager(this);
        Artifacts.OnChangeArtifact += UpdateStats;
        UpdateStats();
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

        RemoveStatModifierSourcesByPrefix("equipment:");

        if (Weapon != null)
            SetStatModifierSource("equipment:weapon", createModifierFromBuff(Weapon.Buff));

        for (int i = 0; i < Artifacts.Get().Length; i++)
        {
            Artifact artifact = Artifacts.Get()[i];

            if (artifact == null)
                continue;

            SetStatModifierSource($"equipment:artifact:{i}:main", createModifierFromBuff(artifact.MainAttribute));

            for (int buffIndex = 0; buffIndex < artifact.Attributes.Count; buffIndex++)
                SetStatModifierSource($"equipment:artifact:{i}:sub:{buffIndex}", createModifierFromBuff(artifact.Attributes[buffIndex]));
        }

        RebuildStatAdditionalValues();

        if (Stats.CritRate.Total() > 100)
        {
            SetStatModifierSource("rule:crit-rate-overcap", StatModifier.Flat(StatType.CritDamage, 100 - Stats.CritRate.Total()));
            RebuildStatAdditionalValues();
        }
        else
        {
            RemoveStatModifierSource("rule:crit-rate-overcap");
        }

        base.UpdateStats();
    }

    private static StatModifier createModifierFromBuff(Buff buff)
    {
        if (buff.IsPercent)
            return StatModifier.PercentOfDefault(buff.StatType, buff.Value.Value);

        return StatModifier.Flat(buff.StatType, buff.Value.Value);
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

        for (int i = 0; i < 5; i++)
        {
            var slot = Artifacts.Get()[i];
            artifacts[i] = slot?.ToJson();
        }

        jsonEntity.Artifacts = artifacts;
        return jsonEntity;
    }

    public override void LoadJson(IJsonEntity jsonEntity)
    {
        JsonCharacter data = (JsonCharacter)jsonEntity;
        Logger.Log($"Loading character {data.Name}");
        Name = data.Name;
        ID = data.ID;
        StarRating.Value = data.StarRating;
        Experience.Level.Current.Value = data.Level;
        Experience.Xp.Current.Value = data.CurrentXp;

        if (data.CurrentWeapon != null)
            Weapon = (Weapon.Weapon)ItemSerializer.DeserializeItem("weapon", ItemSerializer.SerializeToString(data.CurrentWeapon));

        for (int i = 0; i < data.Artifacts.Length; i++)
        {
            if (data.Artifacts[i] != null)
                Artifacts.Equip((Artifact)ItemSerializer.DeserializeItem("artifact", ItemSerializer.SerializeToString(data.Artifacts[i])), i);
        }

        UpdateStats();
    }

    public Enemy CreateEnemy(WeaponChoices weaponChoices)
    {
        Enemy enemy = new Enemy
        {
            Name = Name
        };

        if (enemy.TextureMapping != null)
        {
            enemy.TextureMapping.Remove("Idle");
            enemy.TextureMapping.Add("Idle", TextureMapping.Get("Idle"));
        }

        enemy.Secondary = Secondary;
        enemy.Utility = Utility;
        enemy.Ultimate = Ultimate;
        enemy.WeaponChoices = weaponChoices;
        return enemy;
    }
}
