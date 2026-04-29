using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Utils;
using osuTK;

namespace GentrysQuest.Game.Entity.AI
{
    public abstract class AiMovementPattern
    {
        public abstract Vector2 GetDirection(DrawableEntity self, DrawableEntity target);

        public virtual void OffsetPositions(Vector2 offset)
        {
        }

        protected static Vector2 DirectionTo(Vector2 from, Vector2 to)
        {
            Vector2 direction = to - from;

            if (direction == Vector2.Zero || float.IsNaN(direction.X) || float.IsNaN(direction.Y))
                return Vector2.Zero;

            direction.Normalize();
            return direction;
        }
    }

    public class AiWaypointPattern : AiMovementPattern
    {
        private readonly Vector2[] points;
        private readonly float acceptanceRadius;
        private int currentIndex;

        public AiWaypointPattern(float acceptanceRadius, params Vector2[] points)
        {
            this.points = points;
            this.acceptanceRadius = acceptanceRadius;
        }

        public override Vector2 GetDirection(DrawableEntity self, DrawableEntity target)
        {
            if (points == null || points.Length == 0)
                return Vector2.Zero;

            if (MathBase.GetDistance(self.Position, points[currentIndex]) <= acceptanceRadius)
                currentIndex = (currentIndex + 1) % points.Length;

            return DirectionTo(self.Position, points[currentIndex]);
        }

        public override void OffsetPositions(Vector2 offset)
        {
            for (int i = 0; i < points.Length; i++)
                points[i] += offset;
        }
    }

    public class AiCircleTargetPattern : AiMovementPattern
    {
        private readonly float radius;
        private readonly bool clockwise;

        public AiCircleTargetPattern(float radius, bool clockwise = true)
        {
            this.radius = radius;
            this.clockwise = clockwise;
        }

        public override Vector2 GetDirection(DrawableEntity self, DrawableEntity target)
        {
            if (target == null)
                return Vector2.Zero;

            Vector2 toTarget = DirectionTo(self.Position, target.Position);
            Vector2 tangent = clockwise ? new Vector2(-toTarget.Y, toTarget.X) : new Vector2(toTarget.Y, -toTarget.X);
            float distance = (float)MathBase.GetDistance(self.Position, target.Position);
            Vector2 rangeCorrection = distance > radius ? toTarget : -toTarget;

            return DirectionTo(Vector2.Zero, tangent + rangeCorrection * 0.4f);
        }
    }
}
