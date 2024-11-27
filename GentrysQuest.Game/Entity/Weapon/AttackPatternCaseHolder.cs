using System.Collections.Generic;

namespace GentrysQuest.Game.Entity.Weapon
{
    public class AttackPatternCaseHolder
    {
        public int AttackNumberCase { get; private set; }
        private List<AttackPatternEvent> caseEvents;

        public AttackPatternCaseHolder(int attackNumberCase = 0)
        {
            AttackNumberCase = attackNumberCase;
            caseEvents = new List<AttackPatternEvent>();
        }

        public void AddEvent(AttackPatternEvent pattern) => caseEvents.Add(pattern);

        public List<AttackPatternEvent> GetEvents() => caseEvents;

        public AttackPatternEvent GetLastEvent() => caseEvents[^1];
    }
}
