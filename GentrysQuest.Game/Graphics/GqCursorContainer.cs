using osu.Framework.Graphics.Cursor;

namespace GentrysQuest.Game.Graphics
{
    public partial class GqCursorContainer : CursorContainer
    {
        public GqCursorContainer() => ActiveCursor = new GqCursor();
    }
}
