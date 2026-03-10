using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Overlays.GameMenu.GachaTab;

public partial class GachaRollButton : GqButton
{
    private GqText buttonText;

    [BackgroundDependencyLoader]
    private void load()
    {
        Masking = true;
        CornerRadius = 10;
        CornerExponent = 2;
        Children =
        [
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = new Colour4(35, 35, 35, 255),
            },
            buttonText = new GqText("Roll")
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            }
        ];
    }

    public void SetText(string text) => buttonText.Text = text;
}
