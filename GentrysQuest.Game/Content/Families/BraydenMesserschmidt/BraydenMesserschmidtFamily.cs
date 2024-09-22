using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Families.BraydenMesserschmidt
{
    public class BraydenMesserschmidtFamily : Family
    {
        public BraydenMesserschmidtFamily()
        {
            Name = "Brayden Messerschmidt Family";
            TwoSetBuff = new TwoSetBuff(new Buff(20, StatType.CritDamage, false));
            FourSetBuff = new BraydenMesserschmidtFourSetBuff();

            Artifacts.Add(typeof(OsuTablet));
            Artifacts.Add(typeof(MadokaChibiPlush));
        }
    }
}
