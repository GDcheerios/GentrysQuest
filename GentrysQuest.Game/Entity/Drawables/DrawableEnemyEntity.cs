using System;
using System.Collections.Generic;
using GentrysQuest.Game.Content.Effects;
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
        public EnemyController EnemyController;
        private Box directionTrack;
        private Vector2 lastPosition;
        private int stuckCounter = 0;
        private const int stuckThreshold = 1; // Number of frames to consider as "stuck"
        private const float stuckDistanceThreshold = 1f; // Minimum movement to consider as not stuck
        private VisibilityBox surroundingVisibility;
        private AiState aiState = AiState.Idle;

        /// <summary>
        /// The last position check to determine if we should set a new destination.
        /// </summary>
        private double lastPositionCheckTime;

        /// <summary>
        /// Determines how long until the enemy will find a new position to move to while Idle.
        /// </summary>
        private const int NEW_POSITION_INTERVAL = 5000;

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
            checkTime();
            AddInternal(surroundingVisibility = new VisibilityBox(this, true));
            AddInternal(EnemyController = new EnemyController(this));
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

            // Weight factor to balance obstacle avoidance and player pursuit
            const float avoidance_weight = 0.7f;
            const float player_weight = 0.3f;

            Vector2 goToPosition = -MathBase.GetDirection(Position, FocusedPosition);

            foreach (KeyValuePair<int, bool> angle in EnemyController.GetIntersectedAngles())
            {
                Vector2 directionVector = MathBase.GetAngleToVector((float)angle.Key).Normalized();
                float dotProduct = Vector2.Dot(goToPosition, directionVector);

                // Adjust the weight based on whether this direction is obstructed
                float weight = angle.Value ? avoidance_weight : -avoidance_weight;

                desirableDirection += weight * dotProduct * directionVector;
                totalWeight += Math.Abs(dotProduct) * avoidance_weight;
            }

            // Add some influence of the direct player direction regardless of obstacles
            desirableDirection += goToPosition * player_weight;
            totalWeight += player_weight;

            if (totalWeight == 0)
                return goToPosition;

            desirableDirection /= totalWeight;

            Vector2 currentPosition = Position;

            lastPosition = currentPosition;

            return desirableDirection.Normalized();
        }

        private Vector2 getUnstuckDirection()
        {
            const float random_angle = 90;
            return new Vector2((float)Math.Cos(random_angle), (float)Math.Sin(random_angle)).Normalized();
        }

        private bool canSeePlayer()
        {
            int x = 1;
            const int visibility_distance = 20;
            bool seenPlayer = false;
            VisibilityBox[] boxes = new VisibilityBox[visibility_distance];

            while (x < visibility_distance)
            {
                VisibilityBox box = new VisibilityBox(this)
                {
                    Position = MathBase.GetAngleToVector(MathBase.GetAngle(Position, Vector2.Zero)) * x
                };
                AddInternal(box);
                x++;
            }

            foreach (VisibilityBox box in boxes) RemoveInternal(box, true);
            return seenPlayer;
        }

        private Vector2 getDirectionToPlayer() => MathBase.GetDirection(Position, followEntity.Position);

        private bool checkTime() => Clock.CurrentTime - lastPositionCheckTime > NEW_POSITION_INTERVAL;
        private void updateTime() => lastPositionCheckTime = Clock.CurrentTime;
        private void updatePosition() => FocusedPosition = new Vector2(MathBase.RandomFloat(-1000, 1000), MathBase.RandomFloat(-1000, 1000));

        public override void Attack(Vector2 position)
        {
            base.Attack(position);
            GetBase().AddEffect(new Stun((int)Weapon.GetBase().SkillRef.Cooldown));
            GetBase().AddEffect(new Disarm(2000));
        }

        protected override void Update()
        {
            base.Update();
            if (followEntity == null) return;

            switch (aiState)
            {
                case AiState.Pursuing:
                    if (GetBase().Weapon != null && MathBase.GetDistance(Position, followEntity.Position) < GetBase().Weapon!.Distance && GetBase().CanAttack) Attack(followEntity.Position);

                    if (surroundingVisibility.CheckCollision(followEntity.ColliderBox))
                    {
                        FocusedPosition = Vector2.Zero;
                        updateTime();
                        break;
                    }

                    aiState = AiState.Idle;

                    break;

                case AiState.Idle:
                    if (checkTime())
                    {
                        updateTime();
                        updatePosition();
                    }

                    if (surroundingVisibility.CheckCollision(followEntity.ColliderBox)) aiState = AiState.Pursuing;

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            DirectionLooking = (int)MathBase.GetAngle(Position, FocusedPosition);

            if (GetBase().CanMove)
            {
                Direction += getDesirableDirection();
                if (!float.IsFinite(Direction.X) && !float.IsFinite(Direction.Y)) return;
            }

            float rotation = MathBase.GetAngle(Vector2.Zero, Direction) + 90;
            if (!float.IsNaN(rotation)) directionTrack.Rotation = rotation;
            Move(Direction != Vector2.Zero ? Direction.Normalized() : Vector2.Zero, GetSpeed());
        }
    }
}
