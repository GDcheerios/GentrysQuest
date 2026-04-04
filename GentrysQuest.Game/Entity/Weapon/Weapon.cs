using System.Collections.Generic;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Entity.Weapon
{
    public abstract class Weapon : Item
    {
        #region Metadata

        /// <summary>
        /// The weapon type
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// Describes how far away an enemy who is using the weapon needs to be to start attack an opponent
        /// </summary>
        public abstract int Distance { get; }

        /// <summary>
        /// If the weapon itself can deal damage or just projectiles
        /// </summary>
        public virtual bool IsGeneralDamageMode => true;

        /// <summary>
        /// A stat representing the damage.
        /// Makes more sense to have set in constructor
        /// </summary>
        public Stat Damage = new("Damage", StatType.Attack, 0); // Base damage

        #endregion

        #region States

        /// <summary>
        /// Counter for weapon attack
        /// </summary>
        public int AttackAmount { get; set; }

        /// <summary>
        /// This is the counter for attack patterns
        /// </summary>
        public int AttackCaseCounter { get; private set; }

        /// <summary>
        /// reference to direction looking
        /// </summary>
        public float Direction;

        /// <summary>
        /// If the weapon can attack.
        /// This is true when not attacking.
        /// </summary>
        public bool CanAttack;

        /// <summary>
        /// If clicking.
        /// </summary>
        public bool IsClicking;

        #endregion

        public DrawableWeapon DrawableInstance;

        /// <summary>
        /// How long the user has been holding for.
        /// </summary>
        protected double HoldDuration() => holdStartTime != 0 ? new ElapsedTime(GameClock.CurrentTime, holdStartTime) : 0;

        /// <summary>
        /// When the hold
        /// </summary>
        private double holdStartTime;

        /// <summary>
        /// Access the skill information for the weapon.
        /// This is how we display cooldown information on the HUD.
        /// </summary>
        public WeaponSkill SkillRef { get; protected set; }

        /// <summary>
        /// Custom resting event if needed
        /// </summary>
        public virtual AttackKeyframe RestingEvent { get; protected set; } = new AttackKeyframe
        {
            DoesDamage = false,
            Size = Vector2.Zero
        };

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

        public bool IsAttacking { get; private set; }

        public virtual float DropChance { get; set; } = 0.1f;

        public delegate void HitEvent(DamageDetails details);

        public event HitEvent OnHitEntity;

        protected Weapon()
        {
            Buff = ValidBuffs.Count > 0 ? new Buff(this, ValidBuffs[MathBase.RandomChoice(ValidBuffs.Count)]) : new Buff(this);
            SkillRef = new WeaponSkill();
            CalculateXpRequirement();
            UpdateStats();
        }

        public override void LevelUp()
        {
            base.LevelUp();
            UpdateStats();
            Holder?.UpdateStats();
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

        public override void LoadJson(IJsonEntity jsonEntity)
        {
            JsonWeapon jsonWeapon = (JsonWeapon)jsonEntity;
            LoadJsonBase(jsonWeapon);
            Buff = new Buff(jsonWeapon.Buff);
        }

        public void UpdateStats()
        {
            int level = Experience.CurrentLevel() - 1;
            int additionalDamage = level;
            additionalDamage += (int)(1.05 * Difficulty * level);
            additionalDamage += level * (StarRating.Value + 2);
            Damage.SetAdditional(additionalDamage);
        }

        /// <summary>
        /// On click logic.
        /// What will happen when clicked.
        /// </summary>
        public virtual void OnClick(float direction)
        {
            // logic
            holdStartTime = GameClock.CurrentTime;
            Direction = direction;
            IsClicking = true;
        }

        /// <summary>
        /// On release logic.
        /// What will happen when released.
        /// </summary>
        public virtual void OnRelease()
        {
            // logic
            holdStartTime = 0;
            IsClicking = false;
        }

        /// <summary>
        /// Ends the attack.
        /// Can be used to add custom logic.
        /// </summary>
        public virtual void EndAttack() => Holder.Attack(); // "OnAttack event"

        /// <summary>
        /// This is ran every frame while the animation is playing.
        /// </summary>
        public virtual void OnUpdate()
        {
            // implement frame logic
        }

        public void HitEntity(DamageDetails details)
        {
            OnHitEntity?.Invoke(details);
            Holder.HitEntity(details);
        }
    }
}
