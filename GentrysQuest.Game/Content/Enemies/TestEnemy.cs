using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Enemies
{
    public class TestEnemy : Enemy
    {
        public TestEnemy()
        {
            Name = "Test Enemy";

            WeaponChoices.AddChoice(new Bow(), 33);
            WeaponChoices.AddChoice(new Knife(), 33);
            WeaponChoices.AddChoice(new Sword(), 33);
            WeaponChoices.AddChoice(new Spear(), 33);
            WeaponChoices.AddChoice(new Hammer(), 33);
        }
    }
}
