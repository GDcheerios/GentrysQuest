using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.AI;

namespace GentrysQuest.Game.Content.Enemies
{
    public class AngryChineseMan : Enemy
    {
        public override string Name { get; set; } = "Angry Chinese Man";
        public override string Description { get; protected set; } = "He so angry";

        public AngryChineseMan()
        {
            Stats.Speed.Point = 2;

            TextureMapping = new();
            TextureMapping.Add("Idle", "enemies_angry_chinese_man_idle.gif");

            AudioMapping.Add("Spawn", "enemies_angry_chinese_man_spawn.m4a");

            AiProfile = AiProfile.Aggressive();
            WeaponChoices.AddChoice(new Spear(), 100);
        }
    }
}
