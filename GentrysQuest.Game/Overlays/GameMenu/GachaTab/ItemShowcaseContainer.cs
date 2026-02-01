using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace GentrysQuest.Game.Overlays.GameMenu.GachaTab
{
    public partial class ItemShowcaseContainer(List<EntityBase> entities) : Container
    {
        private readonly EntityInfoListContainer entityInfoListContainer = new()
        {
            RelativeSizeAxes = Axes.Both
        };

        private double lastInputTime;
        private double lastEdgeHitTime;
        private bool towardsEnd = true;

        private const double pause_after_input_ms = 3000;
        private const double pause_on_edge_ms = 2000;
        private const float auto_scroll_speed = 3500;
        private const float extra_padding = 50;

        [BackgroundDependencyLoader]
        private void load()
        {
            Name = "Item Showcase Container";
            RelativeSizeAxes = Axes.Both;
            Child = entityInfoListContainer;
            _ = entityInfoListContainer.AddFromList(entities);
            entityInfoListContainer.GetScrollContainer().ScrollbarVisible = false;
            entityInfoListContainer.GetEntityInfoDrawables().ForEach(drawable =>
            {
                drawable.Level.Hide();
                drawable.BuffContainer.Hide();

                drawable.EdgeFadeStart = 80f;
                drawable.MinAlphaAwayFromCentre = 0.01f;
                drawable.MinScaleAwayFromCentre = 0.01f;

                drawable.Width = 1;
                drawable.NameText.Scale = new Vector2(0.8f);
                drawable.StarRatingContainer.Scale = new Vector2(0.8f);
                drawable.Height = 80;

                switch (drawable)
                {
                    case CharacterInfoDrawable characterDrawable:
                        characterDrawable.EquippedItemContainer.Hide();
                        break;

                    case ArtifactInfoDrawable artifactDrawable:
                        artifactDrawable.BuffContainer.Hide();
                        break;

                    case WeaponInfoDrawable weaponDrawable:
                        weaponDrawable.BuffContainer.Hide();
                        break;
                }
            });
            entityInfoListContainer.Sort("Name", false);
            entityInfoListContainer.Sort("Star Rating", false);
            entityInfoListContainer.AddPadding((int)extra_padding);
        }

        private void registerInput()
        {
            lastInputTime = Time.Current;
        }

        protected override void OnDrag(DragEvent e)
        {
            base.OnDrag(e);
            registerInput();
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            registerInput();
            return base.OnScroll(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            registerInput();
            return base.OnMouseDown(e);
        }

        protected override void Update()
        {
            base.Update();

            if (Time.Current - lastInputTime < pause_after_input_ms)
                return;

            if (Time.Current - lastEdgeHitTime < pause_on_edge_ms)
                return;

            BasicScrollContainer scrollContainer = entityInfoListContainer.GetScrollContainer();

            const double min = 0;
            double max = scrollContainer.ScrollableExtent;

            double dt = Time.Elapsed / 1000.0;
            double direction = towardsEnd ? 1 : -1;

            double target = scrollContainer.Current + direction * auto_scroll_speed * dt;

            if (target <= min)
            {
                scrollContainer.ScrollTo(min);
                towardsEnd = true;
                lastEdgeHitTime = Time.Current;
                return;
            }

            if (target >= max)
            {
                scrollContainer.ScrollTo(max);
                towardsEnd = false;
                lastEdgeHitTime = Time.Current;
                return;
            }

            scrollContainer.ScrollTo(target);
        }
    }
}
