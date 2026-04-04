using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace GentrysQuest.Game.Graphics
{
    public partial class GqTextBox : BasicTextBox
    {
        protected override float LeftRightPadding => 10;
        protected override float CaretWidth => 5;

        [BackgroundDependencyLoader]
        private void load()
        {
            Placeholder.Colour = Color4.White;
            BackgroundUnfocused = new Color4(0, 0, 0, 10);
            BackgroundFocused = new Color4(10, 10, 10, 45);
            BackgroundCommit = new Color4(45, 45, 45, 80);
        }
    }
}
