using System;
using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Utils;
using JetBrains.Annotations;
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
        public int CurrentTenacity = 0;
        public Vector2 PositionRef;

        // Stats
        public Stats Stats = new();
        public Dictionary<Entity, int> EnemyHitCounter = new();

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

        // Skills
        public Skill Secondary = null;
        public Skill Utility = null;
        public Skill Ultimate = null;

        public Entity()
        {
            OnLevelUp += UpdateStats;
            OnLevelUp += Stats.Restore;
            OnSwapWeapon += UpdateStats;
            CurrentTenacity = (int)Stats.Tenacity.Total();
            CalculateXpRequirement();
        }

        #region Events

        public delegate void EntitySpawnEvent();

        public delegate void EntityHealthEvent(int amount);

        public delegate void EntityHitEvent(DamageDetails details);

        public delegate void ProjectileAdditionEvent(ProjectileParameters parameters);

        // Spawn / Death events
        public event EntitySpawnEvent OnSpawn;
        public event EntitySpawnEvent OnDeath;

        // Health events
        public event EntityEvent OnHealthEvent;
        public event EntityHealthEvent OnDamage;
        public event EntityHealthEvent OnHeal;
        public event EntityHealthEvent OnCrit;

        // Equipment events
        public event EntityEvent OnSwapWeapon;
        public event EntityEvent OnSwapArtifact;

        // Combat events
        public event EntityEvent OnAttack;
        public event EntityHitEvent OnHitEntity;
        public event EntityHitEvent OnGetHit;

        // Other Events
        public event EntityEvent OnUpdateStats;
        public event Action OnEffect;
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
            if (CanDie) return;

            CanMove = false;
            CanAttack = false;
            IsDead = true;
            OnDeath?.Invoke();
        }

        public void Attack() => OnAttack?.Invoke();

        public int AfterDefense(int amount) => (int)(amount * (100 / (Stats.Defense.Current.Value * DefenseModifier)));

        public virtual void Damage(int amount = 1)
        {
            if (Invincible) return;

            if (IsDodging) return;

            IsFullHealth = false;
            Stats.Health.UpdateCurrentValue(-amount * DamageModifier);
            if (Stats.Health.Current.Value <= 0 && !IsDead) Die();
            OnHealthEvent?.Invoke();
            OnDamage?.Invoke(amount);
        }

        public void DamageWithDefense(int amount) => Damage(AfterDefense(amount));

        public virtual void Crit(int amount = 1)
        {
            Damage(amount);
            OnCrit?.Invoke(amount);
        }

        public void CritWithDefense(int amount) => Crit(AfterDefense(amount));

        public void HitEntity(DamageDetails details) => OnHitEntity?.Invoke(details);
        public void OnHit(DamageDetails details) => OnGetHit?.Invoke(details);

        public virtual void Heal(int amount)
        {
            Stats.Health.UpdateCurrentValue(amount * HealingModifier);
            IsFullHealth = Stats.Health.Current.Value == Stats.Health.Total();
            OnHealthEvent?.Invoke();
            OnHeal?.Invoke((int)(amount * HealingModifier));
        }

        public void SetWeapon([CanBeNull] Weapon.Weapon weapon)
        {
            Weapon = weapon;
            if (weapon != null) weapon.Holder = this;
            OnSwapWeapon?.Invoke();
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
            if (MathBase.IsChanceSuccessful(Weapon!.DropChance)) return Weapon;

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

        public void AddEffect(StatusEffect statusEffect)
        {
            bool inList = false;
            statusEffect.SetEffector(this);

            foreach (var effect in Effects.Where(effect => effect.GetType() == statusEffect.GetType()))
            {
                effect.Stack++;
                inList = true;
            }

            if (!inList) Effects.Add(statusEffect);
            OnEffect?.Invoke();
        }

        public void RemoveEffect(string name)
        {
            for (var index = 0; index < Effects.Count; index++)
            {
                var effect = Effects[index];

                if (effect.Name.Equals(name))
                {
                    effect.OnRemove?.Invoke();
                    Effects.Remove(effect);
                }
            }

            OnEffect?.Invoke();
        }

        public void Affect(double time)
        {
            foreach (StatusEffect effect in Effects.ToList())
            {
                effect.SetTime(time);
                effect.Handle();

                if (time - effect.StartTime > effect.Duration)
                {
                    if (effect.Stack == 1) RemoveEffect(effect.Name);
                    else effect.Stack--;
                }
            }

            OnEffect?.Invoke();
        }

        public void AddProjectile(ProjectileParameters parameters) => OnAddProjectile?.Invoke(parameters);

        /// <summary>
        /// Defines how stats will update
        /// </summary>
        public virtual void UpdateStats() => OnUpdateStats?.Invoke();

        #endregion

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
