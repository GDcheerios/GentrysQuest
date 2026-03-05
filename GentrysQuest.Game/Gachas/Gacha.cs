using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Content.Artifacts;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Users;
using GentrysQuest.Game.Utils;

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
        public List<Character> Characters { get; set; } = [];

        /// <summary>
        /// The list of weapons you can receive in a gacha roll
        /// </summary>
        public List<Weapon> Weapons { get; set; } = [];

        /// <summary>
        /// rolls the gacha for characters to the desired user.
        /// </summary>
        /// <param name="amount">amount to roll</param>
        /// <param name="user">user to target</param>
        /// <returns>List of the retrieved characters</returns>
        public List<Character> RollCharacter(int amount, IUser user)
        {
            int price = (int)(Price * amount);
            List<Character> rolledCharacters = [];
            if (!user.MoneyHandler.CanAfford(price)) return [];

            for (int i = 0; i < amount; i++)
            {
                int starRating = MathBase.RandomGachaStarRating();
                while (Characters.Count(w => w.StarRating == starRating) == 0) starRating = MathBase.RandomGachaStarRating();
                List<Character> validCharacters = Characters.Where(c => c.StarRating == starRating).ToList();
                rolledCharacters.Add(validCharacters[MathBase.RandomChoice(validCharacters.Count)]);
            }

            if (rolledCharacters.Count != 0)
            {
                user.MoneyHandler.Spend(price);

                List<Character> duplicateCharacters = [];

                foreach (Character rolledCharacter in rolledCharacters)
                {
                    duplicateCharacters.AddRange(from userCharacter in user.Characters where rolledCharacter.Name == userCharacter.Name select rolledCharacter);
                    if (!duplicateCharacters.Contains(rolledCharacter)) user.Characters.Add(rolledCharacter);
                }

                foreach (Character _ in duplicateCharacters) user.AddItem(new EmptyGachaContainer());
            }

            return rolledCharacters;
        }

        /// <summary>
        /// rolls the gacha for weapons to the desired user.
        /// </summary>
        /// <param name="amount">amount to roll</param>
        /// <param name="user">user to target</param>
        /// <returns>List of the retrieved weapons</returns>
        public List<Weapon> RollWeapon(int amount, IUser user)
        {
            int price = (int)(Price * amount);
            List<Weapon> rolledWeapons = [];
            if (user.MoneyHandler != null && !user.MoneyHandler.CanAfford((int)(Price * amount))) return [];

            for (int i = 0; i < amount; i++)
            {
                int starRating = MathBase.RandomGachaStarRating();
                while (Weapons.Count(w => w.StarRating == starRating) == 0) starRating = MathBase.RandomGachaStarRating();
                List<Weapon> validWeapons = Weapons.Where(w => w.StarRating == starRating).ToList();
                Weapon weapon = validWeapons[MathBase.RandomChoice(validWeapons.Count)];
                rolledWeapons.Add(weapon);
                user.AddItem(weapon);
            }

            if (rolledWeapons.Count != 0) user.MoneyHandler!.Spend(price);
            return rolledWeapons;
        }
    }
}
