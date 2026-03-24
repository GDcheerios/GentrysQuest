using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Utils;
using JetBrains.Annotations;
using osu.Framework.Graphics.Colour;
using osuTK;

namespace GentrysQuest.Game.Entity
{
    public class Entity : EntityBase
    {
        // Info
        public bool IsDead;
        public bool IsFullHealth;
        public bool IsDodging = false;
        public bool CanDodge = true;
        public bool CanAttack = true;
        public bool CanMove = true;
        public bool Invincible = false;
        public bool CanDie = true;
        public bool CanKnockback = true;
        public int CurrentTenacity;
        public Vector2 PositionRef;
        public double LastDamageTime;
        public double LastTenacityTime;

        // Stats
        public Stats Stats = new();
        public StatModifierCollection StatModifiers { get; } = new();
        public Dictionary<Entity, int> HitCounter = new();

        // Equips
        [CanBeNull]
        public Weapon.Weapon Weapon;

        // Effects
        public List<StatusEffect> Effects = new();

        // Stat Modifiers
        public float SpeedModifier = 1;
        public float HealingModifier = 1;
        public float DamageModifier = 1;
        public float DefenseModifier = 1;
        public float PositionJump = 0; // For teleporting
        public float KnockbackModifier = 1;
        public float EffectModifier = 1;

        // Skills
        public Skill Secondary = null;
        public Skill Utility = null;
        public Skill Ultimate = null;

        public Entity()
        {
            CurrentTenacity = (int)Stats.Tenacity.Total();
            Stats.Health.Current.ValueChanged += delegate { OnHealthEvent?.Invoke(); };
            CalculateXpRequirement();
        }

        #region Events

        public delegate void EntitySpawnEvent();

        public delegate void EntityHealthEvent(int amount);

        public delegate void EntityHealthDisplayEvent(string text, ColourInfo colour = default);

        public delegate void EntityDamageEvent(DamageDetails details);

        public delegate void ProjectileAdditionEvent(ProjectileParameters parameters);

        public delegate void SwapArtifactEvent(Artifact artifact);

        public delegate void SwapWeaponEvent(Weapon.Weapon weapon);

        public delegate void EffectEvent(StatusEffect effect);

        // Spawn / Death events
        public event EntitySpawnEvent OnSpawn;
        public event EntitySpawnEvent OnDeath;

        // Health events
        public event EntityEvent OnHealthEvent;
        public event EntityHealthDisplayEvent OnHealthDisplay;

        // Equipment events
        public event SwapWeaponEvent OnSwapWeapon;
        public event SwapArtifactEvent OnSwapArtifact;

        // Combat events
        public event EntityEvent OnAttack;
        public event EntityDamageEvent OnHitEntity;
        public event EntityDamageEvent OnGetHit;
        public event EntityDamageEvent OnDamage;

        // Other Events
        public event EntityEvent OnUpdateStats;
        public event EffectEvent OnEffect;
        public event ProjectileAdditionEvent OnAddProjectile;

        #endregion

        #region Methods

        public void Spawn()
        {
            CanMove = true;
            CanAttack = true;
            IsDead = false;
            OnSpawn?.Invoke();
        }

        public virtual void Die()
        {
            if (!CanDie) return;

            CanMove = false;
            CanAttack = false;
            IsDead = true;
            OnDeath?.Invoke();
        }

        public override void LevelUp()
        {
            base.LevelUp();
            UpdateStats();
            Difficulty = (byte)(Experience.Level.Current.Value / 20);
        }

        public void Attack() => OnAttack?.Invoke();

        public int AfterDefense(int amount) => (int)(amount * (100 / (Stats.Defense.Current.Value * DefenseModifier)));

        public virtual void Damage(DamageDetails details)
        {
            IsFullHealth = false;
            int amount = (int)(details.Damage * DamageModifier);
            Stats.Health.UpdateCurrentValue(-amount);
            if (Stats.Health.Current.Value <= 0 && !IsDead) Die();
            OnDamage?.Invoke(details);
            LastDamageTime = GameClock.CurrentTime;
        }

        public void HitEntity(DamageDetails details) => OnHitEntity?.Invoke(details);
        public void OnHit(DamageDetails details) => OnGetHit?.Invoke(details);

        public virtual void Heal(int amount)
        {
            Stats.Health.UpdateCurrentValue(amount * HealingModifier);
            IsFullHealth = Stats.Health.Current.Value == Stats.Health.Total();
            OnHealthEvent?.Invoke();
        }

        public void Heal()
        {
            int amount = (int)Stats.Health.Total();
            Stats.Health.UpdateCurrentValue(amount);
            OnHealthEvent?.Invoke();
        }

        public void DisplayHealthEvent(string text, ColourInfo colour = default) => OnHealthDisplay?.Invoke(text, colour);

        public void SetWeapon([CanBeNull] Weapon.Weapon weapon)
        {
            Weapon = weapon;
            if (weapon != null) weapon.Holder = this;
            UpdateStats();
            OnSwapWeapon?.Invoke(weapon);
        }

        public int GetXpReward()
        {
            int value = 0;

            value += Experience.Level.Current.Value * 5;
            value += Stats.GetPointTotal() * 2;
            if (Weapon != null) value += (int)(Weapon.Damage.Current.Value / 4);

            return value;
        }

