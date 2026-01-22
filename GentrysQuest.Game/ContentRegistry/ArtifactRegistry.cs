using System;
using System.Collections.Generic;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.ContentRegistry
{
    public static class ArtifactRegistry
    {
        private static readonly Dictionary<string, Func<Artifact>> map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Osu Tablet"] = () => new Content.Families.BraydenMesserschmidt.OsuTablet(),
            ["Madoka Chibi Plush"] = () => new Content.Families.BraydenMesserschmidt.MadokaChibiPlush(),
            ["Keyboard"] = () => new Content.Families.Intro.Keyboard(),
            ["El Hefe"] = () => new Content.Families.JVee.ElHefe()
        };

        public static Artifact Create(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            if (map.TryGetValue(key.Trim(), out var ctor)) return ctor();

            return null;
        }
    }
}
