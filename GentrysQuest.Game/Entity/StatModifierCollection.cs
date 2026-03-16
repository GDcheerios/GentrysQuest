using System;
using System.Collections.Generic;
using System.Linq;

namespace GentrysQuest.Game.Entity
{
    public class StatModifierCollection
    {
        private readonly Dictionary<string, List<StatModifier>> modifiersBySource = new();

        public event Action OnChange;

        public void SetSource(string sourceKey, IEnumerable<StatModifier> modifiers)
        {
            modifiersBySource[sourceKey] = modifiers.ToList();
            OnChange?.Invoke();
        }

        public void RemoveSource(string sourceKey)
        {
            if (!modifiersBySource.Remove(sourceKey))
                return;

            OnChange?.Invoke();
        }

        public void RemoveSourcesByPrefix(string prefix)
        {
            List<string> sourceKeys = modifiersBySource.Keys.Where(key => key.StartsWith(prefix)).ToList();

            if (sourceKeys.Count == 0)
                return;

            foreach (string sourceKey in sourceKeys)
                modifiersBySource.Remove(sourceKey);

            OnChange?.Invoke();
        }

        public IReadOnlyList<StatModifier> ForStat(StatType statType) =>
            modifiersBySource.Values
                             .SelectMany(modifiers => modifiers)
                             .Where(modifier => modifier.StatType == statType)
                             .ToList();

        public void Clear()
        {
            if (modifiersBySource.Count == 0)
                return;

            modifiersBySource.Clear();
            OnChange?.Invoke();
        }
    }
}
