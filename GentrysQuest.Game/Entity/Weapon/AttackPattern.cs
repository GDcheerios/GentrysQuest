using System.Collections.Generic;
using System.Linq;

namespace GentrysQuest.Game.Entity.Weapon
{
    public class AttackPattern
    {
        private readonly List<AttackPatternCaseHolder> caseEventList = new();
        private AttackPatternCaseHolder selectedCaseHolder;

        public void AddCase()
        {
            AttackPatternCaseHolder thePattern = new AttackPatternCaseHolder(caseEventList.Count);
            selectedCaseHolder = thePattern;
            caseEventList.Add(thePattern);
        }

        public void SetCaseHolder(int caseNumber) => selectedCaseHolder = GetCase(caseNumber);
        public void Add(AttackPatternEvent attackPatternEvent) => selectedCaseHolder.AddEvent(attackPatternEvent);

        public AttackPatternEvent GetFirstCaseEvent() => caseEventList[0].GetEvents()[0];

        public AttackPatternCaseHolder GetCase(int caseNumber) => caseEventList.FirstOrDefault(caseHolder => caseHolder.AttackNumberCase == caseNumber);

        public int GetListAmount() => caseEventList.Count;
    }
}
