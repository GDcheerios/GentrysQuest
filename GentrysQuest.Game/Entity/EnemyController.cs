using System.Collections.Generic;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Utils;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Entity
{
    public partial class EnemyController : CompositeDrawable
    {
        private DrawableEntity parent;
        private MovementHitBox[] directionChecks;
        private const int DIRECTIONS = 8;
        private const float DISTANCE = 0.5f;

        public EnemyController(DrawableEnemyEntity enemy)
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            parent = enemy;

            directionChecks = new MovementHitBox[DIRECTIONS];

            for (int i = 0; i < DIRECTIONS; i++)
            {
                var rotation = i * (360 / DIRECTIONS);
                directionChecks[i] = new MovementHitBox(enemy)
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.05f),
                    Rotation = rotation,
                    Position = MathBase.GetAngleToVector(rotation) * DISTANCE
                };
                AddInternal(directionChecks[i]);
            }
        }

        public Dictionary<int, bool> GetIntersectedAngles()
        {
            Dictionary<int, bool> points = new Dictionary<int, bool>();

            for (int i = 0; i < DIRECTIONS; i++)
            {
                if (HitBoxScene.Collides(directionChecks[i])) points.Add(i * (360 / DIRECTIONS), true);
                else points.Add(i * (360 / DIRECTIONS), false);
            }

            return points;
        }
    }
}
