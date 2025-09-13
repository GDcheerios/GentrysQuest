using GentrysQuest.Game.Content.Families;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using NUnit.Framework;

namespace GentrysQuest.Game.Tests.Visual.Entity
{
    [TestFixture]
    public partial class TestSceneArtifact : GentrysQuestTestScene
    {
        private Artifact artifact;
        private ArtifactInfoDrawable artifactInfoDrawable;
        private StatDrawableContainer statDrawableContainer;

        public TestSceneArtifact()
        {
            Add(statDrawableContainer = new StatDrawableContainer()
            {
                Y = 100
            });

            AddStep("Create artifact", () =>
            {
                artifact = new TestArtifact();
                updateInfo();
            });
            AddStep("Level Up", () =>
            {
                artifact.LevelUp();
                updateInfo();
            });
            AddStep("Add buff", () =>
            {
                artifact.AddBuff();
                updateInfo();
            });
        }

        private void updateInfo()
        {
            if (artifactInfoDrawable != null) Remove(artifactInfoDrawable, true);
            artifactInfoDrawable = new ArtifactInfoDrawable(artifact);
            Add(artifactInfoDrawable);
            statDrawableContainer.Clear();
            // statDrawableContainer.AddStat(new StatDrawable(artifact.MainAttribute, true));
            // foreach (Buff buff in artifact.Attributes) statDrawableContainer.AddStat(new StatDrawable(buff, false));
        }
    }
}
