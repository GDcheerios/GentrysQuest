using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Graphics.Dialogue;

public partial class DialogueTextFlowContainer : TextFlowContainer
{
    private readonly double characterAppearanceDuration;
    private const int FONT_SIZE = 30;

    public DialogueTextFlowContainer(double characterAppearanceDuration = 100) => this.characterAppearanceDuration = characterAppearanceDuration;

    public double AddTextWithEffect(string text, double totalDuration = 0)
    {
        string strippedText = System.Text.RegularExpressions.Regex.Replace(text, @"\[p:\d+\]", "");
        double currentTimeOffset = 0;

        string[] words = System.Text.RegularExpressions.Regex.Split(text, @"(?<=\s)");

        foreach (var word in words)
        {
            int currentIndex = 0;

            while (currentIndex < word.Length)
            {
                if (word[currentIndex] == '[' && currentIndex + 1 < word.Length && word[currentIndex + 1] == 'p')
                {
                    int closingBracket = word.IndexOf(']', currentIndex);

                    if (closingBracket != -1)
                    {
                        string tag = word.Substring(currentIndex + 1, closingBracket - currentIndex - 1);
                        string[] parts = tag.Split(':');

                        if (parts.Length == 2 && double.TryParse(parts[1], out double pauseDuration))
                        {
                            currentTimeOffset += pauseDuration;
                            currentIndex = closingBracket + 1;
                            continue;
                        }
                    }
                }

                int nextTag = word.IndexOf("[p:", currentIndex);
                string textSegment = nextTag == -1 ? word.Substring(currentIndex) : word.Substring(currentIndex, nextTag - currentIndex);

                if (!string.IsNullOrEmpty(textSegment))
                {
                    var textPart = AddText(textSegment, t =>
                    {
                        t.Font = new FontUsage(size: FONT_SIZE);
                        t.Alpha = 0;
                    });

                    foreach (var drawable in textPart.Drawables)
                    {
                        Scheduler.AddDelayed(() =>
                        {
                            drawable.FadeIn(characterAppearanceDuration);
                        }, currentTimeOffset);

                        currentTimeOffset += characterAppearanceDuration;
                    }

                    currentIndex += textSegment.Length;
                }
                else
                {
                    currentIndex++;
                }
            }
        }

        return currentTimeOffset;
    }
}
