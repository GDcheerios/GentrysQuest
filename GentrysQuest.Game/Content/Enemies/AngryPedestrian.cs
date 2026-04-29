using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.AI;

namespace GentrysQuest.Game.Content.Enemies
{
    public class AngryPedestrian : Enemy
    {
        public override string Name { get; set; } = "Angry Pedestrian";
        public override string Description { get; protected set; } = "A very very angry pedestrian for whatever reason";

        public AngryPedestrian()
        {
            TextureMapping = new();
            TextureMapping.Add("Idle", "enemies_angry_pedestrian_idle.jpeg");

            AiProfile = AiProfile.Aggressive();
            WeaponChoices.AddChoice(new Knife(), 100);
        }
    }
}
