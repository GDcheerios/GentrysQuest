using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;

namespace GentrysQuest.Game.Content.Weapons;

public class TestWeapon : Weapon
{
    public override string Type { get; } = "Test";
    public override int Distance { get; } = int.MaxValue;
    public override string Name { get; set; } = "Test Weapon";

    public TestWeapon(int starRating) => StarRating = new StarRating(starRating);
}
