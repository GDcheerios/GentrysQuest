using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;

namespace GentrysQuest.Game.Content.Artifacts
{
    public class MadokaChibiPlush : Artifact
    {
        public override string Name { get; set; } = "Madoka Chibi Plush";
        public override string Description { get; protected set; } = "Brayden's trusty old Madoka Chibi Plush";

        public MadokaChibiPlush()
        {
            TextureMapping = new TextureMapping();
            TextureMapping.Add("Icon", "artifacts_brayden_messerschmidt_madoka_chibi_plush.jpg");
        }
    }
}
