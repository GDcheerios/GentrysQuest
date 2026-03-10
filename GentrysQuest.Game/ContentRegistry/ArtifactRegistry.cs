using System;
using System.Collections.Generic;
using GentrysQuest.Game.Content.Artifacts;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.ContentRegistry
{
    public static class ArtifactRegistry
    {
        private static readonly Dictionary<string, Func<Artifact>> map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Osu Tablet"] = () => new OsuTablet(),
            ["Madoka Chibi Plush"] = () => new MadokaChibiPlush(),
            ["Keyboard"] = () => new Keyboard(),
            ["El Hefe"] = () => new ElHefe()
        };

        public static Artifact Create(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            if (map.TryGetValue(key.Trim(), out var ctor)) return ctor();

            return null;
        }
    }
}
