using System.Linq;
using GentrysQuest.Game.Entity.Weapon;
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

        /// <summary>
        /// Used to obtain the base weapon object
        /// </summary>
        /// <returns>The base weapon object</returns>
        public Weapon.Weapon GetBase() => Weapon;

        public float Distance;
        public Vector2 PositionHolder;
        private OnHitEffect onHitEffect;
        private bool doesDamage;

        /// <summary>
        /// Where the weapon should rest after an attack.
        /// </summary>
        private AttackPatternEvent restingPattern;

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
                Weapon.CanAttack = true;
            }
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Sprite.Colour = Colour4.White;
            Sprite.Texture = textures.Get(Weapon.TextureMapping.Get("Base"));
            GetBase().SkillRef.TextureMapping.Add("Icon", GetBase().TextureMapping.Get("Icon"));
        }

        /// <summary>
        /// Drawable weapon logic for attacking.
        /// </summary>
        /// <param name="direction">Attack direction</param>
        public void Attack(float direction)
        {
            DamageQueue.Clear();
            Weapon.CanAttack = false;
            HitBox.Enable();
            Weapon.AttackAmount += 1;
            AttackPatternCaseHolder caseHolder = Weapon.AttackPattern.GetCase(Weapon.AttackAmount); // set the caseHolder
            Weapon.Holder.Attack(); // Call the holder base method to handle events.

            if (caseHolder == null)
            {
                Weapon.AttackAmount = 1;
                caseHolder = Weapon.AttackPattern.GetCase(Weapon.AttackAmount);
            }

            var patterns = caseHolder.GetEvents();
            double delay = 0;

            foreach (AttackPatternEvent pattern in patterns)
            {
                double speed = getPatternSpeed(pattern);
                Scheduler.AddDelayed(() =>
                {
                    handlePattern(pattern, direction, speed);
                    restingPattern = pattern;
                }, delay);
                delay += speed;
            }

            // Skill logic
            GetBase().SkillRef.LastUseTime = Clock.CurrentTime;
            GetBase().SkillRef.SetCooldown(delay);
            GetBase().SkillRef.Act();

            LastUseTime = Clock.CurrentTime + delay;
            Scheduler.AddDelayed(() => RestWeapon(true), delay);
        }

        private double getPatternSpeed(AttackPatternEvent pattern) => pattern.TimeMs / Weapon.Holder.Stats.AttackSpeed.Current.Value;

        /// <summary>
        /// Rests the weapon.
        /// </summary>
        /// <param name="delay">if there's an added delay to the weapon rest</param>
        public void RestWeapon(bool delay = false)
        {
            // re-enable attacking and hitbox
            Weapon.CanAttack = true;
            HitBox.Disable();

            if (restingPattern != null)
            {
                handlePattern(restingPattern, User.DirectionLooking + 90, delay ? getPatternSpeed(restingPattern) : 0, true);

                if (delay)
                {
                    transitionCooldown = true;
                    Scheduler.AddDelayed(() =>
                    {
                        transitionCooldown = false;
                    }, getPatternSpeed(restingPattern));
                }
            }
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

            if (pattern.ForcedMovement)
            {
                User.ApplyKnockback(
                    MathBase.GetAngleToVector(pattern.ForcedMovementDirection + User.DirectionLooking),
                    pattern.ForcedMovementStrength,
                    (int)speed,
                    KnockbackType.StopsMovement
                );
            }

            if (!resting) // non-resting logic
            {
                if (pattern.ResetHitBox) DamageQueue.Clear();
                Weapon.Damage.Add(Weapon.Damage.GetPercentFromTotal(pattern.DamagePercent));
                Weapon.Holder.SpeedModifier = pattern.MovementSpeed;
                onHitEffect = pattern.OnHitEffect;
                doesDamage = pattern.DoesDamage;
                Weapon.KnockbackMultiplier = pattern.KnockbackMultiplier;
            }

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
            Position = MathBase.RotateVector(PositionHolder, Rotation - 180) + MathBase.GetAngleToVector(Rotation - 90) * Distance;
            GetBase().SkillRef.SetPercent(Clock.CurrentTime);

            if (new ElapsedTime(Clock.CurrentTime, LastUseTime) > COMBO_RESET_INTERVAL)
            {
                Weapon.AttackAmount = 0;
                restingPattern = Weapon.AttackPattern.GetFirstCaseEvent();
                transitionCooldown = true;
                handlePattern(restingPattern, User.DirectionLooking + 90, 100, true);
            }

            if (!Weapon.CanAttack)
            {
                if (doesDamage && Weapon.IsGeneralDamageMode) _ = new DamageFrameHandler(HitBoxScene.GetIntersections(HitBox), DamageQueue, User.GetBase());
            }
            else
            {
                Weapon.UpdateStats();
                Weapon.Holder.SpeedModifier = 1;
                if (!transitionCooldown) RestWeapon();
            }
        }
    }
}
