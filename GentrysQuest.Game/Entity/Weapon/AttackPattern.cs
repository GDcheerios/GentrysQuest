using System.Collections.Generic;

namespace GentrysQuest.Game.Entity.Weapon
{
    public class AttackPattern
    {
        private readonly List<AttackPatternCaseHolder> caseEventList = new();
        private AttackPatternCaseHolder selectedCaseHolder;

        public void AddCase(int caseNumber)
        {
            AttackPatternCaseHolder thePattern = new AttackPatternCaseHolder(caseNumber);
            selectedCaseHolder = thePattern;
            caseEventList.Add(thePattern);
        }

        public void SetCaseHolder(int caseNumber) => selectedCaseHolder = GetCase(caseNumber);
        public void Add(AttackPatternEvent attackPatternEvent) => selectedCaseHolder.AddEvent(attackPatternEvent);

        public AttackPatternEvent GetFirstCaseEvent() => caseEventList[0].GetEvents()[0];

        public AttackPatternCaseHolder GetCase(int caseNumber)
        {
            foreach (AttackPatternCaseHolder caseHolder in caseEventList)
            {
                if (caseHolder.AttackNumberCase == caseNumber) return caseHolder;
            }

            return null;
        }

        public int GetListAmount() => caseEventList.Count;
    }
}
