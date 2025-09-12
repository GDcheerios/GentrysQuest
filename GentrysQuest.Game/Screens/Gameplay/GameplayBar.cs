using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Screens.Gameplay;

public partial class GameplayBar : ProgressBar
{
    private readonly SpriteText barText;

    public GameplayBar()
        : base()
    {
        AddInternal(
            barText = new SpriteText
            {
                Text = $"{0}/{Max.Value}",
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Font = FontUsage.Default.With(size: 45)
            });
    }
}
