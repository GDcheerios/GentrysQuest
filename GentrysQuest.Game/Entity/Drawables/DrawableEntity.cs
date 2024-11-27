using System;
using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    /// <summary>
    /// The part of the entity that we see
    /// </summary>
    public partial class DrawableEntity : CompositeDrawable, IDrawableEntity
    {
        /// <summary>
        /// The entity reference
        /// </summary>
        protected readonly Entity Entity;

        /// <summary>
        /// The entity sprite
        /// </summary>
        public Sprite Sprite { get; set; }

        /// <summary>
        /// The overhead of the entity
        /// </summary>
        private readonly DrawableEntityBar entityBar;

        /// <summary>
        /// The visual weapon
        /// </summary>
        public DrawableWeapon Weapon;

        public AffiliationType Affiliation { get; set; }
        public List<Particle> Particles { get; set; } = new();
        public List<Projectile> QueuedProjectiles { get; set; } = new();

        public HitBox HitBox { get; set; }
        public CollisonHitBox ColliderBox;

        public int DirectionLooking;
        public Vector2 Direction = Vector2.Zero;
        public Vector2 FocusedPosition = Vector2.Zero;
        private Vector2 knockbackDirection;
        private float knockbackForce;
        private double knockbackDuration;
        private double knockbackTimeRemaining;

        // stat modifiers
        /// <summary>
        /// The base speed variable for all entities
        /// </summary>
        protected const double SPEED_MAIN = 0.15;

        private const int DODGE_TIME = 250;
        private const int DODGE_INTERVAL = 1000;
        private const int BASE_DODGE_SPEED = 3;

        /// <summary>
        /// The center of this DrawableEntity
        /// </summary>
        private static readonly Vector2 CENTER = new Vector2(50);

        /// <summary>
        /// When doing some math you might need this
        /// </summary>
        public const float SLOWING_FACTOR = 0.01f;

        private double lastRegenTime;
        private double lastHitTime;

        // Movement events
        public delegate void Movement(Vector2 direction, double speed);

        public event Movement OnMove;

        /// <summary>
        /// A drawable entity
        /// </summary>
        /// <param name="entity">The entity reference</param>
        /// <param name="affiliationType">Entity Affiliation</param>
        /// <param name="showInfo">Will overhead info be shown on screen?</param>
        public DrawableEntity(Entity entity, AffiliationType affiliationType = AffiliationType.None, bool showInfo = true)
        {
            entity.UpdateStats();
            entity.Stats.Restore();
            Entity = entity;
            Affiliation = affiliationType;
            Size = new Vector2(100);
            HitBox = new HitBox(this);
            ColliderBox = new CollisonHitBox(this);
            Colour = Colour4.White;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            AlwaysPresent = true;
            InternalChildren = new Drawable[]
            {
                Sprite = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                },
                entityBar = new DrawableEntityBar(Entity),
                HitBox,
                ColliderBox
            };

            if (!showInfo)
            {
                entityBar.HealthProgressBar.Hide();
                entityBar.HealthText.Hide();
                entityBar.EntityLevel.Hide();
                entityBar.StatusEffects.Anchor = Anchor.CentreLeft;
                entityBar.StatusEffects.Origin = Anchor.CentreLeft;
            }

            if (Entity.Weapon != null) Weapon = new DrawableWeapon(this, Affiliation);
            Entity.OnSwapWeapon += setDrawableWeapon;
            entity.OnDamage += delegate(int amount) { addIndicator(amount, DamageType.Damage); };
            entity.OnHeal += delegate(int amount) { addIndicator(amount, DamageType.Heal); };
            entity.OnCrit += delegate(int amount) { addIndicator(amount, DamageType.Crit); };
            entity.OnDamage += delegate { lastHitTime = Clock.CurrentTime; };
            entity.OnDeath += delegate { Sprite.FadeOut(100); };
            entity.OnSpawn += delegate { Sprite.FadeIn(100); };
            entity.OnSpawn += delegate { lastRegenTime = Clock.CurrentTime; };
            entity.OnEffect += delegate
            {
                foreach (var effect in Entity.Effects.Where(effect => !effect.IsInfinite))
                {
                    effect.StartTime ??= Clock.CurrentTime;
                }
            };
            entity.OnAddProjectile += parameters =>
            {
                Projectile projectile = new Projectile(parameters);
                projectile.Direction += DirectionLooking;
                QueuedProjectiles.Add(projectile);
            };
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, ISampleStore samples)
        {
            // textures
            Sprite.Colour = Colour4.White;
            Sprite.Texture = textures.Get(Entity.TextureMapping.Get("Idle"));

            // sounds
            Entity.OnSpawn += delegate { AudioManager.Instance.PlaySound(new DrawableSample(samples.Get(Entity.AudioMapping.Get("Spawn")))); };
            Entity.OnDamage += delegate { AudioManager.Instance.PlaySound(new DrawableSample(samples.Get(Entity.AudioMapping.Get("Damage")))); };
            Entity.OnLevelUp += delegate { AudioManager.Instance.PlaySound(new DrawableSample(samples.Get(Entity.AudioMapping.Get("Levelup")))); };
            Entity.OnDeath += delegate { AudioManager.Instance.PlaySound(new DrawableSample(samples.Get(Entity.AudioMapping.Get("Death")))); };
        }

        private void regen()
        {
            lastRegenTime = Clock.CurrentTime;
            Entity.Heal((int)Entity.Stats.RegenStrength.Current.Value);
        }

        public void ApplyKnockback(Vector2 direction, float force, int duration, KnockbackType type)
        {
            knockbackDirection = direction;
            knockbackForce = force;
            knockbackDuration = duration;
            knockbackTimeRemaining = duration;

            switch (type)
            {
                case KnockbackType.None:
                    break;

                case KnockbackType.StopsMovement:
                    Entity.AddEffect(new Stall(duration));
                    break;

                case KnockbackType.Stuns:
                    Entity.AddEffect(new Stun(duration + 300));
                    Weapon.RestWeapon();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void Move(Vector2 direction, double speed)
        {
            float value = (float)(Clock.ElapsedFrameTime * speed);
            ColliderBox.Position += (direction * 0.05f) * value;

            if (!HitBoxScene.Collides(ColliderBox)) OnMove?.Invoke(direction, speed);
        }

        /// <summary>
        /// Passes attack info down to children
        /// </summary>
        /// <param name="position">Location of the attack</param>
        public virtual void StartAttack(Vector2 position)
        {
            if (!Entity.CanAttack) return;

            double angle = MathBase.GetAngle(Position + CENTER, position);
            if (Weapon.GetBase().CanAttack) Weapon.StartAttack((float)angle + 90);
        }

        public virtual void EndAttack() => Weapon.EndAttack();

        /// <summary>
        /// Adds an indicator text for when this entity heals/takes damage
        /// </summary>
        /// <param name="amount">The amount of health change</param>
        /// <param name="type">The type of damage</param>
        private void addIndicator(int amount, DamageType type)
        {
            Colour4 colour = default;
            byte size = 50;

            switch (type)
            {
                case DamageType.Heal:
                    colour = Colour4.LimeGreen;
                    break;

                case DamageType.Damage:
                    colour = Colour4.White;
                    break;

                case DamageType.Crit:
                    colour = Colour4.Red;
                    size = 52;
                    break;
            }

            Indicator indicatorReference;
            AddInternal(indicatorReference = new Indicator(amount)
            {
                Colour = colour,
                Font = FontUsage.Default.With(size: size),
                Shadow = true
            });
            Scheduler.AddDelayed(() =>
            {
                RemoveInternal(indicatorReference, false);
            }, indicatorReference.FadeOut());
        }

        public void Dodge()
        {
            if (Entity.CanDodge)
            {
                Entity.CanDodge = false;
                Entity.IsDodging = true;
                Scheduler.AddDelayed(() => { Entity.IsDodging = false; }, DODGE_TIME);
                Scheduler.AddDelayed(() => { Entity.CanDodge = true; }, DODGE_INTERVAL);
                ApplyKnockback(Direction, (int)(BASE_DODGE_SPEED + GetSpeed()), DODGE_TIME, KnockbackType.StopsMovement);
            }
        }

        private void setDrawableWeapon()
        {
            if (Weapon != null)
            {
                RemoveInternal(Weapon, true);
                HitBoxScene.Remove(Weapon.HitBox);
            }

            if (Entity.Weapon != null)
            {
                Weapon = new DrawableWeapon(this, Affiliation);
                Weapon.Affiliation = Affiliation;
                AddInternal(Weapon);
            }
        }

        /// In some cases you'll want to get the entity reference for this drawable class
        /// <returns>The entity reference for this drawable</returns>
        public Entity GetBase() => Entity;

        /// <summary>
        /// Manages the speed of the entity
        /// </summary>
        /// <returns></returns>
        public double GetSpeed() => (SPEED_MAIN * Entity.Stats.Speed.Current.Value * Entity.SpeedModifier) + Entity.PositionJump;

        public void AddParticle(Particle particle) => Particles.Add(particle);

        protected override void Update()
        {
            // Main update logic
            base.Update();
            Entity.PositionRef = Position;

            // Movement logic
            Direction = Vector2.Zero;

            //  Knockback logic
            if (knockbackTimeRemaining > 0)
            {
                float knockbackDelta = (float)(knockbackTimeRemaining / knockbackDuration);
                Entity.SpeedModifier = knockbackForce * knockbackDelta;
                Direction += knockbackDirection * knockbackForce;

                knockbackTimeRemaining -= Clock.ElapsedFrameTime;

                if (knockbackTimeRemaining < 0)
                {
                    knockbackTimeRemaining = 0;
                    Entity.SpeedModifier = 1;
                }
            }

            // Reset collider box
            ColliderBox.Position = new Vector2(0);

            // Effects logic
            Entity.Affect(Clock.CurrentTime);

            // Skills logic
            Entity.Secondary?.SetPercent(Clock.CurrentTime);
            Entity.Utility?.SetPercent(Clock.CurrentTime);
            Entity.Ultimate?.SetPercent(Clock.CurrentTime);

            // Reset the teleport
            if (Entity.PositionJump > 0) Entity.PositionJump--;

            if (new ElapsedTime(Clock.CurrentTime, lastHitTime) > new Second(0.5))
            {
                Entity.AddTenacity();
                lastHitTime = Clock.CurrentTime;
            }

            // Regen should always be at the bottom
            // Skip if entity is dead or full health
            if (Entity.IsDead || Entity.IsFullHealth) return;

            // Regen timer
            double elapsedRegenTime = Clock.CurrentTime - lastRegenTime;

            // enemies should have no regen
            // therefore we skip if stat is 0
            if (Entity.Stats.RegenSpeed.Current.Value == 0) return;

            if (elapsedRegenTime * Entity.Stats.RegenSpeed.Current.Value >= 1000) regen();
        }
    }
}
