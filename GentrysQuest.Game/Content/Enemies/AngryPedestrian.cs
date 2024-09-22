using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Enemies
{
    public class AngryPedestrian : Enemy
    {
        public override string Name { get; set; } = "Angry Pedestrian";
        public override string Description { get; protected set; } = "A very very angry pedestrian for whatever reason";

        public AngryPedestrian()
        {
            TextureMapping.Add("Idle", "enemies_angry_pedestrian_idle.jpeg");

            WeaponChoices.AddChoice(new Knife(), 100);
        }
    }
}
