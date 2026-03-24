using System;
using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osuTK;

namespace GentrysQuest.Game.Content.Artifacts
{
    public class RandomArtifact : Artifact
    {
        public override string Name { get; set; } = "Random Artifact";
        public override string Description { get; protected set; }

        public override List<StatType> ValidMainAttributes { get; set; } =
        [
            StatType.Attack,
            StatType.Defense,
            StatType.Health,
            StatType.CritRate,
            StatType.CritDamage,
            StatType.RegenStrength
        ];

        public readonly List<RandomIconShape> IconShapes = new();

        private TriggerType triggerType;
        private EffectType effectType;
        private double effectValueA;
        private double lastKnownHealth;
        private bool handlingHealthEvent;

        public RandomArtifact()
        {
            TextureMapping = new TextureMapping();
            TextureMapping.Add("Icon", string.Empty);

            generateRandomVisual();
            rollRandomEffect();
        }

        public override void OnEquip(Entity.Entity entity)
        {
            lastKnownHealth = entity.Stats.Health.Current.Value;

            switch (triggerType)
            {
                case TriggerType.OnHitEntity:
                    entity.OnHitEntity += onHitEntity;
                    break;

                case TriggerType.OnGetHit:
                    entity.OnGetHit += onGetHit;
                    break;

                case TriggerType.OnHeal:
                    entity.OnHealthEvent += onHealthEvent;
                    break;
            }
        }

        public override void OnUnequip(Entity.Entity entity)
        {
            entity.OnHitEntity -= onHitEntity;
            entity.OnGetHit -= onGetHit;
            entity.OnHealthEvent -= onHealthEvent;
        }

        private void onHitEntity(DamageDetails details)
        {
            switch (effectType)
            {
                case EffectType.FlatBonusDamage:
                    details.Damage += (int)effectValueA;
                    break;

                case EffectType.PercentBonusDamage:
                    details.Damage += (int)Math.Round(details.Damage * effectValueA);
                    break;

                case EffectType.Lifesteal:
                    Holder?.Heal((int)Math.Max(1, Math.Round(details.Damage * effectValueA)));
                    break;
            }
        }

        private void onGetHit(DamageDetails details)
        {
            if (effectType != EffectType.FlatDamageReduction) return;

            details.Damage = Math.Max(0, details.Damage - (int)effectValueA);
        }

        private void onHealthEvent()
        {
            if (Holder == null || handlingHealthEvent || effectType != EffectType.AttackFromHeal) return;

            var currentHealth = Holder.Stats.Health.Current.Value;
            var healthDelta = currentHealth - lastKnownHealth;
            lastKnownHealth = currentHealth;

            if (healthDelta <= 0) return;

            handlingHealthEvent = true;

            try
            {
                Holder.Stats.Attack.Add(healthDelta * effectValueA);
            }
            finally
            {
                handlingHealthEvent = false;
            }
        }

        private void rollRandomEffect()
        {
            triggerType = (TriggerType)Random.Shared.Next(0, 3);

            switch (triggerType)
            {
                case TriggerType.OnHitEntity:
                {
                    effectType = (EffectType)Random.Shared.Next((int)EffectType.FlatBonusDamage, (int)EffectType.Lifesteal + 1);
                    effectValueA = effectType switch
                    {
                        EffectType.FlatBonusDamage => Random.Shared.Next(5, 31),
                        EffectType.PercentBonusDamage => Random.Shared.NextDouble() * 0.45 + 0.1,
                        EffectType.Lifesteal => Random.Shared.NextDouble() * 0.2 + 0.05,
                        _ => 0
                    };
                    break;
                }

                case TriggerType.OnGetHit:
                    effectType = EffectType.FlatDamageReduction;
                    effectValueA = Random.Shared.Next(4, 26);
                    break;

                case TriggerType.OnHeal:
                    effectType = EffectType.AttackFromHeal;
                    effectValueA = Random.Shared.NextDouble() * 0.25 + 0.05;
                    break;
            }

            Description = buildDescription();
        }

        private string buildDescription()
        {
            return effectType switch
            {
                EffectType.FlatBonusDamage => $"[condition]On hit[/condition], gain [unit]+{(int)effectValueA}[/unit] damage.",
                EffectType.PercentBonusDamage => $"[condition]On hit[/condition], gain [unit]+{Math.Round(effectValueA * 100)}%[/unit] damage.",
                EffectType.Lifesteal => $"[condition]On hit[/condition], heal [unit]{Math.Round(effectValueA * 100)}%[/unit] of dealt damage.",
                EffectType.FlatDamageReduction => $"[condition]On getting hit[/condition], reduce incoming damage by [unit]{(int)effectValueA}[/unit].",
                EffectType.AttackFromHeal => $"[condition]On heal[/condition], gain [unit]+{Math.Round(effectValueA * 100)}%[/unit] of healed amount as Attack.",
                _ => "A chaotic artifact."
            };
        }

        private void generateRandomVisual()
        {
            IconShapes.Clear();
            var count = Random.Shared.Next(4, 9);

            for (var i = 0; i < count; i++)
            {
                var width = (float)(Random.Shared.NextDouble() * 0.45 + 0.15);
                var height = (float)(Random.Shared.NextDouble() * 0.45 + 0.15);

                IconShapes.Add(new RandomIconShape
                {
                    RelativePosition = new Vector2(
                        (float)(Random.Shared.NextDouble() * 0.8 + 0.1),
                        (float)(Random.Shared.NextDouble() * 0.8 + 0.1)
                    ),
                    RelativeSize = new Vector2(width, height),
                    Colour = ColourInfo.SingleColour(new Colour4(
                        (byte)Random.Shared.Next(35, 255),
                        (byte)Random.Shared.Next(35, 255),
                        (byte)Random.Shared.Next(35, 255),
                        255
                    )),
                    Rotation = (float)(Random.Shared.NextDouble() * 360)
                });
            }
        }

        private enum TriggerType
        {
            OnHitEntity,
            OnGetHit,
            OnHeal
        }

        private enum EffectType
        {
            FlatBonusDamage,
            PercentBonusDamage,
            Lifesteal,
            FlatDamageReduction,
            AttackFromHeal
        }
    }

    public class RandomIconShape
    {
        public Vector2 RelativePosition { get; set; }
        public Vector2 RelativeSize { get; set; }
        public ColourInfo Colour { get; set; }
        public float Rotation { get; set; }
    }
}
