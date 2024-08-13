using GentrysQuest.Game.Entity.Weapon;
using osuTK;

namespace GentrysQuest.Game.Content.Weapons
{
    public class Spear : Weapon
    {
        public override string Type { get; } = "Spear";
        public override int Distance { get; set; } = 300;
        public override string Name { get; set; } = "Spear";
        public override string Description { get; protected set; } = "Just a spear";

        public Spear()
        {
            Damage.SetDefaultValue(23);

            TextureMapping.Add("Icon", "weapons_spear.png");
            TextureMapping.Add("Base", "weapons_spear.png");

            Vector2 hitbox = new Vector2(0.05f, 1);
            Vector2 size = new Vector2(1, 1.5f);

            AttackPattern.AddCase(1);
            AttackPattern.Add(new AttackPatternEvent { Position = new Vector2(50, 0), HitboxSize = hitbox, Size = size });
            AttackPattern.Add(new AttackPatternEvent(50) { Position = new Vector2(25, 0), HitboxSize = hitbox, Size = size, DoesDamage = false });
            AttackPattern.Add(new AttackPatternEvent(125) { Distance = 50, Position = new Vector2(0, 0), HitboxSize = hitbox, Size = size, DoesDamage = false });
            AttackPattern.Add(new AttackPatternEvent(125) { Distance = 100, Position = new Vector2(0, 0), HitboxSize = hitbox, Size = size });
            AttackPattern.Add(new AttackPatternEvent(100) { Distance = 100, Position = new Vector2(0, 0), HitboxSize = hitbox, Size = size });
            AttackPattern.Add(new AttackPatternEvent(250) { Distance = 0, Position = new Vector2(50, 0), HitboxSize = hitbox, Size = size, DoesDamage = false});
        }
    }
}
