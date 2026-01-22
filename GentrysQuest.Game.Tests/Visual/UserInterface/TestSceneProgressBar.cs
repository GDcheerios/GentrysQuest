using GentrysQuest.Game.Graphics;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Tests.Visual.UserInterface;

[TestFixture]
public partial class TestSceneProgressBar : GentrysQuestTestScene
{
    private Container theContainer;
    private ProgressBar testProgressBar;

    public TestSceneProgressBar()
    {
        theContainer = new Container
        {
            Children = new Drawable[]
            {
                testProgressBar = new ProgressBar()
            },
            RelativeSizeAxes = Axes.Both,
            RelativePositionAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(0.5f, 0.12f)
        };
        Add(theContainer);
    }

    [Test]
    public void ChangeProgress()
    {
        AddSliderStep("progress", 0, 1, 0.5, _ => testProgressBar.Current.Value = (float)_);
    }
}
