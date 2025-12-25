using System;
using System.Collections.Generic;
using System.Linq;

namespace GentrysQuest.Game.Quests;

public static class QuestManager
{
    private static readonly List<Quest> all_quests = [];
    private static readonly List<Quest> active_quests = [];

    public static event Action<Quest> OnQuestStarted;
    public static event Action<Quest> OnQuestCompleted;

    /// <summary>
    /// Registers a quest into the global database
    /// </summary>
    public static void RegisterQuest(Quest quest)
    {
        if (!all_quests.Contains(quest)) all_quests.Add(quest);
    }

    /// <summary>
    /// Registers a quest into the global database and immediately accepts it
    /// </summary>
    public static void PushQuest(Quest quest)
    {
        RegisterQuest(quest);
        AcceptQuest(quest);
    }

    /// <summary>
    /// Moves a quest from the database to the active list
    /// </summary>
    public static void AcceptQuest(Quest quest)
    {
        if (active_quests.Contains(quest)) return;

        quest.StartQuest();
        active_quests.Add(quest);

        quest.QuestCompleted += handleQuestCompletion;
        OnQuestStarted?.Invoke(quest);
    }

    /// <summary>
    /// The global "Signal" that something happened in the game.
    /// Any active quest with a matching objective name will progress.
    /// </summary>
    public static void SignalProgress(string objectiveName, int amount = 1)
    {
        foreach (var quest in active_quests)
        {
            var matchingObjectives = quest.Objectives
                                          .Where(o => o.Name == objectiveName && !o.Completed);

            foreach (var objective in matchingObjectives)
            {
                objective.Increment(amount);
            }
        }
    }

    /// <summary>
    /// Sets the progress of an objective to a specific value.
    /// Any active quest with a matching objective name will have its progress set.
    /// </summary>
    public static void SignalSet(string objectiveName, int value)
    {
        foreach (var quest in active_quests.ToList())
        {
            var matchingObjectives = quest.Objectives
                                          .Where(o => o.Name == objectiveName && !o.Completed);

            foreach (var objective in matchingObjectives)
            {
                objective.SetProgress(value);
            }
        }
    }

    /// <summary>
    /// Marks an objective as complete.
    /// Any active quest with a matching objective name will be marked as complete.
    /// </summary>
    public static void SignalComplete(string objectiveName)
    {
        foreach (var quest in active_quests.ToList())
        {
            var matchingObjectives = quest.Objectives
                                          .Where(o => o.Name == objectiveName && !o.Completed);

            foreach (var objective in matchingObjectives)
            {
                objective.Complete();
            }
        }
    }

    private static void handleQuestCompletion(Quest quest)
    {
        active_quests.Remove(quest);
        quest.QuestCompleted -= handleQuestCompletion;
        OnQuestCompleted?.Invoke(quest);
    }

    public static IEnumerable<Quest> GetActiveQuests() => active_quests;
}
