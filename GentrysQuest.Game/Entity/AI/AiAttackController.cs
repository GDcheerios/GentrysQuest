using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Utils;
using osuTK;

namespace GentrysQuest.Game.Entity.AI
{
    public class AiAttackController
    {
        private readonly DrawableEntity self;

        private DrawableEntity target;
        private AiAttackState state = AiAttackState.Ready;
        private double nextAttackTime;
        private double stateEndTime;

        public string DebugState => state.ToString();

        public AiAttackController(DrawableEntity self)
        {
            this.self = self;
        }

        public void SetTarget(DrawableEntity target) => this.target = target;

        public void Update(bool wantsAttack, Vector2 targetPosition, AiProfile profile)
        {
            if (self.GetBase().Weapon == null || target == null || target.GetBase().IsDead)
            {
                cancelAttack();
                return;
            }

            double now = GameClock.CurrentTime;

            switch (state)
            {
                case AiAttackState.Ready:
                    if (wantsAttack && now >= nextAttackTime && self.GetBase().CanAttack)
                        beginWindup(now, profile);
                    break;

                case AiAttackState.Windup:
                    if (!wantsAttack)
                    {
                        cancelAttack();
                        break;
                    }

                    if (now >= stateEndTime)
                        beginAttack(now, targetPosition, profile);
                    break;

                case AiAttackState.Holding:
                    if (now >= stateEndTime || !wantsAttack)
                        releaseAttack(now, profile);
                    break;

                case AiAttackState.Recovering:
                    if (now >= stateEndTime)
                        state = AiAttackState.Ready;
                    break;
            }
        }

        private void beginWindup(double now, AiProfile profile)
        {
            state = AiAttackState.Windup;
            stateEndTime = now + MathBase.RandomFloat(profile.AttackWindupMinimum, profile.AttackWindupMaximum);
        }

        private void beginAttack(double now, Vector2 targetPosition, AiProfile profile)
        {
            self.DoAttack(targetPosition);
            state = AiAttackState.Holding;
            stateEndTime = now + holdDuration(profile);
        }

        private void releaseAttack(double now, AiProfile profile)
        {
            self.OnRelease();
            state = AiAttackState.Recovering;
            nextAttackTime = now + MathBase.RandomFloat(profile.AttackCooldownMinimum, profile.AttackCooldownMaximum);
            stateEndTime = nextAttackTime;
        }

        private void cancelAttack()
        {
            if (state == AiAttackState.Holding)
                self.OnRelease();

            state = AiAttackState.Ready;
        }

        private float holdDuration(AiProfile profile)
        {
            bool isRanged = profile.RangeStyle == AiRangeStyle.LongRange
                            || self.GetBase().Weapon.Distance >= profile.RangedWeaponDistance;

            return isRanged
                ? MathBase.RandomFloat(profile.RangedAttackHoldMinimum, profile.RangedAttackHoldMaximum)
                : MathBase.RandomFloat(profile.MeleeAttackHoldMinimum, profile.MeleeAttackHoldMaximum);
        }

        private enum AiAttackState
        {
            Ready,
            Windup,
            Holding,
            Recovering
        }
    }
}
