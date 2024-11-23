using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Game.Utils
{
    /// <summary>
    /// Some classes don't have access to a clock because of the hierarchy.
    /// So I'm making a helper class!
    /// </summary>
    public partial class GameClock : CompositeDrawable
    {
        private static GameClock instance;
        public static double CurrentTime => (instance ??= new GameClock()).Clock.CurrentTime;
        public static double FrameTime => (instance ??= new GameClock()).Clock.ElapsedFrameTime;
    }
}
