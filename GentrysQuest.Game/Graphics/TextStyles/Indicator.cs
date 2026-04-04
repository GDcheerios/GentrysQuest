using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Graphics.TextStyles;

public partial class Indicator : SpriteText
{
    public Indicator(string text) => Text = $"{text}";

    [BackgroundDependencyLoader]
    private void load()
    {
        Anchor = Anchor.TopCentre;
        Origin = Anchor.BottomCentre;
        AllowMultiline = false;
        Alpha = 0;
        Scale = new Vector2(0.6f);
        animate();
    }

    /// <summary>
    /// Fades and moves the indicator text, then expires it.
    /// </summary>
    private void animate()
    {
        const int moveDuration = 700;
        const int fadeOutDuration = 450;

        this.FadeIn(80, Easing.OutQuint);
        this.ScaleTo(1.1f, 120, Easing.OutBack)
            .Then()
            .ScaleTo(1f, 120, Easing.Out);

        this.MoveToX(X + 28, moveDuration, Easing.OutSine);
        this.MoveToY(Y - 120, moveDuration, Easing.OutQuint);

        this.Delay(180)
            .Then()
            .FadeOut(fadeOutDuration, Easing.In);

        this.Delay(moveDuration + fadeOutDuration)
            .Then()
            .Expire();
    }
}
