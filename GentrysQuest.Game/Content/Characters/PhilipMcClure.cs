using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Characters
{
    public class PhilipMcClure : Character
    {
        public PhilipMcClure()
        {
            Name = "Philip McClure";
            StarRating = new StarRating(5);

            TextureMapping.Add("Idle", "characters_philip_mcclure_idle.png");

            AudioMapping.Add("Spawn", "characters_philip_mcclure_spawn.mp3");
            AudioMapping.Add("Damage", "characters_philip_mcclure_damage.mp3");
            AudioMapping.Add("Death", "characters_philip_mcclure_death.mp3");
            AudioMapping.Add("Levelup", "characters_philip_mcclure_levelup.mp3");
        }
    }
}
