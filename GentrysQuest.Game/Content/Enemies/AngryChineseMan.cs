using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Enemies
{
    public class AngryChineseMan : Enemy
    {
        public override string Name { get; set; } = "Angry Chinese Man";
        public override string Description { get; protected set; } = "He so angry";

        public AngryChineseMan()
        {
            Stats.Speed.point = 2;

            TextureMapping.Add("Idle", "enemies_angry_chinese_man_idle.gif");

            AudioMapping.Add("Spawn", "enemies_angry_chinese_man_spawn.m4a");

            WeaponChoices.AddChoice(new BrodysBroadsword(), 100);
        }
    }
}
