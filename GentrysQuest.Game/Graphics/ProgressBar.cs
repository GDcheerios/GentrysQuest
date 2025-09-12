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

    public Bindable<float> Current = new();
    public Bindable<float> Max = new(1);

    public Colour4 BackgroundColour { get; set; } = Colour4.DarkGray;
    public Colour4 ForegroundColour { get; set; } = Colour4.White;

    public ProgressBar()
    {
        RelativeSizeAxes = Axes.Both;
        Current.ValueChanged += _ => foreground.ResizeWidthTo((float)(Current.Value / Max.Value), DELAY, Easing.InOutCirc);
        Max.ValueChanged += _ => foreground.ResizeWidthTo((float)(Current.Value / Max.Value), DELAY, Easing.InOutCirc);
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
    }
}
