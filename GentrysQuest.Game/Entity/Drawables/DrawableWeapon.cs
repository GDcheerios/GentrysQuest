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
        public List<OnHitEffect> OnHitEffects;
        private bool doesDamage;

        /// <summary>
        /// if the weapon is ready for resting pattern
        /// </summary>
        private bool readyForRest;

        /// <summary>
        /// The delay to make animations smooth
        /// </summary>
        private const int FADE_DELAY = 50;

        /// <summary>
        /// if an animation is currently playing
        /// </summary>
        public bool AnimationPlaying = false;

        public DrawableWeapon(DrawableEntity entity, AffiliationType affiliation)
        {
            User = entity;
            Weapon = entity.GetBase().Weapon;
            Weapon!.DrawableInstance = this;
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
        public void OnClick(float direction) => Weapon.OnClick(direction);

        public void OnRelease() => Weapon.OnRelease();

        public void AddParticle(Particle particle) => Particles.Add(particle);

        private float getDirection() => User.DirectionLooking + 90;

        public void PlayAnimation(AttackAnimation caseHolder)
        {
            if (AnimationPlaying) return;

            Weapon.AttackAmount++;
            DamageQueue.Clear();
            readyForRest = false;

            AnimationPlaying = true;
            float direction = getDirection();
            var list = caseHolder.GetEvents();
            double delay = 0;

            foreach (var t in list)
            {
                double speed = getPatternSpeed(t);
                var patternEvent = t;
                Scheduler.AddDelayed(() =>
                    {
                        playKeyframe(patternEvent, direction, speed);
                    }, delay
                );

                delay += speed;
            }

            GetBase().SkillRef.SetCooldown(delay);
            GetBase().SkillRef.Act();
            Scheduler.AddDelayed((() =>
            {
                readyForRest = true;
                AnimationPlaying = false;
            }), delay + FADE_DELAY);
        }

        private double getPatternSpeed(AttackKeyframe pattern)
        {
            double attackSpeed = Weapon?.Holder?.Stats.AttackSpeed.Current.Value ?? 1;

            if (attackSpeed <= 0 || double.IsNaN(attackSpeed) || double.IsInfinity(attackSpeed))
                attackSpeed = 1;

            double duration = pattern.TimeMs / attackSpeed;

            return duration < 0 || double.IsNaN(duration) || double.IsInfinity(duration) ? 0 : duration;
        }

        /// <summary>
        /// Rests the weapon.
        /// </summary>
        public void RestWeapon()
        {
            if (Weapon.RestingEvent != null)
            {
                playKeyframe(Weapon.RestingEvent, User.DirectionLooking + 90, getPatternSpeed(Weapon.RestingEvent), true);
            }
            else
            {
                AttackKeyframe restingPattern = new AttackKeyframe(100) { Size = Vector2.Zero };
                playKeyframe(restingPattern, direction: User.DirectionLooking, getPatternSpeed(restingPattern), true);
            }
        }

        public void StopAnimation()
        {
            AnimationPlaying = false;
            readyForRest = true;
        }

        /// <summary>
        /// Handles pattern transition.
        /// </summary>
        /// <param name="pattern">The pattern</param>
        /// <param name="direction">Direction to handle to</param>
        /// <param name="speed">speed modifier to the pattern</param>
        /// <param name="resting">if this is a resting pattern</param>
        private void playKeyframe(AttackKeyframe pattern, float direction, double speed, bool resting = false)
        {
            HitBox.Enable();
            if (pattern.Event != null) pattern.RunEvent();
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

            switch (resting)
            {
                case true:
                    HitBox.Disable();
                    this.RotateTo(pattern.Direction + direction, speed, pattern.Transition);
                    break;

                case false:
                {
                    this.RotateTo(pattern.Direction + direction, duration: speed, pattern.Transition);
                    if (pattern.ResetHitBox) DamageQueue.Clear();
                    Weapon.Damage.Add(Weapon.Damage.GetPercentFromTotal(pattern.DamagePercent));
                    Weapon.Holder.SpeedModifier = pattern.MovementSpeed;
                    OnHitEffects = pattern.OnHitEffects;
                    doesDamage = pattern.DoesDamage;
                    Weapon.KnockbackMultiplier = pattern.KnockbackMultiplier;
                    break;
                }
            }

            if (!doesDamage) HitBox.Disable();

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
            Weapon.OnUpdate();
            if (readyForRest) RestWeapon();

            Position = MathBase.RotateVector(PositionHolder, Rotation - 180) + MathBase.GetAngleToVector(Rotation - 90) * Distance;

            GetBase().SkillRef.Update();

            if (!AnimationPlaying) return;

            if (doesDamage && Weapon.IsGeneralDamageMode) _ = new DamageFrameHandler(HitBoxScene.GetIntersections(HitBox), DamageQueue, User.GetBase(), this);
        }
    }
}
