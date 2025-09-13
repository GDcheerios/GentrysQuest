using System.Collections.Generic;
using System.Text.RegularExpressions;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace GentrysQuest.Game.Graphics.TextStyles
{
    public partial class TaggedTextContainer : TextFlowContainer
    {
        public int Font { get; set; } = 16;

        private static readonly Dictionary<string, Color4> TagColors = new()
        {
            { "condition", Color4.Gold },
            { "unit", Color4.Aqua },
            { "details", Color4.Gray },
            { "type", Color4.Red },
            { "stat", Color4.Green }
        };

        private static readonly Regex TagRegex = new Regex(@"\[(\w+)\](.*?)\[\/\1\]", RegexOptions.Compiled);

        [BackgroundDependencyLoader]
        private void load()
        {
            Flow.AutoSizeAxes = Axes.None;
            Flow.RelativeSizeAxes = Axes.Both;
        }

        public void SetTaggedText(string input)
        {
            Clear(); // Clear existing children

            var matches = TagRegex.Matches(input);
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                // Add plain text before the tag
                if (match.Index > lastIndex)
                {
                    string plainText = input.Substring(lastIndex, match.Index - lastIndex);
                    AddTextWithNewLines(plainText, Color4.White);
                }

                // Add tagged text
                string tag = match.Groups[1].Value;
                string content = match.Groups[2].Value;
                AddTextWithNewLines(content, TagColors.TryGetValue(tag, out Color4 color) ? color : Color4.White);

                lastIndex = match.Index + match.Length;
            }

            // Add any remaining plain text after the last tag
            if (lastIndex < input.Length)
            {
                string remainingText = input.Substring(lastIndex);
                AddTextWithNewLines(remainingText, Color4.White);
            }
        }

        private void AddTextWithNewLines(string text, Color4 color)
        {
            var lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 0)
                {
                    NewLine();
                }

                AddWordsAsSprites(lines[i], color);
            }
        }

        private void AddWordsAsSprites(string text, Color4 color)
        {
            var words = text.Split(' ');

            foreach (var word in words)
            {
                AddText(word + " ", t =>
                {
                    t.Colour = color;
                    t.Font = FontUsage.Default.With(size: Font);
                });
            }
        }

        public IEnumerable<Drawable> GetSpriteTexts()
        {
            return Flow.Children;
        }
    }
}
