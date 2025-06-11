using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Game.Location.Drawables
{
    public sealed partial class DrawableMap : CompositeDrawable
    {
        public Map MapReference { get; private set; }
        public List<MapObject> Objects { get; private set; }
        public List<DrawableEntity> Npcs { get; private set; } = new();

        public DrawableMap()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public void RemoveNpc(DrawableEntity entity)
        {
            Npcs.Remove(entity);
            RemoveInternal(entity, true);
        }

        public void Load(Map map)
        {
            MapReference = map;
            Objects = new();

            map.Load();

            Size = map.Size * 2;

            foreach (var mapObject in map.Objects)
            {
                Objects.Add(mapObject);
                AddInternal(mapObject);
            }

            foreach (DrawableEntity entity in map.Npcs)
            {
                Npcs.Add(entity);
                AddInternal(entity);
            }
        }

        public void Unload()
        {
            foreach (MapObject mapObject in Objects)
            {
                HitBoxScene.Remove(mapObject.Collider);
                RemoveInternal(mapObject, true);
            }

            foreach (DrawableEntity entity in Npcs)
            {
                HitBoxScene.Remove(entity.HitBox);
                RemoveInternal(entity, true);
            }
        }

        protected override void Update()
        {
            base.Update();
            MapReference.Update();
        }
    }
}
