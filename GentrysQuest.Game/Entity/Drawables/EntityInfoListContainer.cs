using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GentrysQuest.Game.Graphics;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class EntityInfoListContainer : Container
    {
        private readonly BasicScrollContainer scrollContainer;
        private readonly List<EntityInfoDrawable> entityReferences;
        private bool queued = false;
        private readonly List<EntityBase> queuedEntities = new();
        private const int DURATION = 150;
        private const int DELAY = 50;
        private const int DELAY_ITEM_LIMIT = 7;
        private const int DELAY_MAX = DELAY * DELAY_ITEM_LIMIT + 1;
        private const int SORT_DURATION = 100;
        private readonly SpriteText noItemsDisclaimer;
        private readonly LoadingIndicator loadingIndicator;
        public event EventHandler FinishedLoading;

        public Bindable<int> Spacing { get; } = new Bindable<int>(20);

        public EntityInfoListContainer()
        {
            Name = "Entity Info List";
            RelativeSizeAxes = Axes.Both;
            Origin = Anchor.TopCentre;
            Anchor = Anchor.TopCentre;
            Padding = new MarginPadding(3f);
            Children =
            [
                scrollContainer = new BasicScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1, 0.9f),
                    Padding = new MarginPadding { Bottom = 10 },
                },
                noItemsDisclaimer = new SpriteText
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Y = 20,
                    Text = "This list is empty...",
                    Font = new FontUsage().With(size: 48),
                    Colour = Colour4.White
                },
                loadingIndicator = new LoadingIndicator
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre
                }
            ];
            loadingIndicator.FadeOut(0);
            entityReferences = new();
            Spacing.ValueChanged += _ => RepositionItems();
        }

        private Task addToList(EntityInfoDrawable drawable)
        {
            drawable.Y = drawable.Height * scrollContainer.Count;

            drawable.GetViewportScreenSpaceRect = () =>
            {
                var topLeft = scrollContainer.ToScreenSpace(scrollContainer.DrawRectangle.TopLeft);
                var bottomRight = scrollContainer.ToScreenSpace(scrollContainer.DrawRectangle.BottomRight);

                return new RectangleF(
                    topLeft.X,
                    topLeft.Y,
                    bottomRight.X - topLeft.X,
                    bottomRight.Y - topLeft.Y
                );
            };

            scrollContainer.Add(drawable);
            entityReferences.Add(drawable);
            return Task.CompletedTask;
        }

        public List<EntityInfoDrawable> GetEntityInfoDrawables() => entityReferences;

        private async Task addEntity(EntityBase entity, int delay = 0, bool hidden = false)
        {
            EntityInfoDrawable entityInfoDrawable;

            switch (entity)
            {
                case Character character:
                    entityInfoDrawable = new CharacterInfoDrawable(character);
                    break;

                case Artifact artifact:
                    entityInfoDrawable = new ArtifactInfoDrawable(artifact);
                    break;

                case Weapon.Weapon weapon:
                    entityInfoDrawable = new WeaponInfoDrawable(weapon);
                    break;

                default:
                    entityInfoDrawable = new EntityInfoDrawable(entity);
                    break;
            }

            await addToList(entityInfoDrawable);
            if (hidden) entityInfoDrawable.FadeOut().Then().ScaleTo(new Vector2(1, 0.001f));
        }

        public async Task AddFromList<T>(List<T> entityList) where T : EntityBase
        {
            if (entityList.Count == 0) noItemsDisclaimer.FadeIn(DURATION);
            else noItemsDisclaimer.FadeOut(DURATION);
            for (int i = 0; i < entityList.Count; i++) await addEntity(entityList[i]);
        }

        public void AddPadding(int padding) => scrollContainer.Padding = new MarginPadding { Bottom = padding, Top = padding };

        private async Task drawableFadeOut(EntityInfoDrawable drawable)
        {
            drawable.FadeOut(DELAY);
            await Task.Delay(DELAY);
        }

        private Task removeDrawable(EntityInfoDrawable drawable) => Task.FromResult(scrollContainer.Remove(drawable, false));

        public async Task ClearList()
        {
            int index = 0;

            foreach (EntityInfoDrawable entityInfoDrawable in entityReferences.ToList())
            {
                entityReferences.Remove(entityInfoDrawable);
                if (index < DELAY_ITEM_LIMIT)
                    await drawableFadeOut(entityInfoDrawable);
                await removeDrawable(entityInfoDrawable);
                index++;
            }
        }

        public async Task RemoveFromList(EntityInfoDrawable drawable) => await drawableFadeOut(drawable);

        protected override void Update()
        {
            if (!queued && queuedEntities.Count > 0)
            {
                AddFromList(queuedEntities);
                queuedEntities.Clear();
                FinishedLoading?.Invoke(null, null);
            }

            base.Update();
        }

        public int GetDelayLimit() => DELAY_ITEM_LIMIT;
        public int GetSortDuration() => SORT_DURATION;
        public int GetDelay() => DELAY;

        public void ScrollToTop() => scrollContainer.ScrollToStart();

        public void ScrollToItem(int index)
        {
            if (index < 3 || index > entityReferences.Count - 4) return;

            var targetY = (index - 3) * 110;
            scrollContainer.ScrollTo(targetY);
        }

        public BasicScrollContainer GetScrollContainer() => scrollContainer;

        public void RepositionItems()
        {
            for (int i = 0; i < entityReferences.Count; i++)
            {
                entityReferences[i].MoveToY(i * entityReferences[i].Height + Spacing.Value, SORT_DURATION, Easing.InOutCirc);
            }
        }

        public void Sort(string condition, bool reversed)
        {
            List<dynamic[]> newList = (
                from EntityInfoDrawable entityInfoDrawable in scrollContainer.Children
                select new dynamic[]
                {
                    entityInfoDrawable.entity, entityInfoDrawable
                }
            ).ToList();

            switch (condition)
            {
                case "Star Rating":
                    newList.Sort((x, y) => x[0].StarRating.Value.CompareTo(y[0].StarRating.Value));
                    break;

                case "Name":
                    newList.Sort((x, y) => string.Compare(x[0].Name, y[0].Name));
                    break;

                case "Level":
                    newList.Sort((x, y) => x[0].Experience.Level.Current.Value.CompareTo(y[0].Experience.Level.Current.Value));
                    break;
            }

            if (!reversed) newList.Reverse();

            int yPos = 0;

            foreach (var pair in newList)
            {
                EntityInfoDrawable entityInfoDrawable = pair[1];
                entityInfoDrawable.MoveToY(yPos, SORT_DURATION, Easing.InOutCirc);
                yPos += (int)entityInfoDrawable.Height + Spacing.Value;
            }

            for (int i = 0; i < newList.Count; i++)
            {
                entityReferences[i] = newList[i][1];
            }
        }
    }
}
