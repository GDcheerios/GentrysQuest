using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace GentrysQuest.Game.Input
{
    /// <summary>
    /// A clickable container to determine where the player is looking and where they click.
    /// </summary>
    /// <param name="player">The player to follow</param>
    public partial class ClickContainer(DrawablePlayableEntity player) : Container
    {
        /// <summary>
        /// When the player started holding down.
        /// We can use this to handle OnHold events.
        /// </summary>
        private double holdStart;

        /// <summary>
        /// If the left click is currently pressed down.
        /// </summary>
        private bool isPressed;

        /// <summary>
        /// The mouse position
        /// </summary>
        private Vector2 mousePos;

        /// <summary>
        /// How long should the mouse be pressed down
        /// to be considered as "holding".
        /// </summary>
        private const int HOLD_TIME = 300;

        private static readonly Vector2 PLAYER_OFFSET = new(50);

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Size = new Vector2(20f);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    player.DoAttack(mousePos);
                    break;

                case MouseButton.Right:
                    player.GetBase().Secondary?.Act();
                    break;
            }

            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    player.OnRelease();
                    break;
            }

            base.OnMouseUp(e);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            mousePos = e.MousePosition;
            return base.OnMouseMove(e);
        }

        protected override void Update()
        {
            base.Update();
            player.DirectionLooking = (int)MathBase.GetAngle(player.Position + PLAYER_OFFSET, mousePos);
        }
    }
}
