using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Families.BenMeier
{
    public class BenMeierFamily : Family
    {
        public BenMeierFamily()
        {
            Name = "Ben Meier Family";
            Description = "Ben Meier Family";

            TwoSetBuff = new TwoSetBuff(new Buff(20, StatType.AttackSpeed, true));
            FourSetBuff = new BenMeierFourSetBuff();

            Artifacts.Add(typeof(EnergyDrink));
        }
    }
}
