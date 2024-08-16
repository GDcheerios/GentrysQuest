using System;
using System.Collections.Generic;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class DrawableEnemyEntity : DrawableEntity
    {
        private DrawableEntity followEntity;
        private EnemyController enemyController;
        private Box directionTrack;
        private Vector2 lastPosition;
        private int stuckCounter = 0;
        private const int stuckThreshold = 1; // Number of frames to consider as "stuck"
        private const float stuckDistanceThreshold = 1f; // Minimum movement to consider as not stuck

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
            AddInternal(directionTrack = new Box
            {
                Colour = Colour4.Yellow,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(0.1f, 1),
                Origin = Anchor.BottomCentre,
#if DEBUG
                Alpha = 0.5f,
#else
                Alpha = 0f,
#endif
                Anchor = Anchor.Centre
            });
        }

        public void FollowEntity(DrawableEntity drawableEntity) => followEntity = drawableEntity;

        private Vector2 getDesirableDirection()
        {
            float totalWeight = 0f;
            Vector2 desirableDirection = Vector2.Zero;

            Vector2 playerDirection = -getDirectionToPlayer().Normalized();

            // Weight factor to balance obstacle avoidance and player pursuit
            const float AVOIDANCE_WEIGHT = 0.7f;
            const float PLAYER_WEIGHT = 0.3f;

            foreach (KeyValuePair<int, bool> angle in enemyController.GetIntersectedAngles())
            {
                Vector2 directionVector = MathBase.GetAngleToVector((float)angle.Key).Normalized();
                float dotProduct = Vector2.Dot(playerDirection, directionVector);

                // Adjust the weight based on whether this direction is obstructed
                float weight = angle.Value ? AVOIDANCE_WEIGHT : -AVOIDANCE_WEIGHT;

                desirableDirection += weight * dotProduct * directionVector;
                totalWeight += Math.Abs(dotProduct) * AVOIDANCE_WEIGHT;
            }

            // Add some influence of the direct player direction regardless of obstacles
            desirableDirection += playerDirection * PLAYER_WEIGHT;
            totalWeight += PLAYER_WEIGHT;

            if (totalWeight == 0)
                return playerDirection;

            desirableDirection /= totalWeight;

            Vector2 currentPosition = Position;

            // Check if the enemy is stuck
            if ((currentPosition - lastPosition).Length < stuckDistanceThreshold)
            {
                stuckCounter++;
            }
            else
            {
                stuckCounter = 0; // Reset if the enemy moved
            }

            lastPosition = currentPosition;

            // If stuck, encourage the enemy to move in a different direction
            if (stuckCounter >= stuckThreshold)
            {
                stuckCounter = 0; // Reset the counter
                // desirableDirection += getUnstuckDirection();
            }

            return desirableDirection.Normalized();
        }

        private Vector2 getUnstuckDirection()
        {
            // Implement a simple random or predefined direction to help the enemy get unstuck.
            // You can expand this to include more sophisticated logic, like favoring directions with no obstacles.
            Random random = new Random();
            float randomAngle = (float)(random.NextDouble() * Math.PI * 2); // Random angle in radians
            return new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle)).Normalized();
        }

        private Vector2 getDirectionToPlayer() => MathBase.GetDirection(Position, followEntity.Position);

        protected override void Update()
        {
            base.Update();

            if (followEntity == null) return;

            if (GetBase().Weapon != null && MathBase.GetDistance(Position, followEntity.Position) < GetBase().Weapon!.Distance) Attack(followEntity.Position);

            DirectionLooking = (int)MathBase.GetAngle(Position, followEntity.Position);

            if (Entity.CanMove) Direction += getDesirableDirection();
            // if (Entity.CanMove) Direction += getDirectionToPlayer();
            if (Direction != Vector2.Zero) Move(Direction.Normalized(), GetSpeed());
            float rotation = MathBase.GetAngle(Vector2.Zero, Direction) + 90;
            if (!float.IsNaN(rotation)) directionTrack.Rotation = rotation;
        }
    }
}
