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
        public override int Distance { get; set; } = 200;
        public override string Name { get; set; } = "Brayden's Osu Pen";

        public override string Description { get; protected set; } = "The man himself, Brayden's osu pen!\n"
                                                                     + "When [condition]held by the true wielder[/condition], gain a [unit]20%[/unit][details]+ 20 per difficulty[/details] increase in all your stats. "
                                                                     + "On the last attack you have a [unit]20%[/unit] chance to [type]bleed[/type] enemies for [unit]6 seconds[/unit]. "
                                                                     + "On a [condition]critical hit[/condition] you get a small boost of speed";

        public override StarRating StarRating { get; protected set; } = new(5);

        public override List<StatType> ValidBuffs { get; set; } = [StatType.CritDamage];

        public BraydensOsuPen()
        {
            Damage.SetDefaultValue(46);

            #region Design

            Origin = Anchor.BottomCentre;

            #endregion

            #region AttackPattern

            const int distance = 35;
            var time = (int)MathBase.SecondToMs(0.5); // seconds
            Vector2 hbSize = new Vector2(0.1f, 1);
            OnHitEffect lastComboEffect = new OnHitEffect(20)
            {
                Effect = new Bleed(new Second(6))
            };

            AttackPattern.AddCase(1);
            AttackPattern.Add(new AttackPatternEvent
            {
                Direction = -90,
                Distance = distance,
                HitboxSize = hbSize,
                Transition = Easing.InOutCirc
            });
            AttackPattern.Add(new AttackPatternEvent(time / 2)
            {
                Direction = -45,
                Distance = distance,
                HitboxSize = hbSize,
                Transition = Easing.InQuart,
                DoesDamage = false,
            });
            AttackPattern.Add(new AttackPatternEvent(time)
            {
                Direction = 90,
                Distance = distance,
                Transition = Easing.OutQuart,
                HitboxSize = hbSize
            });

            AttackPattern.AddCase(2);
            AttackPattern.Add(new AttackPatternEvent
                { Direction = 90, Distance = distance, HitboxSize = hbSize});
            AttackPattern.Add(new AttackPatternEvent(time / 2)
            {
                Direction = 45,
                Distance = distance,
                HitboxSize = hbSize,
                Transition = Easing.InQuart,
                DoesDamage = false
            });
            AttackPattern.Add(new AttackPatternEvent(time)
                { Direction = -90, Distance = distance, Transition = Easing.OutQuart, HitboxSize = hbSize });

            AttackPattern.AddCase(3);
            AttackPattern.Add(new AttackPatternEvent
                { Direction = -90, Distance = distance, HitboxSize = hbSize });
            AttackPattern.Add(new AttackPatternEvent(time / 4)
            {
                Direction = -45,
                Distance = distance,
                HitboxSize = hbSize,
                Transition = Easing.InQuart,
                DoesDamage = false,
                OnHitEffect = lastComboEffect
            });
            AttackPattern.Add(new AttackPatternEvent(time / 2)
            {
                Direction = 90,
                Distance = distance,
                HitboxSize = hbSize,
                OnHitEffect = lastComboEffect,
            });
            AttackPattern.Add(new AttackPatternEvent(time)
            {
                Direction = 360,
                Distance = distance,
                HitboxSize = hbSize,
                Transition = Easing.OutQuart,
                OnHitEffect = lastComboEffect,
                ResetHitBox = true,
            });
            AttackPattern.Add(new AttackPatternEvent(25)
                { Direction = 360 });
            AttackPattern.Add(new AttackPatternEvent(100)
            {
                Direction = 270,
                Distance = distance,
                DoesDamage = false
            });

            #endregion

            #region cases

            OnHitEntity += details =>
            {
                if (details.IsCrit) Holder.AddEffect(new Swiftness());
            };

            #endregion

            #region TextureMapping

            TextureMapping.Add("Icon", "brayden_osu_pen_base.png");
            TextureMapping.Add("Base", "brayden_osu_pen_base.png");

            #endregion
        }
    }
}
