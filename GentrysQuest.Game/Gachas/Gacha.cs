using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;

namespace GentrysQuest.Game.Gachas
{
    public class Gacha
    {
        /// <summary>
        /// The name of the gacha
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The price of one gacha roll
        /// </summary>
        public uint Price { get; set; }

        /// <summary>
        /// The list of characters you can receive in a gacha roll
        /// </summary>
        public List<Character> Characters { get; set; }

        /// <summary>
        /// The list of weapons you can receive in a gacha roll
        /// </summary>
        public List<Weapon> Weapons { get; set; }
    }
}
