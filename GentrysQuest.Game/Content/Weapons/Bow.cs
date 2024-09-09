using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using osuTK;

namespace GentrysQuest.Game.Content.Weapons
{
    public class Bow : Weapon
    {
        public override string Type { get; } = "Bow";
        public override int Distance { get; set; } = 1000;
        public override string Name { get; set; } = "Bow";
        public override string Description { get; protected set; } = "Just a bow.";
        public override bool IsGeneralDamageMode { get; protected set; } = false;

        public Bow()
        {
            Damage.SetDefaultValue(20);

            AttackPattern.AddCase(1);
            AttackPattern.Add(new AttackPatternEvent { Size = new Vector2(0.5f), Distance = 50 });
            AttackPattern.Add(new AttackPatternEvent(500) { Size = new Vector2(0.5f), Distance = 40 });
            AttackPattern.Add(new AttackPatternEvent(10) { Size = new Vector2(0.5f), Distance = 30 });
            AttackPattern.Add(new AttackPatternEvent(750)
            {
                Size = new Vector2(0.5f),
                Distance = 50,
                HitboxSize = new Vector2(0),
                MovementSpeed = 0f,
                Projectiles =
                [
                    new ProjectileParameters
                    {
                        Speed = 15,
                        PassthroughAmount = 1,
                        Damage = (int)Damage.Current.Value,
                        TakesHolderDamage = true,
                        TakesDefense = true
                    }
                ]
            });
        }
    }
}
