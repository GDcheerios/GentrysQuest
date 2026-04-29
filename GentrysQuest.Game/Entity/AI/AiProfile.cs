namespace GentrysQuest.Game.Entity.AI
{
    public class AiProfile
    {
        public AiTemperament Temperament { get; set; } = AiTemperament.Balanced;
        public AiRangeStyle RangeStyle { get; set; } = AiRangeStyle.Auto;

        public float VisionRange { get; set; } = 1500;
        public float LoseTargetRange { get; set; } = 2200;
        public float LastKnownPositionDuration { get; set; } = 2500;

        public float PreferredDistance { get; set; } = 0;
        public float RangeTolerance { get; set; } = 90;
        public float AttackRangePadding { get; set; } = 25;
        public float DefensiveRetreatHealthPercent { get; set; } = 0.35f;

        public float WanderRadius { get; set; } = 800;
        public float WanderIntervalMinimum { get; set; } = 1600;
        public float WanderIntervalMaximum { get; set; } = 4200;

        public float RangedWeaponDistance { get; set; } = 450;
        public float AttackWindupMinimum { get; set; } = 80;
        public float AttackWindupMaximum { get; set; } = 220;
        public float AttackCooldownMinimum { get; set; } = 450;
        public float AttackCooldownMaximum { get; set; } = 950;
        public float MeleeAttackHoldMinimum { get; set; } = 120;
        public float MeleeAttackHoldMaximum { get; set; } = 260;
        public float RangedAttackHoldMinimum { get; set; } = 500;
        public float RangedAttackHoldMaximum { get; set; } = 1400;

        public static AiProfile Balanced() => new();

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
            PreferredDistance = 200,
            RangeTolerance = 150,
            RangedAttackHoldMinimum = 800,
            RangedAttackHoldMaximum = 1800
        };
    }
}
