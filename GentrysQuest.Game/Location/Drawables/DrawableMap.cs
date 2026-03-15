using System;
using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Location.Drawables
{
    public sealed partial class DrawableMap : CompositeDrawable
    {
        public Map Reference { get; private set; }
        public List<MapObject> Objects { get; private set; }
        public List<DrawableEntity> Npcs { get; private set; } = new();

        public Action<Vector2> OnMove { get; set; }

        public DrawableMap()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.TopLeft;
        }

        public void RemoveNpc(DrawableEntity entity)
        {
            Npcs.Remove(entity);
            RemoveInternal(entity, true);
        }

        public void Move(Vector2 vector)
        {
            OnMove?.Invoke(vector);
            Position += vector;
        }

        public void Load(Map map)
        {
            Reference = map;
            Reference.SetDrawable(this);
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

#if DEBUG
            AddInternal(new MapObject
            {
                Name = "Spawn Point",
                Position = Reference.SpawnPoint,
                Size = new Vector2(150),
                Alpha = 0.5f,
                Colour = Colour4.LightBlue,
                Origin = Anchor.Centre
            });
#endif
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
    }
}
