using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Audio;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace GentrysQuest.Game.Overlays.GameMenu.GachaTab;

public partial class GachaRollContainer : Container
{
    private readonly Container viewport;
    private readonly FillFlowContainer itemDrawableContainer;
    private readonly List<EntityBase> items;
    private readonly EntityBase pulledItem;

    private ISampleStore samples;

    private float lastMarkerContentX;
    private bool trackingTicks;

    private const ushort MaxItems = 80;
    private const double RollDuration = 8000;
    private const double RollStartDelay = 1000;
    private const float ItemSpacing = 25f;
    private const float MarkerWidth = 4f;
    private const int ItemsBeforePulled = 5;
    private const int ItemsAfterPulled = 10;
    private const float fallbackItemWidth = 100f;

    public GachaRollContainer(List<EntityBase> items, EntityBase pulledItem)
    {
        this.items = items;
        this.pulledItem = pulledItem;

        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        viewport = new Container
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Masking = true,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        };

        itemDrawableContainer = new FillFlowContainer
        {
            AutoSizeAxes = Axes.Both,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            Direction = FillDirection.Horizontal,
            Spacing = new Vector2(ItemSpacing, 0),
        };

        viewport.Child = itemDrawableContainer;

        InternalChildren =
        [
            viewport,
            new Box
            {
                RelativeSizeAxes = Axes.Y,
                Width = MarkerWidth,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = Color4.Gold,
                Alpha = 0.95f,
            }
        ];

        for (int i = 0; i < MaxItems - ItemsAfterPulled - 1; i++)
            addRandomItem();

        itemDrawableContainer.Add(
            new GachaRollItemIcon(
                pulledItem.TextureMapping != null ? pulledItem.TextureMapping.Get("Icon") : "Icon",
                pulledItem.StarRating
            )
        );

        for (int i = 0; i < ItemsAfterPulled; i++)
            addRandomItem();
    }

    private void addRandomItem()
    {
        int starRating = MathBase.RandomGachaStarRating();
        List<EntityBase> validItems = items.Where(item => item.StarRating == starRating).ToList();
        if (validItems.Count == 0)
            validItems = items;

        EntityBase item = validItems[MathBase.RandomChoice(validItems.Count)];
        string texture = item.TextureMapping != null ? item.TextureMapping.Get("Icon") : "Icon";

        itemDrawableContainer.Add(new GachaRollItemIcon(texture, item.StarRating));
    }

    [BackgroundDependencyLoader]
    private void load(ISampleStore sampleStore)
    {
        samples = sampleStore;
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        Scheduler.AddOnce(startRoll);
    }

    private void startRoll()
    {
        int pulledItemIndex = MaxItems - ItemsAfterPulled - 1;

        if (pulledItemIndex < 0 || pulledItemIndex >= itemDrawableContainer.Count)
            return;

        Drawable pulledDrawable = itemDrawableContainer[pulledItemIndex];
        float itemWidth = pulledDrawable.DrawWidth > 0 ? pulledDrawable.DrawWidth : fallbackItemWidth;

        if (DrawWidth <= 0)
        {
            Scheduler.AddDelayed(startRoll, 50);
            return;
        }

        float itemStride = itemWidth + itemDrawableContainer.Spacing.X;
        float startCentreX = ItemsBeforePulled * itemStride + itemWidth / 2f;
        float pulledCentreX = pulledItemIndex * itemStride + itemWidth / 2f;

        float targetX = DrawWidth / 2f - pulledCentreX;
        float startX = DrawWidth / 2f - startCentreX;

        itemDrawableContainer.X = startX;
        lastMarkerContentX = getMarkerContentX();

        Scheduler.AddDelayed(() =>
        {
            trackingTicks = true;
            lastMarkerContentX = getMarkerContentX();

            itemDrawableContainer.MoveToX(targetX, RollDuration, Easing.OutQuint);

            Scheduler.AddDelayed(() => trackingTicks = false, RollDuration);
        }, RollStartDelay);
    }

    protected override void Update()
    {
        base.Update();

        if (!trackingTicks)
            return;

        float currentMarkerContentX = getMarkerContentX();

        if (currentMarkerContentX <= lastMarkerContentX)
        {
            lastMarkerContentX = currentMarkerContentX;
            return;
        }

        bool playedTick = false;

        foreach (Drawable child in itemDrawableContainer)
        {
            float childCentreX = child.X + child.DrawWidth / 2f;

            if (childCentreX >= lastMarkerContentX && childCentreX < currentMarkerContentX)
            {
                playedTick = true;
                break;
            }
        }

        if (playedTick)
            AudioManager.Instance.PlaySound(new DrawableSample(samples.Get("sounds_menu_dClick.mp3")));

        lastMarkerContentX = currentMarkerContentX;
    }

    private float getMarkerContentX() => DrawWidth / 2f - itemDrawableContainer.X;
}
