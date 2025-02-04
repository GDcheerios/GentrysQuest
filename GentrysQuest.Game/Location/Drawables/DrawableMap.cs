using System.Collections.Generic;
using System.Linq;
using GentrysQuest.Game.Entity;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Location.Drawables
{
    public sealed partial class DrawableMap : CompositeDrawable
    {
        public Map MapReference { get; private set; }
        public List<DrawableMapObject> Objects { get; private set; }

        public DrawableMap()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Size = new Vector2(5000);
        }

        public void Load(Map map)
        {
            MapReference = map;
            Objects = new();

            map.Load();

            foreach (var newMapObject in map.Objects.Select(mapObject => new DrawableMapObject(mapObject)))
            {
                Objects.Add(newMapObject);
                AddInternal(newMapObject);
            }
        }

        public void Unload()
        {
            foreach (DrawableMapObject mapObject in Objects)
            {
                HitBoxScene.Remove(mapObject.Collider);
                RemoveInternal(mapObject, true);
            }
        }
    }
}
