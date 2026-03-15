using System.Collections.Generic;
using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Weapons
{
    public class BraydensOsuPen : Weapon
    {
        public override string Type { get; } = "Pen";
        public override int Distance { get; } = 200;
        public override string Name { get; set; } = "Brayden's Osu Pen";

        public override string Description { get; protected set; } = "The man himself, Brayden's osu pen!\n"
                                                                     + "On a spinning attack you have a [unit]20%[/unit] chance to [type]bleed[/type] enemies for [unit]6 seconds[/unit]. ";

        public override StarRating StarRating { get; protected set; } = new(5);

        public override List<StatType> ValidBuffs { get; set; } = [StatType.CritDamage];

        private readonly AttackAnimationRegistry attackRegistry = new AttackAnimationRegistry();

        private bool didSpin;

        /// <summary>
        /// How long until this weapon considers the attack as spin attack
        /// </summary>
        private const double spin_time = 250;

        public override AttackKeyframe RestingEvent { get; protected set; } = new AttackKeyframe(50)
        {
            Direction = -105
        };

        private string nextAnimation = "first";
        private AttackKeyframe lastFromFirst;
        private AttackKeyframe lastFromSecond;
        private AttackKeyframe lastFromSpin;

        public BraydensOsuPen()
        {
            Damage.SetDefaultValue(46);

            #region Design

            Origin = Anchor.BottomCentre;

            #endregion

            #region AttackPattern

            const int distance = 35;
            var time = (int)MathBase.SecondToMs(0.3); // seconds
            Vector2 hbSize = new Vector2(0.1f, 1);
            OnHitEffect lastComboEffect = new OnHitEffect(20) { Effect = new Bleed(new Second(6)) };

            attackRegistry.RegisterAnimation("first");
            attackRegistry.AddKeyframe(new AttackKeyframe
            {
                Direction = -90,
                Distance = distance,
                HitboxSize = hbSize,
            });

            lastFromFirst = new AttackKeyframe(time)
            {
                Direction = 90,
                Distance = distance,
                Transition = Easing.OutCirc,
                HitboxSize = hbSize,
                Event = () =>
                {
                    nextAnimation = "second";
                    RestingEvent = lastFromFirst;
                }
            };

            attackRegistry.AddKeyframe(lastFromFirst);

            attackRegistry.RegisterAnimation("second");
            attackRegistry.AddKeyframe(new AttackKeyframe
                { Direction = 90, Distance = distance, HitboxSize = hbSize });

            lastFromSecond = new AttackKeyframe(time)
            {
                Direction = -90, Distance = distance, Transition = Easing.OutCirc, HitboxSize = hbSize, Event = () =>
                {
                    nextAnimation = "first";
                    RestingEvent = lastFromSecond;
                }
            };
            attackRegistry.AddKeyframe(lastFromSecond);

            attackRegistry.RegisterAnimation("spin");
            attackRegistry.AddKeyframe(new AttackKeyframe { Direction = -90, Distance = distance, HitboxSize = hbSize });
            attackRegistry.AddKeyframe(new AttackKeyframe(1000)
            {
                Direction = 360,
                Distance = distance,
                HitboxSize = hbSize,
                Transition = Easing.OutQuart,
                OnHitEffects = [lastComboEffect],
                ResetHitBox = true,
                Event = () =>
                {
                    nextAnimation = "first";
                    RestingEvent = lastFromFirst;
                }
            });

            #endregion

            #region TextureMapping

            TextureMapping = new();
            TextureMapping.Add("Icon", "brayden_osu_pen_base.png");
            TextureMapping.Add("Base", "brayden_osu_pen_base.png");

            #endregion
        }

        public override void OnRelease()
        {
            base.OnRelease();

            if (didSpin)
            {
                didSpin = false;
                return;
            }

            DrawableInstance.PlayAnimation(attackRegistry.GetAnimation(nextAnimation));
        }

        public override void OnUpdate()
        {
            if (HoldDuration() > spin_time)
            {
                DrawableInstance.PlayAnimation(attackRegistry.GetAnimation("spin"));
                EndAttack();
                didSpin = true;
            }
        }
    }
}
