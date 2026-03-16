using System.Collections.Generic;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Entity
{
    public class WeaponChoices
    {
        private List<Weapon.Weapon> weapons = new();
        private List<int> chanceOfPicking = new();

        public void AddChoice(Weapon.Weapon weapon, int chanceOfPicking = 50)
        {
            weapons.Add(weapon);
            this.chanceOfPicking.Add(chanceOfPicking);
        }

        public Weapon.Weapon GetChoice()
        {
            while (true)
            {
                int i = MathBase.RandomChoice(weapons.Count);
                if (MathBase.IsChanceSuccessful(chanceOfPicking[i], 100))
                    return weapons[i];
            }
        }
    }
}
