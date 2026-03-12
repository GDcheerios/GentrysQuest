using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Graphics;

public partial class ProgressBar : CompositeDrawable
{
    private const int DELAY = 200;

    private Box background;
    private Box foreground;

    public Bindable<float> Current;
    public Bindable<float> Max;

    public Colour4 BackgroundColour { get; set; } = Colour4.DarkGray;
    public Colour4 ForegroundColour { get; set; } = Colour4.White;
    public bool Animate { get; set; } = true;
    public float MaxInit { get; set; } = 1;
    public float CurrentInit { get; set; } = 0;

    public ProgressBar()
    {
        Current = new Bindable<float>(CurrentInit);
        Max = new Bindable<float>(MaxInit);
        RelativeSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren =
        [
            background = new Box
            {
                Colour = BackgroundColour,
                RelativeSizeAxes = Axes.Both
            },
            foreground = new Box
            {
                Colour = ForegroundColour,
                RelativeSizeAxes = Axes.Both
            }
        ];

        if (Animate)
        {
            Current.ValueChanged += _ => updateForegroundWidth(DELAY, Easing.InOutCirc);
            Max.ValueChanged += _ => updateForegroundWidth(DELAY, Easing.InOutCirc);
        }
        else
        {
            Current.ValueChanged += _ => updateForegroundWidth(0, Easing.None);
            Max.ValueChanged += _ => updateForegroundWidth(0, Easing.None);
        }

        updateForegroundWidth(0, Easing.None);
    }

    private void updateForegroundWidth(double duration, Easing easing)
    {
        float width = 0;

        if (float.IsFinite(Current.Value) && float.IsFinite(Max.Value) && Max.Value > 0)
        {
            width = Current.Value / Max.Value;
            width = float.Clamp(width, 0, 1);
        }

        foreground.ResizeWidthTo(width, duration, easing);
    }
}
