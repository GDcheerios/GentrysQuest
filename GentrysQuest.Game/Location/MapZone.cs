using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Location
{
    public delegate void TouchEventHandler(DrawableEntity entity);

    public partial class MapZone : MapObject
    {
        public event TouchEventHandler OnTouched;

        public bool Flashes { get; set; }

        public MapZone()
        {
            Reactive = true;
            HasCollider = false;
        }

        protected virtual void OnTouch(DrawableEntity entity) => OnTouched?.Invoke(entity);

        [BackgroundDependencyLoader]
        private void load()
        {
            if (Flashes) this.FlashColour(Colour4.White, 500).Loop();
        }

        protected override void Update()
        {
            base.Update();
            List<HitBox> intersections = HitBoxScene.GetIntersections(IntersectingHitBox);

            foreach (HitBox hitBox in intersections)
            {
                if (hitBox.GetParent() is DrawableEntity entity) OnTouch(entity);
            }
        }
    }
}
