using GentrysQuest.Game.Overlays.Notifications;
using GentrysQuest.Game.Screens.Gameplay;
using GentrysQuest.Game.Utils;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace GentrysQuest.Game.Entity.Drawables;

/// <summary>
/// The playable version of the drawable entity
/// </summary>
public partial class DrawablePlayableEntity : DrawableEntity
{
    /// <summary>
    /// A container that manages mouse clicks
    /// Since it's a playable entity you should be able to click
    /// </summary>
    private GameplayClickContainer clickContainer;

    // Movement information
    private bool up;
    private bool down;
    private bool left;
    private bool right;

    public DrawablePlayableEntity(Character entity)
        : base(entity, AffiliationType.Player, false)
    {
        entity.OnLevelUp += delegate { NotificationContainer.Instance.AddNotification(new Notification("Leveled up!", NotificationType.Informative)); };
        if (entity.Secondary != null) entity.Secondary.User = this;
        if (entity.Utility != null) entity.Utility.User = this;
        if (entity.Ultimate != null) entity.Ultimate.User = this;
    }

    public void SetupClickContainer() => AddInternal(clickContainer = new GameplayClickContainer(this)); // Add clickable container to the player scene

    public void RemoveClickContainer()
    {
        RemoveInternal(clickContainer, true);
        clickContainer.Dispose();
        clickContainer = null;
    }

    protected override bool OnKeyDown(KeyDownEvent e)
    {
        switch (e.Key)
        {
            case Key.A:
                left = true;
                break;

            case Key.D:
                right = true;
                break;

            case Key.W:
                up = true;
                break;

            case Key.S:
                down = true;
                break;

            case Key.ShiftLeft:
                Dodge();
                break;

            case Key.Space:
                if (Entity.Utility?.PercentToDone == 100 || Entity.Utility?.UsesAvailable > 0)
                {
                    Entity.Utility?.Act();
                    if (Entity.Utility != null) Entity.Utility.LastUseTime = Clock.CurrentTime;
                }

                break;

            case Key.R:
                if (Entity.Ultimate?.PercentToDone == 100 || Entity.Ultimate?.UsesAvailable > 0)
                {
                    Entity.Ultimate?.Act();
                    if (Entity.Ultimate != null) Entity.Ultimate.LastUseTime = Clock.CurrentTime;
                }

                break;
        }

        return base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyUpEvent e)
    {
        switch (e.Key)
        {
            case Key.A:
                left = false;
                break;

            case Key.D:
                right = false;
                break;

            case Key.W:
                up = false;
                break;

            case Key.S:
                down = false;
                break;
        }

        base.OnKeyUp(e);
    }

    protected override void Update()
    {
        base.Update();

        if (Entity.CanMove)
        {
            if (left) Direction += MathBase.GetAngleToVector(180);
            if (right) Direction += MathBase.GetAngleToVector(0);
            if (up) Direction += MathBase.GetAngleToVector(270);
            if (down) Direction += MathBase.GetAngleToVector(90);
        }

        if (Direction != Vector2.Zero) Move(Direction.Normalized(), GetSpeed());
    }
}
