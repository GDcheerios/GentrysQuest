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
                                                                     + "On a spinning attack you have a [unit]20%[/unit] chance to [type]bleed[/type] enemies for [unit]6 seconds[/unit]. ";

        public override StarRating StarRating { get; protected set; } = new(5);

        public override List<StatType> ValidBuffs { get; set; } = [StatType.CritDamage];

        private AttackPattern mainAttack = new AttackPattern();
        private AttackPattern spinningAttack = new AttackPattern();

        public override AttackPatternEvent RestingEvent { get; protected set; } = new AttackPatternEvent(50)
        {
            Direction = -105
        };

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

            mainAttack.AddCase();
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

            mainAttack.AddCase();
            mainAttack.Add(new AttackPatternEvent
                { Direction = 90, Distance = distance, HitboxSize = hbSize });
            mainAttack.Add(new AttackPatternEvent(time)
                { Direction = -90, Distance = distance, Transition = Easing.OutCirc, HitboxSize = hbSize });

            spinningAttack.AddCase();
            spinningAttack.Add(new AttackPatternEvent { Direction = -90, Distance = distance, HitboxSize = hbSize });
            spinningAttack.Add(new AttackPatternEvent(1000)
            {
                Direction = 360,
                Distance = distance,
                HitboxSize = hbSize,
                Transition = Easing.OutQuart,
                OnHitEffects = [lastComboEffect],
                ResetHitBox = true,
                InteruptAttack = true
            });

            RestingEvent = mainAttack.GetFirstCaseEvent();

            #endregion

            #region TextureMapping

            TextureMapping.Add("Icon", "brayden_osu_pen_base.png");
            TextureMapping.Add("Base", "brayden_osu_pen_base.png");

            #endregion
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (HoldDuration >= 250)
            {
                CurrentCase = GetCase(spinningAttack);
                EndAttack();
            }

            CurrentCase = GetCase(mainAttack);
        }

        public override void EndAttack()
        {
            base.EndAttack();
            RestingEvent = GetCurrentCase().GetLastEvent();
        }
    }
}
