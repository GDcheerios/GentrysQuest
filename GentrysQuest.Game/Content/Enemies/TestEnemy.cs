using GentrysQuest.Game.Content.Artifacts;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Enemies
{
    public class TestEnemy : Enemy
    {
        public TestEnemy()
        {
            Name = "Test Enemy";

            TextureMapping = new();

            WeaponChoices.AddChoice(new Bow());
            WeaponChoices.AddChoice(new Knife());
            WeaponChoices.AddChoice(new Sword());
            WeaponChoices.AddChoice(new Spear());
            WeaponChoices.AddChoice(new Hammer());
            WeaponChoices.AddChoice(new BraydensOsuPen());

            ArtifactChoices.GuaranteeDrop = false;
            ArtifactChoices.MultipleDrop = false;

            ArtifactChoices.AddChoice(new ElHefe());
            ArtifactChoices.AddChoice(new EmptyGachaContainer());
            ArtifactChoices.AddChoice(new EnergyDrink());
            ArtifactChoices.AddChoice(new OsuTablet());
            ArtifactChoices.AddChoice(new MadokaChibiPlush());
            ArtifactChoices.AddChoice(new Keyboard());
        }
    }
}
