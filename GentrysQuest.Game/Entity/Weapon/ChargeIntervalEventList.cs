using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace GentrysQuest.Game.Entity.Weapon
{
    public class ChargeIntervalEventList
    {
        [CanBeNull]
        private Dictionary<int, AttackPatternCaseHolder> chargeIntervalEvents;

        /// <summary>
        /// Clears interval list
        /// </summary>
        public void Clear() => chargeIntervalEvents = null;

        public void SetInterval(int chargeTime, AttackPatternCaseHolder pattern)
        {
            if (chargeIntervalEvents == null) chargeIntervalEvents = new Dictionary<int, AttackPatternCaseHolder>();
            chargeIntervalEvents[chargeTime] = pattern;
        }

        /// <summary>
        /// Tries to get the first ready charge interval and removes it from the list.
        /// </summary>
        [CanBeNull]
        public AttackPatternCaseHolder TryGetReadyCharge(int chargeTime)
        {
            if (chargeIntervalEvents == null || !chargeIntervalEvents.Any()) return null;

            var readyPair = chargeIntervalEvents.FirstOrDefault(pair => pair.Key <= chargeTime);

            if (readyPair.Equals(default(KeyValuePair<int, AttackPatternCaseHolder>))) return null;

            chargeIntervalEvents.Remove(readyPair.Key);
            return readyPair.Value;
        }
    }
}
