using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;

namespace GentrysQuest.Game.Content.Artifacts
{
    public class TestArtifact : Artifact
    {
        public TestArtifact()
            : base()
        {
            Name = "Test Artifact";
            TextureMapping = new TextureMapping();
        }
    }
}
