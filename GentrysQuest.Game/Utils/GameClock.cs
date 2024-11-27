using System;
using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Game.Utils
{
    public partial class GameClock : CompositeDrawable
    {
        private static GameClock instance;

        public static double CurrentTime => getInstance()?.Clock?.CurrentTime ?? 0;
        public static double FrameTime => getInstance()?.Clock?.ElapsedFrameTime ?? 0;

        private static GameClock getInstance()
        {
            if (instance == null)
            {
                throw new InvalidOperationException("GameClock has not been initialized in the drawable hierarchy.");
            }

            return instance;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            instance = this;
        }
    }
}
