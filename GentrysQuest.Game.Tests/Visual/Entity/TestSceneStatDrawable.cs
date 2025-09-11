using GentrysQuest.Game.Entity.Drawables;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Tests.Visual.Entity
{
    [TestFixture]
    public partial class TestSceneStatDrawable : GentrysQuestTestScene
    {
        private float stat1 = 10;
        private float stat2 = 5;
        private StatDrawable statDrawable1;
        private StatDrawable statDrawable2;

        public TestSceneStatDrawable()
        {
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Gray
            });
            Add(statDrawable1 = new StatDrawable("Poop Stat")
            {
                Position = new Vector2(0),
                Size = new Vector2(600)
            });
            Add(statDrawable2 = new StatDrawable("Minor Poop Stat")
            {
                Position = new Vector2(0, 26),
                Size = new Vector2(600, 200)
            });
        }

        [Test]
        public void StatTesting()
        {
            AddStep("IncrementTheThings", () =>
            {
                stat1 += 1;
                stat2 += 0.5f;
                statDrawable1.Value.Value = stat1;
                statDrawable2.Value.Value = stat2;
            });
        }
    }
}
