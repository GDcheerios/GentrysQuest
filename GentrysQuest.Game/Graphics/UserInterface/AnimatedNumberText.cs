using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Graphics.UserInterface;

public partial class AnimatedNumberText : GqText
{
    private int displayNumber;

    public int DisplayNumber
    {
        get => displayNumber;
        private set
        {
            displayNumber = value;
            CurrentNumber.Value = value;
        }
    }

    public readonly Bindable<int> CurrentNumber = new();

    public int Duration { get; set; } = 300;

    public AnimatedNumberText()
        : base("")
    {
        CurrentNumber.BindValueChanged(e => Text = e.NewValue.ToString(), true);
    }

    public void SetNumber(int target) => this.TransformTo(nameof(DisplayNumber), target, Duration, Easing.OutQuad);
}
