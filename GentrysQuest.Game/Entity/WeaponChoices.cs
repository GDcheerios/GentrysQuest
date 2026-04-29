using System;
using System.Collections.Generic;
using GentrysQuest.Game.Utils;

namespace GentrysQuest.Game.Entity
{
    public class WeaponChoices
    {
        private readonly List<Weapon.Weapon> weapons = new();
        private readonly List<int> chanceOfPicking = new();

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
                    return createWeaponInstance(weapons[i]);
            }
        }

        private static Weapon.Weapon createWeaponInstance(Weapon.Weapon weapon)
        {
            if (weapon == null) return null;

            return Activator.CreateInstance(weapon.GetType()) as Weapon.Weapon;
        }
    }
}
