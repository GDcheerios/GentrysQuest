using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Gachas;

namespace GentrysQuest.Game.Content.Gachas
{
    public class StarterGacha : Gacha
    {
        public StarterGacha()
        {
            Name = "Starter";
            Price = 1000;
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
