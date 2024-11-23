namespace GentrysQuest.Game.Input
{
    public class HoldEvent
    {
        /// <summary>
        /// How long the hold has been for.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// If the mouse is currently pressed down.
        /// </summary>
        public bool IsPressed { get; set; }

        /// <summary>
        /// Check if user has been holding for a certain amount of time.
        /// Defaults to 0 in case adding repetitive attacking.
        /// </summary>
        /// <param name="timeToHold">How long to check</param>
        /// <returns>If the user has been holding long enough</returns>
        public bool MeetsHoldCondition(double timeToHold = 0) => timeToHold >= Duration;
    }
}
