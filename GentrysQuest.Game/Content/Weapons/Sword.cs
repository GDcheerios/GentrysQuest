using GentrysQuest.Game.Entity.Weapon;
using osu.Framework.Graphics;
using osuTK;

namespace GentrysQuest.Game.Content.Weapons
{
    public class Sword : Weapon
    {
        public override string Name { get; set; } = "Sword";
        public override string Type { get; } = "Sword";
        public override int Distance { get; set; } = 250;
        public override string Description { get; protected set; } = "Just a sword";

        public Sword()
        {
            TextureMapping.Add("Icon", "weapons_sword.png");
            TextureMapping.Add("Base", "weapons_sword.png");

            Damage.SetDefaultValue(22);

            Vector2 size = new Vector2(0.25f, 1);

            AttackPattern.AddCase(1);
            AttackPattern.Add(new AttackPatternEvent { Direction = 115, Distance = 100, Size = size });
            AttackPattern.Add(new AttackPatternEvent(650) { Direction = -90, Distance = 100, Size = size, Transition = Easing.In, KnockbackMultiplier = 1.5f });
            AttackPattern.Add(new AttackPatternEvent(100) { Direction = -115, Distance = 100, Size = size, Transition = Easing.Out, KnockbackMultiplier = 1.5f });

            AttackPattern.AddCase(2);
            AttackPattern.Add(new AttackPatternEvent { Direction = -115, Distance = 100, Size = size });
            AttackPattern.Add(new AttackPatternEvent(650) { Direction = 90, Distance = 100, Size = size, Transition = Easing.In, KnockbackMultiplier = 1.5f });
            AttackPattern.Add(new AttackPatternEvent(100) { Direction = 115, Distance = 100, Size = size, Transition = Easing.Out, KnockbackMultiplier = 1.5f });
        }
    }
}
