using System;
using System.Collections.Generic;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class DrawableEnemyEntity : DrawableEntity
    {
        private DrawableEntity followEntity;
        private EnemyController enemyController;
        private Box directionTrack;

        public DrawableEnemyEntity(Entity entity)
            : base(entity, AffiliationType.Enemy)
        {
            OnMove += delegate(Vector2 direction, double speed)
            {
                Position += direction * (float)Clock.ElapsedFrameTime * (float)speed;
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(enemyController = new EnemyController(this));
#if DEBUG
            AddInternal(directionTrack = new Box
            {
                Colour = Colour4.Yellow,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.1f, 1),
                Origin = Anchor.BottomCentre,
                Alpha = 0.5f,
                Anchor = Anchor.Centre
            });
#endif
        }

        public void FollowEntity(DrawableEntity drawableEntity) => followEntity = drawableEntity;

        private Vector2 getDesirableDirection()
        {
            float totalWeight = 0f;
            Vector2 desirableDirection = Vector2.Zero;

            Vector2 playerDirection = -getDirectionToPlayer().Normalized();

            foreach (KeyValuePair<int, bool> angle in enemyController.GetIntersectedAngles())
            {
                Vector2 directionVector = MathBase.GetAngleToVector((float)angle.Key).Normalized();
                float dotProduct = Vector2.Dot(playerDirection, directionVector);
                float weight = angle.Value ? 1f : -1f;

                desirableDirection += weight * dotProduct * directionVector;
                totalWeight += Math.Abs(dotProduct);
            }

            Logger.Log(totalWeight.ToString());

            if (totalWeight == 0)
                return playerDirection;

            desirableDirection /= totalWeight;

            return desirableDirection;
        }

        private Vector2 getDirectionToPlayer() => MathBase.GetDirection(Position, followEntity.Position);

        protected override void Update()
        {
            base.Update();

            if (followEntity == null) return;

            if (GetBase().Weapon != null && MathBase.GetDistance(Position, followEntity.Position) < GetBase().Weapon!.Distance) Attack(followEntity.Position);

            DirectionLooking = (int)MathBase.GetAngle(Position, followEntity.Position);

            if (Entity.CanMove) Direction += getDesirableDirection();
            if (Direction != Vector2.Zero) Move(Direction.Normalized(), GetSpeed());
            float rotation = MathBase.GetAngle(Vector2.Zero, Direction) + 90;
            if (!float.IsNaN(rotation)) directionTrack.Rotation = rotation;
        }
    }
}
