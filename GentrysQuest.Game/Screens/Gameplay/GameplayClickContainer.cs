using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace GentrysQuest.Game.Screens.Gameplay;

/// <summary>
/// A clickable container to determine where the player is looking and where they click.
/// </summary>
/// <param name="player">The player to follow</param>
public partial class GameplayClickContainer(DrawablePlayableEntity player) : Container
{
    private double holdStart;
    private bool isHeld;
    private Vector2 mousePos;
    private const int HOLD_TIME = 300;

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
                holdStart = Clock.CurrentTime;
                isHeld = true;
                break;

            case MouseButton.Right:
                if (player.GetBase().Secondary?.PercentToDone == 100 || player.GetBase().Secondary?.UsesAvailable > 0)
                {
                    player.GetBase().Secondary?.Act();
                    player.GetBase().Secondary.LastUseTime = Clock.CurrentTime;
                }

                break;
        }

        return base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseUpEvent e)
    {
        switch (e.Button)
        {
            case MouseButton.Left:
                holdStart = 0;
                isHeld = false;
                break;
        }

        base.OnMouseUp(e);
    }

    protected override bool OnClick(ClickEvent e)
    {
        player.Attack(mousePos);
        return base.OnClick(e);
    }

    protected override bool OnMouseMove(MouseMoveEvent e)
    {
        mousePos = e.MousePosition;
        return base.OnMouseMove(e);
    }

    protected override void Update()
    {
        base.Update();

        if (isHeld && new ElapsedTime(Clock.CurrentTime, holdStart) > HOLD_TIME)
        {
            // Todo: implement hold logic
        }

        player.DirectionLooking = (int)MathBase.GetAngle(player.Position + new Vector2(50), mousePos);
    }
}
