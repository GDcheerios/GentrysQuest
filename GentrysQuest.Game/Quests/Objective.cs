using System;

namespace GentrysQuest.Game.Quests
{
    public class Objective
    {
        /// <summary>
        /// The name of the objective
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Is the objective hidden?
        /// </summary>
        public bool Hidden { get; set; } = false;

        /// <summary>
        /// Has the objective been completed?
        /// </summary>
        public bool Completed { get; private set; } = false;

        public int CurrentValue { get; private set; }
        public int TargetValue { get; set; } = 1;

        public event Action OnCompleted;
        public event Action OnProgressChanged;

        public void Increment(int amount = 1)
        {
            if (Completed) return;

            CurrentValue = Math.Clamp(CurrentValue + amount, 0, TargetValue);
            OnProgressChanged?.Invoke();

            if (CurrentValue >= TargetValue)
            {
                Completed = true;
                OnCompleted?.Invoke();
            }
        }

        public void SetProgress(int value)
        {
            if (Completed) return;

            CurrentValue = Math.Clamp(value, 0, TargetValue);
            OnProgressChanged?.Invoke();

            if (CurrentValue >= TargetValue)
            {
                Completed = true;
                OnCompleted?.Invoke();
            }
        }

        public void Complete()
        {
            Completed = true;
            OnCompleted?.Invoke();
        }
    }
}
