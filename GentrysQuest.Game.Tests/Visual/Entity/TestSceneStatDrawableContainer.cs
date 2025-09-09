using GentrysQuest.Game.Entity.Drawables;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Tests.Visual.Entity
{
    [TestFixture]
    public partial class TestSceneStatDrawableContainer : GentrysQuestTestScene
    {
        private StatDrawableContainer container;

        public TestSceneStatDrawableContainer()
        {
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Gray
            });
            Add(container = new StatDrawableContainer());
            // container.AddStat(new StatDrawable("Main stat", 10, true));
            // container.AddStat(new StatDrawable("Poop", 5, false));
            // container.AddStat(new StatDrawable("Fart", 5, false));
        }

        [Test]
        public void Testing()
        {
            AddStep("Increment all", () =>
            {
                container.GetStatDrawable("Main stat").UpdateValue(1);
                container.GetStatDrawable("Poop").UpdateValue(1);
                container.GetStatDrawable("Fart").UpdateValue(1);
            });
        }
    }
}
