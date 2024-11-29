using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class DrawableWeapon : CompositeDrawable, IDrawableEntity
    {
        protected readonly Weapon.Weapon Weapon;
        protected readonly DamageQueue DamageQueue = new();
        public DrawableEntity User { get; set; }
        public Sprite Sprite { get; set; }
        public HitBox HitBox { get; set; }
        public AffiliationType Affiliation { get; set; }
        public List<Particle> Particles { get; set; }

        /// <summary>
        /// Used to obtain the base weapon object
        /// </summary>
        /// <returns>The base weapon object</returns>
        public Weapon.Weapon GetBase() => Weapon;

        public float Distance;
        public Vector2 PositionHolder;
        public List<OnHitEffect> OnHitEffects = new();
        private bool doesDamage;

        private bool didChargeAttack;

        /// <summary>
        /// Last time of usage so we can reset back to first attack pattern.
        /// </summary>
        public double LastUseTime { get; set; }

        /// <summary>
        /// How long it should take until we reset the attack pattern.
        /// </summary>
        private const int COMBO_RESET_INTERVAL = 1000;

        /// <summary>
        /// This is to ensure that when resting the weapon
        /// right after an attack it won't look clunky.
        /// </summary>
        private bool transitionCooldown;

        /// <summary>
        /// if the weapon is ready for resting pattern
        /// </summary>
        private bool readyForRest;

        /// <summary>
        /// The delay to make animations smooth
        /// </summary>
        private const int FADE_DELAY = 50;

        public DrawableWeapon(DrawableEntity entity, AffiliationType affiliation)
        {
            User = entity;
            Weapon = entity.GetBase().Weapon;
            Affiliation = affiliation;
            HitBox = new HitBox(this);
            Size = new Vector2(1f);
            RelativeSizeAxes = Axes.Both;
            Colour = Colour4.White;
            Anchor = Anchor.Centre;
            AlwaysPresent = true;

            if (Weapon != null)
            {
                Origin = Weapon.Origin;
                InternalChildren = new Drawable[]
                {
                    Sprite = new Sprite
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    HitBox
                };
                readyForRest = true;
            }
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Sprite.Colour = Colour4.White;
            Sprite.Texture = textures.Get(Weapon.TextureMapping.Get("Base"));
            GetBase().SkillRef.TextureMapping.Add("Icon", GetBase().TextureMapping.Get("Icon"));
        }

        private bool weaponExists() => Weapon != null;

        /// <summary>
        /// Drawable weapon logic for attacking.
        /// </summary>
        /// <param name="direction">Attack direction</param>
        public void StartAttack(float direction)
        {
            DamageQueue.Clear();
            Weapon.StartAttack(direction);
            LastUseTime = GameClock.CurrentTime;
            readyForRest = false;
        }

        public void EndAttack() => Weapon.EndAttack();

        public void AddParticle(Particle particle) => Particles.Add(particle);

        private void handlePatternCase(AttackPatternCaseHolder caseHolder, float direction)
        {
            var list = caseHolder.GetEvents();
            double delay = 0;

            foreach (var t in list)
            {
                double speed = getPatternSpeed(t);
                var patternEvent = t;
                Scheduler.AddDelayed(() =>
                    {
                        handlePattern(patternEvent, direction, speed);
                    }, delay
                );

                delay += speed + FADE_DELAY;
            }

            Scheduler.AddDelayed(() => { readyForRest = true; }, delay + FADE_DELAY);
        }

        private double getPatternSpeed(AttackPatternEvent pattern) => pattern.TimeMs / Weapon.Holder.Stats.AttackSpeed.Current.Value;

        /// <summary>
        /// Rests the weapon.
        /// </summary>
        /// <param name="delay">if there's an added delay to the weapon rest</param>
        public void RestWeapon(bool delay = false)
        {
            if (Weapon.RestingEvent != null)
            {
                handlePattern(Weapon.RestingEvent, User.DirectionLooking + 90, delay ? getPatternSpeed(Weapon.RestingEvent) : 0, true);
            }
            else
            {
                AttackPatternEvent restingPattern = new AttackPatternEvent(100) { Size = Vector2.Zero };
                handlePattern(restingPattern, direction: User.DirectionLooking, delay ? getPatternSpeed(restingPattern) : 0, true);
            }

            if (!delay) return;

            transitionCooldown = true;
            Scheduler.AddDelayed(() =>
            {
                transitionCooldown = false;
            }, getPatternSpeed(Weapon.RestingEvent));
        }

        /// <summary>
        /// Handles pattern transition.
        /// </summary>
        /// <param name="pattern">The pattern</param>
        /// <param name="direction">Direction to handle to</param>
        /// <param name="speed">speed modifier to the pattern</param>
        /// <param name="resting">if this is a resting pattern</param>
        private void handlePattern(AttackPatternEvent pattern, float direction, double speed, bool resting = false)
        {
            this.RotateTo(pattern.Direction + direction, duration: speed, pattern.Transition);
            this.TransformTo(nameof(PositionHolder), pattern.Position, speed, pattern.Transition);
            this.ResizeTo(pattern.Size, duration: speed, pattern.Transition);
            HitBox.ScaleTo(pattern.HitboxSize, duration: speed, pattern.Transition);
            this.TransformTo(nameof(Distance), pattern.Distance, speed, pattern.Transition);
            if (pattern.InteruptAttack) Scheduler.AddDelayed(Weapon.EndAttack, speed);

            if (pattern.ForcedMovement)
            {
                User.ApplyKnockback(
                    MathBase.GetAngleToVector(pattern.ForcedMovementDirection + User.DirectionLooking),
                    pattern.ForcedMovementStrength,
                    (int)speed,
                    KnockbackType.StopsMovement
                );
            }

            switch (resting)
            {
                case true:
                    Weapon.CanAttack = true;
                    HitBox.Disable();
                    break;

                case false:
                {
                    if (pattern.ResetHitBox) DamageQueue.Clear();
                    Weapon.Damage.Add(Weapon.Damage.GetPercentFromTotal(pattern.DamagePercent));
                    Weapon.Holder.SpeedModifier = pattern.MovementSpeed;
                    OnHitEffects = pattern.OnHitEffects;
                    doesDamage = pattern.DoesDamage;
                    Weapon.KnockbackMultiplier = pattern.KnockbackMultiplier;
                    break;
                }
            }

            if (doesDamage) HitBox.Disable();

            if (!HitBox.Enabled) return; // don't need to continue if there's no hitbox

            if (pattern.Projectiles == null) return; // don't need to continue if there's no projectiles

            foreach (var projectile in pattern.Projectiles.Select(parameters => new Projectile(parameters)))
            {
                projectile.Position *= Distance;
                projectile.Direction += direction - 90;
                User.QueuedProjectiles.Add(projectile);
            }
        }

        protected override void Update()
        {
            base.Update();
            if (!Weapon.CanAttack) Weapon.OnUpdate();
            if (readyForRest) RestWeapon();

            Position = MathBase.RotateVector(PositionHolder, Rotation - 180) + MathBase.GetAngleToVector(Rotation - 90) * Distance;
            GetBase().SkillRef.SetPercent(Clock.CurrentTime);

            if (Weapon.CanAttack) return;

            if (doesDamage && Weapon.IsGeneralDamageMode) _ = new DamageFrameHandler(HitBoxScene.GetIntersections(HitBox), DamageQueue, User.GetBase(), this);
        }
    }
}
