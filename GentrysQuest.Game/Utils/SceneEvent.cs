using System;
using JetBrains.Annotations;

namespace GentrysQuest.Game.Utils
{
    public class SceneEvent
    {
        /// <summary>
        /// Provide this if you'd like to add dialogue.
        /// </summary>
        [CanBeNull]
        public DialogueEvent? DialogueEvent { get; set; }

        /// <summary>
        /// If you'd like to add some delay.
        /// </summary>
        public double Delay { get; set; } = 0;

        /// <summary>
        /// If you'd like to set a custom duration.
        /// </summary>
        public double Duration { get; set; } = 0;

        /// <summary>
        /// Custom event if you'd need one.
        /// </summary>
        [CanBeNull]
        public Delegate Event = null;
    }
}
