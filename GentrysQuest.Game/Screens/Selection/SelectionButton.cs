using GentrysQuest.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Screens.Selection
{
    public partial class SelectionButton : GQButton
    {
        private Box background;

        public void Activate() => background.Colour = Colour4.Gray;
        public void Deactivate() => background.Colour = Colour4.Black;
    }
}
