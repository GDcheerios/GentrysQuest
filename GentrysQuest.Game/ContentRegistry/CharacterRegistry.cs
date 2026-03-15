using System;
using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using osu.Framework.Logging;

namespace GentrysQuest.Game.ContentRegistry
{
    public static class CharacterRegistry
    {
        private static readonly Dictionary<string, Func<Character>> map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Brayden Messerschmidt"] = () => new Content.Characters.BraydenMesserschmidt(),
            ["GMoney"] = () => new Content.Characters.GMoney(),
            ["Mekhi Elliot"] = () => new Content.Characters.MekhiElliot(),
            ["Philip McClure"] = () => new Content.Characters.PhilipMcClure()
        };

        public static Character Create(string key)
        {
            Logger.Log($"Creating character {key}");
            if (string.IsNullOrWhiteSpace(key)) return null;
            if (map.TryGetValue(key.Trim(), out var ctor)) return ctor();

            return null;
        }
    }
}
