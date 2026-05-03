using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Utils;
using osuTK;

namespace GentrysQuest.Game.Entity.AI
{
    public abstract class AiBrain
    {
        protected readonly DrawableEntity Self;
        protected DrawableEntity Target;

        protected AiProfile Profile => Self.GetBase().AiProfile ?? AiProfile.Balanced();
        protected Entity EntityBase => Self.GetBase();
        protected AiCommand Command { get; private set; } = new();

        public AiCommand CurrentCommand => Command;

        protected AiBrain(DrawableEntity self)
        {
            Self = self;
        }

        public virtual void SetTarget(DrawableEntity target) => Target = target;

        public virtual void OffsetPositions(Vector2 offset)
        {
            Command.MovementPattern?.OffsetPositions(offset);
        }

        public void UpdateBrain()
        {
            Command = new AiCommand();
            Think();
        }

        protected abstract void Think();

        protected bool HasTarget => Target != null && !Target.GetBase().IsDead;
        protected float DistanceToTarget => HasTarget ? (float)MathBase.GetDistance(Self.Position, Target.Position) : float.MaxValue;
        protected Vector2 DirectionToTarget => HasTarget ? safeDirection(Self.Position, Target.Position) : Vector2.Zero;
        protected Vector2 DirectionFromTarget => -DirectionToTarget;

        protected int WeaponRange => EntityBase?.Weapon?.Distance ?? 75;

        protected bool TargetInWeaponRange(float padding = 0) => HasTarget && DistanceToTarget <= WeaponRange + padding;

        protected float HealthPercent
        {
            get
            {
                if (EntityBase == null || EntityBase.Stats.Health.Total() <= 0)
                    return 1;

                return (float)(EntityBase.Stats.Health.Current.Value / EntityBase.Stats.Health.Total());
            }
        }

        protected void ChangeState(AiState state)
        {
            Command.State = state;
        }

        protected void StopMoving(AiState state = AiState.Idle)
        {
            Command.State = state;
            Command.MovementMode = AiMovementMode.None;
        }

        protected void MoveTo(Vector2 destination, AiState state = AiState.FollowGoal, AiMovementMode mode = AiMovementMode.Direct, float acceptanceRadius = 30)
        {
            Command.State = state;
            Command.MovementMode = mode;
            Command.Destination = destination;
            Command.AcceptanceRadius = acceptanceRadius;
        }

        protected void Move(Vector2 direction, AiState state = AiState.FollowGoal, AiMovementMode mode = AiMovementMode.Direct)
        {
            Command.State = state;
            Command.MovementMode = mode;
            Command.Direction = safeNormalize(direction);
        }

        protected void MoveBy(Vector2 offset, AiState state = AiState.FollowGoal, float acceptanceRadius = 30)
            => MoveTo(Self.Position + offset, state, AiMovementMode.Direct, acceptanceRadius);

        protected void MoveAtAngle(float degrees, AiState state = AiState.FollowGoal)
            => Move(MathBase.GetAngleToVector(degrees), state, AiMovementMode.Direct);

        protected void MovePattern(AiMovementPattern pattern, AiState state = AiState.FollowGoal)
        {
            Command.State = state;
            Command.MovementMode = AiMovementMode.Pattern;
            Command.MovementPattern = pattern;
        }

        protected void Patrol(params Vector2[] points) => MovePattern(new AiWaypointPattern(60, points), AiState.Wandering);

        protected void CircleTarget(float radius, bool clockwise = true)
        {
            if (!HasTarget) return;

            Command.LookAt = Target.Position;
            MovePattern(new AiCircleTargetPattern(radius, clockwise), AiState.Repositioning);
        }

        protected void ApproachTarget()
        {
            if (!HasTarget) return;

            Command.LookAt = Target.Position;
            MoveTo(Target.Position, AiState.Pursuing, AiMovementMode.Approach);
        }

        protected void KeepRangeFromTarget(float preferredDistance, float tolerance)
        {
            if (!HasTarget) return;

            Command.LookAt = Target.Position;

            if (DistanceToTarget < preferredDistance - tolerance)
                Move(DirectionFromTarget, AiState.Repositioning, AiMovementMode.Retreat);
            else if (DistanceToTarget > preferredDistance + tolerance)
                MoveTo(Target.Position, AiState.Pursuing, AiMovementMode.KeepRange);
            else
                StrafeTarget();
        }

        protected void RetreatFromTarget()
        {
            if (!HasTarget) return;

            Command.LookAt = Target.Position;
            Move(DirectionFromTarget, AiState.Retreating, AiMovementMode.Retreat);
        }

        protected void StrafeTarget()
        {
            if (!HasTarget) return;

            Vector2 direction = DirectionToTarget;
            Vector2 strafe = new(-direction.Y, direction.X);

            if ((int)(GameClock.CurrentTime / 1800) % 2 == 0)
                strafe = -strafe;

            Command.LookAt = Target.Position;
            Move(strafe, AiState.Repositioning, strafe.X + strafe.Y >= 0 ? AiMovementMode.StrafeClockwise : AiMovementMode.StrafeCounterClockwise);
        }

        protected void AttackTarget()
        {
            if (!HasTarget) return;

            Command.State = AiState.Attacking;
            Command.LookAt = Target.Position;
            Command.ShouldAttack = true;
        }

        private static Vector2 safeDirection(Vector2 from, Vector2 to) => safeNormalize(to - from);

        private static Vector2 safeNormalize(Vector2 direction)
        {
            if (direction == Vector2.Zero || float.IsNaN(direction.X) || float.IsNaN(direction.Y))
                return Vector2.Zero;

            direction.Normalize();
            return direction;
        }
    }
}
