using GentrysQuest.Game.Content.Effects;
using GentrysQuest.Game.Content.Skills;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Content.Characters
{
    public class BraydenMesserschmidt : Character
    {
        public BraydenMesserschmidt()
        {
            Name = "Brayden Messerschmidt";
            StarRating = new StarRating(5);
            Description = "An osu player who formed a contract with ppy(Dean Herbert) to not talk to females.";

            Stats.Speed.point = 1;
            Stats.AttackSpeed.point = 1;
            Stats.CritRate.point = 1;
            Stats.CritDamage.point = 1;

            Secondary = new CircleThrow();
            Utility = new Teleport();

            OnUpdateStats += checkWeapon;

            TextureMapping.Add("Icon", "brayden_idle.png");
            TextureMapping.Add("Idle", "brayden_idle.png");

            AudioMapping.Add("Spawn", "Brayden_Messerschmidt_Spawn.mp3");
            AudioMapping.Add("Damage", "Brayden_Messerschmidt_Damage.mp3");
        }

        private void checkWeapon()
        {
            Logger.Log("I'm checking the waepon");
            RemoveEffect("Chosen One");

            if (Weapon != null && Weapon.GetType() == typeof(BraydensOsuPen))
            {
                AddEffect(new ChosenOne
                {
                    Stack = 1 + Difficulty
                });
            }
        }
    }
}
