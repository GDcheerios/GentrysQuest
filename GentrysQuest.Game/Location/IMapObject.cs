using System;
using osuTK;

namespace GentrysQuest.Game.Location
{
    public interface IMapObject
    {
        /// <summary>
        /// The size of the object
        /// </summary>
        Vector2 Size { get; set; }

        /// <summary>
        /// Where the object is positioned
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Event handler for when touched
        /// </summary>
        EventHandler OnTouchEvent { get; set; }

        /// <summary>
        /// Ran when touched
        /// </summary>
        void OnTouch() => OnTouchEvent?.Invoke(this, EventArgs.Empty);
    }
}
