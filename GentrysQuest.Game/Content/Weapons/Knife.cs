using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Weapons
{
    public class Knife : Weapon
    {
        public override string Type { get; } = "Knife";
        public override int Distance { get; set; } = 150;
        public override string Name { get; set; } = "Knife";
        public override string Description { get; protected set; } = "Just a knife...";

        private readonly AttackPattern attackPattern = new AttackPattern();

        public Knife()
        {
            Damage.SetDefaultValue(16);

            #region Design

            Origin = Anchor.BottomCentre;

            #endregion

            #region AttackPattern

            var time = (int)MathBase.SecondToMs(0.4); // seconds

            attackPattern.AddCase(1);

            attackPattern.Add(new AttackPatternEvent { Distance = 0, HitboxSize = new Vector2(0, 0), Size = new Vector2(0.6f) });
            attackPattern.Add(new AttackPatternEvent(time) { Distance = 15, HitboxSize = new Vector2(0f, 0), Size = new Vector2(0.6f), DoesDamage = false });
            attackPattern.Add(new AttackPatternEvent(time) { Distance = 65, HitboxSize = new Vector2(0.1f, 1), Size = new Vector2(0.6f) });
            attackPattern.Add(new AttackPatternEvent(time) { Distance = 0, HitboxSize = new Vector2(0.1f, 0), Size = new Vector2(0.6f) });

            #endregion

            #region TextureMapping

            TextureMapping.Add("Icon", "knife.png");
            TextureMapping.Add("Base", "knife.png");

            #endregion
        }
    }
}
