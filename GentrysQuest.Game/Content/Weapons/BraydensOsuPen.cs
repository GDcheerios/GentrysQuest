using System.Collections.Generic;
using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Input;
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
                                                                     + "On a spinning attack you have a [unit]20%[/unit] chance to [type]bleed[/type] enemies for [unit]6 seconds[/unit]. ";

        public override StarRating StarRating { get; protected set; } = new(5);

        public override List<StatType> ValidBuffs { get; set; } = [StatType.CritDamage];

        private AttackPattern mainAttack = new AttackPattern();
        private AttackPatternCaseHolder spinningAttack = new AttackPatternCaseHolder(0);

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

            mainAttack.AddCase(1);
            mainAttack.Add(new AttackPatternEvent
            {
                Direction = -90,
                Distance = distance,
                HitboxSize = hbSize,
            });
            mainAttack.Add(new AttackPatternEvent(time)
            {
                Direction = 90,
                Distance = distance,
                Transition = Easing.OutCirc,
                HitboxSize = hbSize
            });

            mainAttack.AddCase(2);
            mainAttack.Add(new AttackPatternEvent
                { Direction = 90, Distance = distance, HitboxSize = hbSize });
            mainAttack.Add(new AttackPatternEvent(time)
                { Direction = -90, Distance = distance, Transition = Easing.OutCirc, HitboxSize = hbSize });

            RestingEvent = mainAttack.GetFirstCaseEvent();

            #endregion

            #region cases

            OnHitEntity += details =>
            {
                if (details.IsCrit) Holder.AddEffect(new Swiftness(new Second(0.5)));
            };

            #endregion

            #region TextureMapping

            TextureMapping.Add("Icon", "brayden_osu_pen_base.png");
            TextureMapping.Add("Base", "brayden_osu_pen_base.png");

            #endregion
        }

        public override void OnAttack(HoldEvent holdEvent)
        {
            base.OnAttack(holdEvent);

            if (holdEvent.MeetsHoldCondition(500))
            {
                CurrentCase = spinningAttack;
                EndAttack();
            }

            if (!holdEvent.IsPressed)
            {
                CurrentCase = mainAttack.GetCase(AttackCaseCounter);
                EndAttack();
            }
        }
    }
}
