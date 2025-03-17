using System;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Graphics.Dialogue;

public partial class DialogueTextFlowContainer : TextFlowContainer
{
    private readonly double characterAppearanceDuration;
    private const int FONT_SIZE = 30;

    public DialogueTextFlowContainer(double characterAppearanceDuration = 50)
    {
        this.characterAppearanceDuration = characterAppearanceDuration;
    }

    public double AddTextWithEffect(string text, double totalDuration = 0)
    {
        double characterAppearanceDuration = totalDuration / text.Length;

        var parts = addTextReturningParts(text, spriteText =>
        {
            spriteText.Alpha = 0;
        });

        double currentTimeOffset = 0;

        foreach (var part in parts)
        {
            Scheduler.AddDelayed(() =>
            {
                part.FadeIn(characterAppearanceDuration);
            }, currentTimeOffset);

            currentTimeOffset += characterAppearanceDuration;
        }

        return currentTimeOffset;
    }

    private SpriteText[] addTextReturningParts(string text, Action<SpriteText> creationParameters)
    {
        return text.Select(character =>
        {
            var spriteText = CreateSpriteText();
            spriteText.Font = new FontUsage(size: FONT_SIZE);
            spriteText.Text = character.ToString();
            creationParameters?.Invoke(spriteText);
            AddInternal(spriteText);
            return spriteText;
        }).ToArray();
    }
}
