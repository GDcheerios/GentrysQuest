using GentrysQuest.Game.Input;
using GentrysQuest.Game.Overlays.Notifications;
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
    private const double FLASH_INTERVAL = 100;
    private const float FLASH_LOW_ALPHA = 0.35f;

    /// <summary>
    /// A container that manages mouse clicks
    /// Since it's a playable entity you should be able to click
    /// </summary>
    private ClickContainer clickContainer;

    // Movement information
    private bool up;
    private bool down;
    private bool left;
    private bool right;

    private double invincibilityFlashStartTime;
    private double invincibilityFlashEndTime;

    public DrawablePlayableEntity(Character entity)
        : base(entity, AffiliationType.Player, false)
    {
        entity.OnLevelUp += delegate { Notification.Create("Leveled up!", NotificationType.Informative); };
        entity.OnDamage += delegate
        {
            invincibilityFlashStartTime = Clock.CurrentTime;
            invincibilityFlashEndTime = invincibilityFlashStartTime + Character.INVINCIBILITY_TIME;
        };
        if (entity.Secondary != null) entity.Secondary.User = this;
        if (entity.Utility != null) entity.Utility.User = this;
        if (entity.Ultimate != null) entity.Ultimate.User = this;
    }

    public void SetupClickContainer() => AddInternal(clickContainer = new ClickContainer(this)); // Add clickable container to the player scene

    public void RemoveClickContainer()
    {
        if (clickContainer == null) return;
        RemoveInternal(clickContainer, true);
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
                Entity.Utility?.Act();
                break;

            case Key.R:
                Entity.Ultimate?.Act();
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

        if (Clock.CurrentTime < invincibilityFlashEndTime)
        {
            int flashIndex = (int)((Clock.CurrentTime - invincibilityFlashStartTime) / FLASH_INTERVAL);
            Alpha = flashIndex % 2 == 0 ? FLASH_LOW_ALPHA : 1;
            return;
        }

        if (Alpha != 1)
            Alpha = 1;
    }
}
