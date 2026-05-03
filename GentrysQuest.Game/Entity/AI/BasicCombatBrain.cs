using System;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Utils;
using osuTK;

namespace GentrysQuest.Game.Entity.AI
{
    public class BasicCombatBrain : AiBrain
    {
        private Vector2 lastKnownTargetPosition;
        private double lastSeenTargetTime = double.MinValue;
        private Vector2 wanderDestination;
        private double nextWanderTime;

        public BasicCombatBrain(DrawableEntity self)
            : base(self)
        {
        }

        public override void OffsetPositions(Vector2 offset)
        {
            base.OffsetPositions(offset);
            lastKnownTargetPosition += offset;
            wanderDestination += offset;
        }

        protected override void Think()
        {
            if (!HasTarget || EntityBase?.CanMove != true)
            {
                StopMoving();
                return;
            }

            float distance = DistanceToTarget;
            bool canSeeTarget = distance <= Profile.VisionRange;

            if (canSeeTarget)
            {
                lastSeenTargetTime = GameClock.CurrentTime;
                lastKnownTargetPosition = Target.Position;
            }

            if (ShouldRetreat(canSeeTarget))
            {
                RetreatFromTarget();
                return;
            }

            if (canSeeTarget)
            {
                fightTarget();
                return;
            }

            if (distance <= Profile.LoseTargetRange && GameClock.CurrentTime - lastSeenTargetTime <= Profile.LastKnownPositionDuration)
            {
                MoveTo(lastKnownTargetPosition, AiState.Investigating, AiMovementMode.Approach, 80);
                return;
            }

            wander();
        }

        protected virtual bool ShouldRetreat(bool canSeeTarget)
        {
            return canSeeTarget
                   && Profile.Temperament == AiTemperament.Defensive
                   && HealthPercent <= Profile.DefensiveRetreatHealthPercent
                   && DistanceToTarget < preferredDistance() + Profile.RangeTolerance;
        }

        private void fightTarget()
        {
            AiRangeStyle rangeStyle = effectiveRangeStyle();
            float preferred = preferredDistance();

            if (rangeStyle == AiRangeStyle.LongRange)
            {
                if (TargetInWeaponRange(Profile.AttackRangePadding))
                    AttackTarget();

                KeepRangeFromTarget(preferred, Profile.RangeTolerance);
                CurrentCommand.ShouldAttack = TargetInWeaponRange(Profile.AttackRangePadding);
                return;
            }

            if (TargetInWeaponRange(Profile.AttackRangePadding))
            {
                moveWhileAttacking(preferred);
                CurrentCommand.ShouldAttack = true;
                CurrentCommand.LookAt = Target.Position;

                return;
            }

            ApproachTarget();
        }

        private void moveWhileAttacking(float preferred)
        {
            switch (Profile.Temperament)
            {
                case AiTemperament.Aggressive:
                    if (DistanceToTarget > Math.Max(WeaponRange * 0.45f, 70))
                        ApproachTarget();
                    else
                        StrafeTarget();
                    break;

                case AiTemperament.Defensive:
                    KeepRangeFromTarget(preferred, Profile.RangeTolerance);
                    break;

                case AiTemperament.Balanced:
                default:
                    StrafeTarget();
                    break;
            }
        }

        private void wander()
        {
            if (GameClock.CurrentTime >= nextWanderTime || MathBase.GetDistance(Self.Position, wanderDestination) < 80)
            {
                wanderDestination = Self.Position + new Vector2(
                    MathBase.RandomFloat(-Profile.WanderRadius, Profile.WanderRadius),
                    MathBase.RandomFloat(-Profile.WanderRadius, Profile.WanderRadius));

                nextWanderTime = GameClock.CurrentTime + MathBase.RandomFloat(Profile.WanderIntervalMinimum, Profile.WanderIntervalMaximum);
            }

            MoveTo(wanderDestination, AiState.Wandering, AiMovementMode.Wander, 80);
        }

        private AiRangeStyle effectiveRangeStyle()
        {
            if (Profile.RangeStyle != AiRangeStyle.Auto)
                return Profile.RangeStyle;

            return WeaponRange >= 450 ? AiRangeStyle.LongRange : AiRangeStyle.ShortRange;
        }

        private float preferredDistance() => Profile.PreferredDistance;
    }
}
