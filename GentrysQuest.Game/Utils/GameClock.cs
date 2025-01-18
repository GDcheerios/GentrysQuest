using System;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;

namespace GentrysQuest.Game.Utils
{
    public partial class GameClock : CompositeDrawable
    {
        private static IFrameBasedClock clock;

        public static double CurrentTime => clock?.CurrentTime ?? 0;
        public static double FrameTime => clock?.ElapsedFrameTime ?? 0;

        public GameClock(IFrameBasedClock clock) => GameClock.clock = clock;

        /// <summary>
        /// Queue a method for activation.
        /// </summary>
        /// <param name="method">the method</param>
        /// <param name="delay">how long until activation</param>
        public void QueueMethod(Action method, double delay = 0) => Scheduler.AddDelayed(method, delay);
    }
}
