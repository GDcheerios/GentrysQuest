using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Gachas;

namespace GentrysQuest.Game.Content.Gachas
{
    /// <summary>
    /// Test gacha that contains all the items
    /// </summary>
    public class GameGacha : Gacha
    {
        public GameGacha()
        {
            Price = 0;
            Name = "Game Gacha";
            Characters =
            [
                new GMoney(),
                new BraydenMesserschmidt(),
                new MekhiElliot(),
                new PhilipMcClure(),
                new TestCharacter(1),
                new TestCharacter(2),
                new TestCharacter(3),
                new TestCharacter(4),
                new TestCharacter(5)
            ];
            Weapons =
            [
                new Bow(),
                new Sword(),
                new Hammer(),
                new Knife(),
                new Spear(),
                new BraydensOsuPen(),
                new BrodysBroadsword()
            ];
        }
    }
}
