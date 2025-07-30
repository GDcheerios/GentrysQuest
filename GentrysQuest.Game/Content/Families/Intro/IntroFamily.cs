using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;

namespace GentrysQuest.Game.Content.Families.Intro
{
    public class IntroFamily : Family
    {
        public IntroFamily()
        {
            Name = "Intro Family";
            Description = "The intro family";
            Artifacts.Add(typeof(Keyboard));
        }
    }

    public class Keyboard : Artifact
    {
        public override string Name { get; set; } = "Keyboard";
        public override string Description { get; protected set; } = "A keyboard";
        public override Family family { get; protected set; } = new IntroFamily();
        public override List<StatType> ValidMainAttributes { get; set; } = [StatType.Health];
        public override List<int> ValidStarRatings { get; set; } = [1];

        public Keyboard()
        {
            TextureMapping = new TextureMapping();
            TextureMapping.Add("Icon", "artifacts_keyboard.png");
        }
    }
}
