using System.Collections.Generic;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Graphics.TextStyles;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class Projectile : CompositeDrawable
    {
        /// <summary>
        /// Design of the projectile
        /// </summary>
        public CustomSprite Design;

        /// <summary>
        /// Speed of the projectile
        /// </summary>
        public double Speed;

        /// <summary>
        /// Direction of the projectile
        /// </summary>
        public double Direction;

        /// <summary>
        /// How long the projectile will last
        /// </summary>
        public double Lifetime;

        /// <summary>
        /// Hitbox of the projectile
        /// </summary>
        public HitBox HitBox;

        /// <summary>
        /// The amount of times the projectile can pass through enemies
        /// </summary>
        public int PassthroughAmount;

        /// <summary>
        /// projectile damage
        /// </summary>
        public int Damage;

        /// <summary>
        /// How it affects
        /// </summary>
        public List<OnHitEffect> OnHitEffects = null;

        /// <summary>
        /// The amount of hits that the projectile has hit
        /// </summary>
        public int Hits;

        /// <summary>
        /// track whether the projectile has been started or not.
        /// </summary>
        private bool started;

        /// <summary>
        /// If the damage takes defense into account
        /// </summary>
        public bool TakesDefense;

        /// <summary>
        /// If the damage takes weapon damage into account
        /// </summary>
        public bool TakesNormalDamage;

        /// <summary>
        /// If the damage takes holder damage into account
        /// </summary>
        public bool TakesHolderDamage;

        private DrawableEntity shooter;

        private readonly DamageQueue damageQueue = new DamageQueue();

        public AffiliationType Affiliation;

        private bool enabled = true;

        public Projectile(ProjectileParameters parameters)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InternalChild = new Box
            {
                Size = new Vector2(16),
                Colour = Colour4.Black
            };
            Speed = parameters.Speed;
            Direction = parameters.Direction;
            Lifetime = parameters.Lifetime;
            HitBox = parameters.HitBox;
            PassthroughAmount = parameters.PassthroughAmount;
            Damage = parameters.Damage;
            OnHitEffects = parameters.OnHitEffects;
            TakesDefense = parameters.TakesDefense;
            TakesNormalDamage = parameters.TakesNormalDamage;
            TakesHolderDamage = parameters.TakesHolderDamage;
        }

        /// <summary>
        /// This sets up the projectile for shooting.
        /// Sets the affiliation and which then sets up the hitbox.
        /// </summary>
        /// <param name="shooter">the shooter</param>
        /// <param name="time">the current time</param>
        public void ShootFrom(DrawableEntity shooter)
        {
            this.shooter = shooter;
            Hits = 0;
            Position = shooter.Position;
            Affiliation = shooter.Affiliation;
            AddInternal(HitBox = new HitBox(this));
            started = true;
            if (TakesHolderDamage) Damage += (int)shooter.GetBase().Stats.Attack.Current.Value;
        }

        public void Disable()
        {
            enabled = false;
            Hide();
        }

        protected override void Update()
        {
            base.Update();
            if (!started) return;

            Position += (MathBase.GetAngleToVector(Direction) * 0.05f) * (float)(Speed * Clock.ElapsedFrameTime);
            if (enabled) _ = new DamageFrameHandler(HitBoxScene.GetIntersections(HitBox), damageQueue, shooter.GetBase(), this);
            if (Hits < PassthroughAmount) return;

            Hide();
        }
    }
}
