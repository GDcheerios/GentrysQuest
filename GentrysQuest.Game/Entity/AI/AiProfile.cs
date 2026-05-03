namespace GentrysQuest.Game.Entity.AI
{
    public class AiProfile
    {
        /// <summary>
        /// The general mindset of the AI, determining how it prioritizes attacking versus survival.
        /// </summary>
        public AiTemperament Temperament { get; set; } = AiTemperament.Balanced;

        /// <summary>
        /// Determines how the AI handles distance relative to its target (e.g., trying to stay at range or closing in).
        /// </summary>
        public AiRangeStyle RangeStyle { get; set; } = AiRangeStyle.Auto;

        /// <summary>
        /// The maximum distance at which the AI can initially detect a potential target.
        /// </summary>
        public float VisionRange { get; set; } = 1500;

        /// <summary>
        /// The distance at which the AI will stop pursuing its current target and return to an idle state.
        /// </summary>
        public float LoseTargetRange { get; set; } = 2200;

        /// <summary>
        /// How long (in milliseconds) the AI will continue to track or move toward a target's last known position after losing line of sight.
        /// </summary>
        public float LastKnownPositionDuration { get; set; } = 2500;

        /// <summary>
        /// The target distance the AI attempts to maintain from its opponent during combat.
        /// </summary>
        public float PreferredDistance { get; set; } = 0;

        /// <summary>
        /// The allowed deviation from the <see cref="PreferredDistance"/> before the AI decides to reposition itself.
        /// </summary>
        public float RangeTolerance { get; set; } = 90;

        /// <summary>
        /// Additional distance added to the effective attack range to prevent the AI from constantly "jittering" at the very edge of its reach.
        /// </summary>
        public float AttackRangePadding { get; set; } = 25;

        /// <summary>
        /// The health threshold (represented as a value from 0.0 to 1.0) at which the AI will prioritize retreating or defensive maneuvers.
        /// </summary>
        public float DefensiveRetreatHealthPercent { get; set; } = 0.35f;

        /// <summary>
        /// The maximum distance the AI can travel from its spawn or anchor point while in a wandering state.
        /// </summary>
        public float WanderRadius { get; set; } = 800;

        /// <summary>
        /// The minimum time (in milliseconds) the AI waits before choosing a new wander destination.
        /// </summary>
        public float WanderIntervalMinimum { get; set; } = 1600;

        /// <summary>
        /// The maximum time (in milliseconds) the AI waits before choosing a new wander destination.
        /// </summary>
        public float WanderIntervalMaximum { get; set; } = 4200;

        /// <summary>
        /// The distance threshold used to determine if the AI should utilize its ranged combat logic.
        /// </summary>
        public float RangedWeaponDistance { get; set; } = 450;

        /// <summary>
        /// The minimum delay (in milliseconds) before an attack is actually triggered after the AI decides to strike.
        /// </summary>
        public float AttackWindupMinimum { get; set; } = 80;

        /// <summary>
        /// The maximum delay (in milliseconds) before an attack is actually triggered after the AI decides to strike.
        /// </summary>
        public float AttackWindupMaximum { get; set; } = 220;

        /// <summary>
        /// The minimum time (in milliseconds) the AI must wait between consecutive attacks.
        /// </summary>
        public float AttackCooldownMinimum { get; set; } = 450;

        /// <summary>
        /// The maximum time (in milliseconds) the AI must wait between consecutive attacks.
        /// </summary>
        public float AttackCooldownMaximum { get; set; } = 950;

        /// <summary>
        /// The minimum time (in milliseconds) a melee attack action is "held" or active.
        /// </summary>
        public float MeleeAttackHoldMinimum { get; set; } = 120;

        /// <summary>
        /// The maximum time (in milliseconds) a melee attack action is "held" or active.
        /// </summary>
        public float MeleeAttackHoldMaximum { get; set; } = 260;

        /// <summary>
        /// The minimum time (in milliseconds) a ranged attack (like a bow draw or spell charge) is held before release.
        /// </summary>
        public float RangedAttackHoldMinimum { get; set; } = 500;

        /// <summary>
        /// The maximum time (in milliseconds) a ranged attack (like a bow draw or spell charge) is held before release.
        /// </summary>
        public float RangedAttackHoldMaximum { get; set; } = 1400;

        /// <summary>
        /// Creates a default AI profile with balanced aggression and standard timing.
        /// </summary>
        public static AiProfile Balanced() => new();

        /// <summary>
        /// Creates a profile that stays closer to targets, attacks more frequently, and tracks targets longer.
        /// </summary>
        public static AiProfile Aggressive() => new()
        {
            Temperament = AiTemperament.Aggressive,
            RangeTolerance = 60,
            AttackRangePadding = 45,
            LastKnownPositionDuration = 4000,
            AttackWindupMinimum = 40,
            AttackWindupMaximum = 140,
            AttackCooldownMinimum = 300,
            AttackCooldownMaximum = 700,
            MeleeAttackHoldMinimum = 180,
            MeleeAttackHoldMaximum = 420
        };

        public static AiProfile Defensive() => new()
        {
            Temperament = AiTemperament.Defensive,
            RangeTolerance = 130,
            DefensiveRetreatHealthPercent = 0.55f,
            AttackWindupMinimum = 140,
            AttackWindupMaximum = 320,
            AttackCooldownMinimum = 700,
            AttackCooldownMaximum = 1300
        };

        public static AiProfile Ranged() => new()
        {
            RangeStyle = AiRangeStyle.LongRange,
            Temperament = AiTemperament.Defensive,
            PreferredDistance = 300,
            RangeTolerance = 100,
            AttackRangePadding = 50,
            RangedAttackHoldMinimum = 800,
            RangedAttackHoldMaximum = 1800
        };
    }
}