        public int GetMoneyReward()
        {
            int value = 0;

            value += Experience.Level.Current.Value;
            value += Stats.GetPointTotal();

            return value;
        }

        public virtual Weapon.Weapon GetWeaponReward()
        {
            if (Weapon != null && MathBase.IsChanceSuccessful(Weapon!.DropChance)) return Weapon;

            return null;
        }

        public void AddTenacity()
        {
            if (CurrentTenacity < Stats.Tenacity.GetCurrent()) CurrentTenacity++;
        }

        public void RemoveTenacity()
        {
            if (CurrentTenacity > 0) CurrentTenacity--;
        }

        public bool HasTenacity() => CurrentTenacity > 0;

        public void AddEffect(StatusEffect statusEffect, Entity effectedBy = null)
        {
            bool inList = false;
            statusEffect.SetEffector(this);
            statusEffect.EffectedBy = effectedBy;

            foreach (var effect in Effects.Where(effect => effect.GetType() == statusEffect.GetType()))
            {
                effect.Stack++;
                effect.RestartLifetime();
                inList = true;
            }

            if (!inList)
            {
                statusEffect.RestartLifetime();
                Effects.Add(statusEffect);
            }

            OnEffect?.Invoke(statusEffect);
        }

        public void RemoveEffect(StatusEffect statusEffect)
        {
            foreach (var effect in Effects.Where(effect => effect.GetType() == statusEffect.GetType()).ToList())
            {
                effect.Stack--;

                if (effect.Stack == 0)
                {
                    effect.OnRemove?.Invoke();
                    Effects.Remove(effect);
                    OnEffect?.Invoke(effect);
                }
                else OnEffect?.Invoke(effect);
            }
        }

        public void RemoveEffect(string name)
        {
            for (var index = 0; index < Effects.Count; index++)
            {
                StatusEffect effect = Effects[index];

                if (effect.Name.Equals(name))
                {
                    effect.OnRemove?.Invoke();
                    Effects.Remove(effect);
                    OnEffect?.Invoke(effect);
                }
            }
        }

        public void ClearEffects()
        {
            foreach (var effect in Effects.ToList())
            {
                effect.OnRemove?.Invoke();
                Effects.Remove(effect);
                OnEffect?.Invoke(effect);
            }
        }

        public void Affect(double time)
        {
            foreach (StatusEffect effect in Effects.ToList())
            {
                effect.SetTime(time);
                effect.Handle();

                if (effect.IsInfinite) continue;

                effect.StartTime ??= time;

                if (time - effect.StartTime >= effect.Duration)
                {
                    if (effect.Stack == 1) RemoveEffect(effect.Name);
                    else
                    {
                        effect.Stack--;
                        effect.RestartLifetime(time);
                        OnEffect?.Invoke(effect);
                    }
                }
            }
        }

        public void AddProjectile(ProjectileParameters parameters) => OnAddProjectile?.Invoke(parameters);

        /// <summary>
        /// Defines how stats will update
        /// </summary>
        public virtual void UpdateStats() => OnUpdateStats?.Invoke();

        #endregion

        protected void SetStatModifierSource(string sourceKey, params StatModifier[] modifiers)
        {
            StatModifiers.SetSource(sourceKey, modifiers);
        }

        protected void SetStatModifierSource(string sourceKey, IEnumerable<StatModifier> modifiers)
        {
            StatModifiers.SetSource(sourceKey, modifiers);
        }

        protected void RemoveStatModifierSource(string sourceKey)
        {
            StatModifiers.RemoveSource(sourceKey);
        }

        protected void RemoveStatModifierSourcesByPrefix(string prefix)
        {
            StatModifiers.RemoveSourcesByPrefix(prefix);
        }

        public void RefreshStatModifiers()
        {
            RebuildStatAdditionalValues();
            OnUpdateStats?.Invoke();
        }

        protected void RebuildStatAdditionalValues()
        {
            Stats.ResetAdditionalValues();

            foreach (Stat stat in Stats.GetStats())
            {
                IReadOnlyList<StatModifier> modifiers = StatModifiers.ForStat(stat.Type);

                if (modifiers.Count == 0)
                    continue;

                double flat = 0;
                double percentOfDefault = 0;

                foreach (StatModifier modifier in modifiers)
                {
                    switch (modifier.Operation)
                    {
                        case StatModifierOperation.Flat:
                            flat += modifier.Value;
                            break;

                        case StatModifierOperation.PercentOfDefault:
                            percentOfDefault += modifier.Value;
                            break;
                    }
                }

                double additional = flat + stat.GetPercentFromDefault((float)percentOfDefault);
                stat.SetAdditional(additional);
            }
        }

        protected void AddToStat(Buff attribute)
        {
            Stat stat = Stats.GetStat(attribute.StatType.ToString());
            stat.Add(attribute.IsPercent ? stat.GetPercentFromDefault((float)attribute.Value.Value) : attribute.Value.Value);
        }

        protected static double CalculatePointBenefit(double normalValue, int point, double pointBenefit)
        {
            return normalValue + (point * pointBenefit);
        }
    }
}
