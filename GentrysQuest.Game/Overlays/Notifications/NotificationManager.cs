using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Game.Overlays.Notifications;

public partial class NotificationManager : CompositeDrawable
{
    private readonly FillFlowContainer notifications;

    public NotificationManager()
    {
        // RelativePositionAxes = Axes.Both;
        RelativeSizeAxes = Axes.Both;
        Origin = Anchor.TopRight;
        Anchor = Anchor.TopRight;
        Depth = -2;
        InternalChildren = new Drawable[]
        {
            notifications = new FillFlowContainer()
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Both
            }
        };
    }

    /// <summary>
    /// Adds a notification to the container
    /// </summary>
    /// <param name="notification">The notification</param>
    public void AddNotification(Notification notification)
    {
        int length = notification.Message.Length * 150;
        notification.ScaleTo(0, 1);
        notifications.Add(notification);
        notification.ScaleTo(1, 100);
        Scheduler.AddDelayed(() => { notification.FadeOut(100); }, length);
        Scheduler.AddDelayed(() =>
        {
            notifications.Remove(notification, false);
        }, length + 100);
    }
}
