using System.Collections.Generic;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;

namespace GentrysQuest.Game.Content.Artifacts
{
    public class ElHefe : Artifact
    {
        public override string Name { get; set; } = "ElHefe";
        public override string Description { get; protected set; } = "The customer service plant named ElHefe";
        public override List<StatType> ValidMainAttributes { get; set; } = [StatType.Health];

        public ElHefe()
        {
            TextureMapping = new TextureMapping();
            TextureMapping.Add("Icon", "artifacts_elhefe.png");
        }
    }
}
