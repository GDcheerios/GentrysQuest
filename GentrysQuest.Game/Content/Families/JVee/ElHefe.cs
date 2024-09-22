using System.Collections.Generic;
using GentrysQuest.Game.Entity;

namespace GentrysQuest.Game.Content.Families.JVee;

public class ElHefe : Artifact
{
    public override string Name { get; set; } = "ElHefe";
    public override string Description { get; protected set; } = "The customer service plant named ElHefe";
    public override Family family { get; protected set; } = new JVeeFamily();
    public override List<StatType> ValidMainAttributes { get; set; } = [StatType.Health];

    public ElHefe()
    {
        TextureMapping.Add("Icon", "artifacts_elhefe.png");
    }
}
