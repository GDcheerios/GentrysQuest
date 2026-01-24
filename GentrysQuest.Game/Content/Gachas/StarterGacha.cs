using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Gachas;

namespace GentrysQuest.Game.Content.Gachas
{
    public class StarterGacha : Gacha
    {
        public StarterGacha()
        {
            Name = "Starter Gacha";
            Price = 100;
            Weapons =
            [
                new Bow(),
                new Hammer(),
                new Sword(),
                new Spear()
            ];
        }
    }
}
