using System;
using osu.Framework.Graphics;
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
        /// The color of the object
        /// </summary>
        Colour4 Colour { get; set; }

        /// <summary>
        /// Event handler for when touched
        /// </summary>
        EventHandler OnTouchEvent { get; set; }

        /// <summary>
        /// Anchor
        /// </summary>
        Anchor Anchor { get; set; }

        /// <summary>
        /// Origin
        /// </summary>
        Anchor Origin { get; set; }

        /// <summary>
        /// Relative sizing axes
        /// </summary>
        Axes RelativeSizeAxes { get; set; }

        /// <summary>
        /// Relative positioning axes
        /// </summary>
        Axes RelativePositionAxes { get; set; }

        /// <summary>
        /// Ran when touched
        /// </summary>
        void OnTouch() => OnTouchEvent?.Invoke(this, EventArgs.Empty);
    }
}
