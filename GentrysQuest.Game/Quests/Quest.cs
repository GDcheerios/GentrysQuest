using System;
using System.Collections.Generic;
using System.Linq;

namespace GentrysQuest.Game.Quests
{
    public enum QuestStatus { Locked, Available, Active, Completed, Failed }

    public class Quest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public QuestStatus Status { get; private set; } = QuestStatus.Available;

        private readonly List<Objective> objectives = [];
        public IReadOnlyList<Objective> Objectives => objectives;

        public event Action<Quest> QuestCompleted;
        public event Action<Quest> QuestUpdated;

        public void AddObjective(Objective objective)
        {
            objectives.Add(objective);
            objective.OnProgressChanged += handleObjectiveUpdated;
            objective.OnCompleted += checkProgress;
        }

        private void handleObjectiveUpdated() => QuestUpdated?.Invoke(this);

        private void checkProgress()
        {
            if (objectives.All(o => o.Completed))
            {
                CompleteQuest();
            }
        }

        public void CompleteQuest()
        {
            if (Status == QuestStatus.Completed) return;

            Status = QuestStatus.Completed;
            QuestCompleted?.Invoke(this);
        }

        public void StartQuest()
        {
            if (Status == QuestStatus.Available)
                Status = QuestStatus.Active;
        }
    }
}
