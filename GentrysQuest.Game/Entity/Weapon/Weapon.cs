using System.Collections.Generic;
using GentrysQuest.Game.Input;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Utils;
using JetBrains.Annotations;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Entity.Weapon
{
    public abstract class Weapon : Item
    {
        /// <summary>
        /// The weapon type
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// Counter for weapon attack
        /// </summary>
        public int AttackAmount { get; set; }

        /// <summary>
        /// This is the counter for attack patterns
        /// </summary>
        public int AttackCaseCounter { get; set; }

        /// <summary>
        /// Describes how far away an enemy who is using the weapon needs to be to start attack an opponent
        /// </summary>
        public abstract int Distance { get; set; }

        /// <summary>
        /// A stat representing the damage.
        /// Makes more sense to have set in constructor
        /// </summary>
        public Stat Damage = new("Damage", StatType.Attack, 0); // Base damage

        public AttackPatternCaseHolder CurrentCase { get; protected set; }

        /// <summary>
        /// If the weapon can attack
        /// </summary>
        public bool CanAttack;

        /// <summary>
        /// If the weapon is attacking
        /// </summary>
        public bool IsAttacking;

        public bool IsCharging { get; private set; }

        /// <summary>
        /// Access the skill information for the weapon.
        /// This is how we display cooldown information on the HUD.
        /// </summary>
        public WeaponSkill SkillRef { get; protected set; }

        /// <summary>
        /// If the weapon itself can deal damage or just other things
        /// </summary>
        public virtual bool IsGeneralDamageMode { get; protected set; } = true;

        /// <summary>
        /// Custom resting event if needed
        /// </summary>
        public virtual AttackPatternEvent RestingEvent { get; protected set; } = new AttackPatternEvent
        {
            DoesDamage = false,
            Size = Vector2.Zero
        };

        /// <summary>
        /// If needing some animation while charging a weapon
        /// </summary>
        [CanBeNull]
        public AttackPatternEvent ChargingEvent { get; protected set; }

        /// <summary>
        /// Who is holding the weapon
        /// </summary>
        public Entity Holder;

        /// <summary>
        /// The weapon buff stat
        /// </summary>
        public Buff Buff;

        /// <summary>
        /// The current knockback strength
        /// </summary>
        public float KnockbackMultiplier;

        /// <summary>
        /// This determines what buffs this weapon could obtain when initialized
        /// </summary>
        public virtual List<StatType> ValidBuffs { get; set; } = new();

        /// <summary>
        /// Where the weapon should be held from
        /// design purpose
        /// </summary>
        public Anchor Origin = Anchor.Centre;

        public virtual float DropChance { get; set; } = 0.1f;

        public delegate void HitEvent(DamageDetails details);

        public event HitEvent OnHitEntity;

        protected Weapon()
        {
            Buff = ValidBuffs.Count > 0 ? new Buff(this, ValidBuffs[MathBase.RandomChoice(ValidBuffs.Count)]) : new Buff(this);
            SkillRef = new WeaponSkill();
            OnLevelUp += delegate
            {
                UpdateStats();
                Buff.Improve();
                Holder?.UpdateStats();
            };
        }

        public JsonWeapon ToJson()
        {
            JsonWeapon jsonEntity = new JsonWeapon
            {
                Name = Name,
                Level = Experience.CurrentLevel(),
                StarRating = StarRating.Value,
                ID = ID,
                CurrentXp = Experience.CurrentXp(),
                Buff = Buff.ToJson()
            };
            return jsonEntity;
        }

        public void LoadJson(JsonWeapon jsonEntity)
        {
            LoadJsonBase(jsonEntity);
            Buff = new Buff(jsonEntity.Buff);
        }

        public void UpdateStats() => Damage.SetAdditional((Experience.Level.Current.Value - 1) * (Difficulty + 1) * StarRating.Value);

        /// <summary>
        /// Code to be run when attacking
        /// </summary>
        public virtual void OnAttack(HoldEvent holdEvent)
        {
            AttackAmount++;
            AttackCaseCounter++;
            if (holdEvent.IsPressed && CanAttack) IsAttacking = true;
            CanAttack = false;
        }

        /// <summary>
        /// Ends the attack.
        /// Can be used to add custom logic.
        /// </summary>
        public virtual void EndAttack()
        {
            IsAttacking = false;
            CanAttack = false;
            Holder.Attack();
        }

        public void HitEntity(DamageDetails details)
        {
            OnHitEntity?.Invoke(details);
            Holder.HitEntity(details);
        }
    }
}
