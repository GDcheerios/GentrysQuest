// * Name              : GentrysQuest.Game
//  * Author           : Brayden J Messerschmidt
//  * Created          : 05/28/2024
//  * Course           : CIS 169 C#
//  * Version          : 1.0
//  * OS               : Windows 11 22H2
//  * IDE              : Jet Brains Rider 2023
//  * Copyright        : This is my work.
//  * Description      : desc.
//  * Academic Honesty : I attest that this is my original work.
//  * I have not used unauthorized source code, either modified or
//  * unmodified. I have not given other fellow student(s) access
//  * to my program.

namespace GentrysQuest.Game.Entity;

public class IntStat : Stat
{
    public IntStat(string name, StatType statType, double minimumValue)
        : base(name, statType, minimumValue)
    {
        // This is how it's meant to be! :)
    }

    public override double GetDefault() => (int)base.GetDefault();
    public override double GetAdditional() => (int)base.GetAdditional();
    public override double GetCurrent() => (int)base.GetCurrent();

    public override double Total()
    {
        return (int)base.Total();
    }
}
