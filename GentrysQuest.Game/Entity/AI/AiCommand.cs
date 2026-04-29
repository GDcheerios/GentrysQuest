using osuTK;

namespace GentrysQuest.Game.Entity.AI
{
    public class AiCommand
    {
        public AiState State { get; set; } = AiState.Idle;
        public AiMovementMode MovementMode { get; set; } = AiMovementMode.None;
        public Vector2? Destination { get; set; }
        public Vector2? Direction { get; set; }
        public Vector2? LookAt { get; set; }
        public AiMovementPattern MovementPattern { get; set; }
        public bool ShouldAttack { get; set; }
        public float AcceptanceRadius { get; set; } = 30;

        public bool HasMovement =>
            MovementMode != AiMovementMode.None
            && (Destination != null || Direction != null || MovementPattern != null);
    }
}
