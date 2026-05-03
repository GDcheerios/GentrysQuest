using System;
using System.Collections.Generic;

namespace GentrysQuest.Game.Entity;

public class Enemy : Entity
{
    public WeaponChoices WeaponChoices = new();
    public ArtifactChoices ArtifactChoices = new();

    public Enemy()
        : base()
    {
        UpdateStats();
    }

    public override void UpdateStats()
    {
        int level = Experience.CurrentLevel();

        Stats.Health.SetDefaultValue(
            Math.Pow(Difficulty, 3) * (2000 * (Stats.Health.Point + 1)) +
            level * 100 * (Stats.Health.Point + 1)
        );

        int damage = (level + 1) * Stats.Attack.Point;
        damage += (int)(10 * Difficulty * level);
        Stats.Attack.SetDefaultValue(damage);

        Stats.Defense.SetDefaultValue(100);

        Stats.CritRate.SetDefaultValue(20);

        Stats.CritDamage.SetDefaultValue(Difficulty * 20);

        Stats.Speed.SetDefaultValue(0.75f);

        Stats.AttackSpeed.SetDefaultValue(1);

        RemoveStatModifierSourcesByPrefix("equipment:");

        if (Weapon != null)
        {
            if (Weapon.Buff.IsPercent)
                SetStatModifierSource("equipment:weapon", StatModifier.PercentOfDefault(Weapon.Buff.StatType, Weapon.Buff.Value.Value));
            else
                SetStatModifierSource("equipment:weapon", StatModifier.Flat(Weapon.Buff.StatType, Weapon.Buff.Value.Value));
        }

        RebuildStatAdditionalValues();

        base.UpdateStats();
    }

    public void SetRelativeLevel(int level)
    {
        int minimumLevel = Math.Max(1, level - 3);
        int maximumLevel = Math.Max(minimumLevel, level + 3);
        int relativeLevel = Random.Shared.Next(minimumLevel, maximumLevel + 1);

        Experience.Level.Current.Value = relativeLevel;
        Difficulty = (byte)(relativeLevel / 20);
        UpdateStats();
    }

    public void SetWeapon() => SetWeapon(WeaponChoices.GetChoice());
    public List<Artifact> GetArtifactReward() => ArtifactChoices.GetChoice();

    public Enemy Copy()
    {
        var copy = new Enemy
        {
            Difficulty = this.Difficulty,
            WeaponChoices = this.WeaponChoices,
            ArtifactChoices = this.ArtifactChoices,
            AiProfile = this.AiProfile
        };

        copy.Experience.Level.Current.Value = this.Experience.CurrentLevel();

        copy.Stats.Health.Point = this.Stats.Health.Point;
        copy.Stats.Attack.Point = this.Stats.Attack.Point;
        copy.Stats.Defense.Point = this.Stats.Defense.Point;
        copy.Stats.CritRate.Point = this.Stats.CritRate.Point;
        copy.Stats.CritDamage.Point = this.Stats.CritDamage.Point;
        copy.Stats.Speed.Point = this.Stats.Speed.Point;
        copy.Stats.AttackSpeed.Point = this.Stats.AttackSpeed.Point;

        if (this.Weapon != null) copy.SetWeapon(this.Weapon);

        copy.UpdateStats();

        return copy;
    }
}
