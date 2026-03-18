using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;

namespace GentrysQuest.Game.Content.Weapons
{
    public class BensLongsword : Weapon
    {
        public override string Name { get; set; } = "Ben's Longsword";

        public override string Description { get; protected set; } = "Ben's Longsword, Long and Bennish.\n"
                                                                     + "Was bought from amazyn.com for at least 50 gentry's quest coins.\n"
                                                                     + "Really effective against [type]Homeless People[/type]";

        public override StarRating StarRating { get; protected set; } = new StarRating(4);
        public override string Type { get; } = "Longsword";
        public override int Distance { get; } = 200;
        public override List<StatType> ValidBuffs { get; set; } = [StatType.Attack, StatType.AttackSpeed];
        public override float DropChance { get; set; } = 0.1f;

        public BensLongsword()
        {
            TextureMapping.Add("Icon", "weapons_benslongsword.png");
            TextureMapping.Add("Base", "weapons_benslongsword.png");

            Damage.SetDefaultValue(40);
        }
    }
}
