using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Weapons
{
    public class Knife : Weapon
    {
        public override string Type => "Knife";
        public override int Distance => 150;
        public override string Name { get; set; } = "Knife";
        public override string Description { get; protected set; } = "Just a knife...";

        private readonly AttackAnimationRegistry attackAnimationRegistry = new AttackAnimationRegistry();

        public Knife()
        {
            Damage.SetDefaultValue(16);

            #region Design

            Origin = Anchor.BottomCentre;

            #endregion

            #region AttackPattern

            var time = (int)MathBase.SecondToMs(0.4); // seconds

            attackAnimationRegistry.RegisterAnimation("stab");
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe { Distance = 0, HitboxSize = new Vector2(0, 0), Size = new Vector2(0.6f) });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(time) { Distance = 15, HitboxSize = new Vector2(0f, 0), Size = new Vector2(0.6f), DoesDamage = false });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(time) { Distance = 65, HitboxSize = new Vector2(0.1f, 1), Size = new Vector2(0.6f) });
            attackAnimationRegistry.AddKeyframe(new AttackKeyframe(time) { Distance = 0, HitboxSize = new Vector2(0.1f, 0), Size = new Vector2(0.6f) });

            #endregion

            #region TextureMapping

            TextureMapping = new();
            TextureMapping.Add("Icon", "knife.png");
            TextureMapping.Add("Base", "knife.png");

            #endregion
        }

        public override void OnClick(float direction)
        {
            base.OnClick(direction);
            DrawableInstance.PlayAnimation(attackAnimationRegistry.GetAnimation("stab"));
        }
    }
}
