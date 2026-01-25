using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace GentrysQuest.Game.Overlays.GameMenu.GachaTab
{
    public partial class ItemShowcaseContainer(List<EntityBase> entities) : Container
    {
        private readonly EntityInfoListContainer entityInfoListContainer = new()
        {
            RelativeSizeAxes = Axes.Both
        };

        private double lastInput;
        private bool isReversed;
        private const float scroll_speed = 0.5f;
        private const float interval = 3000;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Child = entityInfoListContainer;
            _ = entityInfoListContainer.AddFromList(entities);
            entityInfoListContainer.Sort("Name", false);
            entityInfoListContainer.Sort("Star Rating", false);
        }

        protected override void OnDrag(DragEvent e)
        {
            base.OnDrag(e);
            lastInput = Time.Current;
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            lastInput = Time.Current;
            return base.OnScroll(e);
        }

        protected override void Update()
        {
            base.Update();

            // if (!(Time.Current - lastInput > interval)) return;
            //
            // BasicScrollContainer scrollContainer = entityInfoListContainer.GetScrollContainer();
            // scrollContainer.Y += isReversed ? scroll_speed : -scroll_speed * (float)Time.Elapsed;
            //
            // if (scrollContainer.Y <= 0 || scrollContainer.Y >= scrollContainer.Height)
            // {
            //     isReversed = !isReversed;
            // }
        }
    }
}
