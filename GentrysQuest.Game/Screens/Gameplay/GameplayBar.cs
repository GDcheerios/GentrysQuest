using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Screens.Gameplay;

public partial class GameplayBar : ProgressBar
{
    [BackgroundDependencyLoader]
    private void load()
    {
        SpriteText spriteText = new SpriteText
        {
            Text = $"{Current.Value}/{Max.Value}",
            Font = FontUsage.Default.With(size: 32),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Colour = Colour4.White
        };
        Current.ValueChanged += delegate { spriteText.Text = $"{Current.Value}/{Max.Value}"; };
        Max.ValueChanged += delegate { spriteText.Text = $"{Current.Value}/{Max.Value}"; };
        AddInternal(spriteText);
    }
}
