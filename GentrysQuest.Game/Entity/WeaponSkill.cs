// * Name              : GentrysQuest.Game
//  * Author           : Brayden J Messerschmidt
//  * Created          : 09/18/2024
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

public class WeaponSkill : Skill
{
    public override string Name { get; protected set; } = "Weapon";
    public override string Description { get; protected set; } = "Your weapon";
    public override double Cooldown { get; protected set; }

    /// <summary>
    /// Cooldown is normally not to be set from outside
    /// so we have to make an override setter
    /// </summary>
    /// <param name="cooldown">The new cooldown</param>
    public void SetCooldown(double cooldown) => Cooldown = cooldown;
}
