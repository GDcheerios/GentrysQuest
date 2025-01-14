using System;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Utils
{
    public partial class GameClock : CompositeDrawable
    {
        private static GameClock instance;

        public static double CurrentTime => getInstance()?.Clock?.CurrentTime ?? 0;
        public static double FrameTime => getInstance()?.Clock?.ElapsedFrameTime ?? 0;

        public GameClock()
        {
            Logger.Log("Creating GameClock");
        }

        private static GameClock getInstance()
        {
            Logger.Log("fetching GameClock");

            if (instance == null)
            {
                throw new InvalidOperationException("GameClock has not been initialized in the drawable hierarchy.");
            }

            return instance;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Logger.Log($"GameClock loaded at {CurrentTime}", LoggingTarget.Runtime, LogLevel.Verbose);
            instance = this;
        }

        /// <summary>
        /// Queue a method for activation.
        /// </summary>
        /// <param name="method">the method</param>
        /// <param name="delay">how long until activation</param>
        public void QueueMethod(Action method, double delay = 0) => Scheduler.AddDelayed(method, delay);
    }
}
