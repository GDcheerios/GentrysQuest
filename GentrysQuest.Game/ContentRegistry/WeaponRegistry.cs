using System;
using System.Collections.Generic;
using GentrysQuest.Game.Entity.Weapon;

namespace GentrysQuest.Game.ContentRegistry
{
    public static class WeaponRegistry
    {
        private static readonly Dictionary<string, Func<Weapon>> map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Sword"] = () => new Content.Weapons.Sword(),
            ["Bow"] = () => new Content.Weapons.Bow(),
            ["Hammer"] = () => new Content.Weapons.Hammer(),
            ["Knife"] = () => new Content.Weapons.Knife(),
            ["Spear"] = () => new Content.Weapons.Spear(),
            ["Brodys Broadsword"] = () => new Content.Weapons.BrodysBroadsword(),
            ["Braydens Osu Pen"] = () => new Content.Weapons.BraydensOsuPen()
        };

        public static Weapon Create(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            if (map.TryGetValue(key.Trim(), out var ctor)) return ctor();

            return null;
        }
    }
}
