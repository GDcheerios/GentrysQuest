using System;
using System.Collections.Generic;
using GentrysQuest.Game.Entity.AI;
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
        private AiBrain brain;
        private AiAttackController attackController;
        private Vector2 lastPosition;
        private Vector2 smoothedDirection;
        private double lastStuckCheckTime;
        private double stuckRecoverUntil;
        private const float STUCK_DISTANCE_THRESHOLD = 2f;
        private const double STUCK_CHECK_INTERVAL = 500;
        private const double STUCK_RECOVER_TIME = 450;
        private const float TURN_EASE_PER_FRAME = 0.14f;
        private const float STOP_EASE_PER_FRAME = 0.35f;
        private const float STOP_DIRECTION_THRESHOLD = 0.12f;
        public AiState AiState = AiState.Idle;

        public AiCommand CurrentAiCommand => brain?.CurrentCommand;
        public string CurrentAttackState => attackController?.DebugState ?? "None";

        public DrawableEnemyEntity(Entity entity)
            : base(entity, AffiliationType.Enemy)
        {
            OnMove += delegate(Vector2 direction, double speed)
            {
                if (!float.IsNaN(direction.X) && !float.IsNaN(direction.Y))
                    Position += direction * (float)Clock.ElapsedFrameTime * (float)speed;
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            ColliderBox.Size = new Vector2(0.1f);
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

            brain = GetBase().CreateAiBrain(this);
            brain.SetTarget(followEntity);
            attackController = new AiAttackController(this);
            attackController.SetTarget(followEntity);
        }

        public void FollowEntity(DrawableEntity drawableEntity)
        {
            followEntity = drawableEntity;
            brain?.SetTarget(drawableEntity);
            attackController?.SetTarget(drawableEntity);
        }

        public void OffsetAiPositions(Vector2 offset) => brain?.OffsetPositions(offset);

        private Vector2 getSteeredDirection(Vector2 desiredDirection)
        {
            if (!isUsableDirection(desiredDirection))
                return Vector2.Zero;

            Dictionary<int, bool> angles = EnemyController.GetIntersectedAngles();
            Vector2 bestDirection = Vector2.Zero;
            float bestScore = float.MinValue;

            foreach (KeyValuePair<int, bool> angle in angles)
            {
                Vector2 directionVector = MathBase.GetAngleToVector(angle.Key).Normalized();
                float score = Vector2.Dot(desiredDirection, directionVector);

                if (angle.Value)
                    score -= 3;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestDirection = directionVector;
                }
            }

            if (bestScore < -1)
                return getUnstuckDirection();

            return bestDirection.Normalized();
        }

        private Vector2 getDesiredDirection(AiCommand command)
        {
            if (Clock.CurrentTime < stuckRecoverUntil)
                return getUnstuckDirection();

            if (command.MovementPattern != null)
                return command.MovementPattern.GetDirection(this, followEntity);

            if (command.Direction != null)
                return command.Direction.Value;

            if (command.Destination == null)
                return Vector2.Zero;

            if (MathBase.GetDistance(Position, command.Destination.Value) <= command.AcceptanceRadius)
                return Vector2.Zero;

            return safeDirection(Position, command.Destination.Value);
        }

        private Vector2 getUnstuckDirection()
        {
            float angle = (float)(Clock.CurrentTime / 7 % 360);
            return MathBase.GetAngleToVector(angle).Normalized();
        }

        public override void DoAttack(Vector2 position)
        {
            base.DoAttack(position);
        }

        private void updateStuckState(Vector2 movementDirection)
        {
            if (movementDirection == Vector2.Zero || Clock.CurrentTime - lastStuckCheckTime < STUCK_CHECK_INTERVAL)
                return;

            if (MathBase.GetDistance(Position, lastPosition) < STUCK_DISTANCE_THRESHOLD)
            {
                stuckRecoverUntil = Clock.CurrentTime + STUCK_RECOVER_TIME;
                AiState = AiState.StuckRecovering;
            }

            lastPosition = Position;
            lastStuckCheckTime = Clock.CurrentTime;
        }

        private Vector2 easeMovementDirection(Vector2 targetDirection)
        {
            float ease = targetDirection == Vector2.Zero ? STOP_EASE_PER_FRAME : TURN_EASE_PER_FRAME;
            float frameScale = (float)(Clock.ElapsedFrameTime / (1000d / 60d));
            float amount = 1 - MathF.Pow(1 - ease, frameScale);

            smoothedDirection += (targetDirection - smoothedDirection) * amount;

            if (targetDirection == Vector2.Zero && MathBase.GetMagnitude(smoothedDirection) < STOP_DIRECTION_THRESHOLD)
            {
                smoothedDirection = Vector2.Zero;
                return Vector2.Zero;
            }

            return smoothedDirection;
        }

        protected override void Update()
        {
            base.Update();
            if (followEntity == null) return;

            brain.UpdateBrain();
            AiCommand command = brain.CurrentCommand;
            AiState = command.State;

            Vector2 lookAt = command.LookAt ?? command.Destination ?? followEntity.Position;
            if (isUsableDirection(lookAt - Position))
                DirectionLooking = (int)MathBase.GetAngle(Position, lookAt);

            bool canReachTarget = GetBase().Weapon != null
                                  && MathBase.GetDistance(Position, followEntity.Position) <= GetBase().Weapon!.Distance + GetBase().AiProfile.AttackRangePadding;
            attackController.Update(command.ShouldAttack && canReachTarget, followEntity.Position, GetBase().AiProfile);

            Vector2 desiredDirection = getDesiredDirection(command);
            Vector2 targetDirection = Vector2.Zero;

            if (GetBase().CanMove && desiredDirection != Vector2.Zero)
            {
                targetDirection = getSteeredDirection(desiredDirection);
                updateStuckState(targetDirection);
            }

            Direction += easeMovementDirection(targetDirection);

            if (Direction != Vector2.Zero && !isUsableDirection(Direction))
                return;

            float rotation = MathBase.GetAngle(Vector2.Zero, Direction) + 90;
            if (!float.IsNaN(rotation)) directionTrack.Rotation = rotation;
            Move(Direction != Vector2.Zero ? Direction.Normalized() : Vector2.Zero, GetSpeed());
        }

        private static Vector2 safeDirection(Vector2 from, Vector2 to)
        {
            Vector2 direction = to - from;

            if (!isUsableDirection(direction))
                return Vector2.Zero;

            direction.Normalize();
            return direction;
        }

        private static bool isUsableDirection(Vector2 direction)
        {
            return direction != Vector2.Zero && float.IsFinite(direction.X) && float.IsFinite(direction.Y);
        }
    }
}
