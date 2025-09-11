using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Tests.Visual.Entity
{
    [TestFixture]
    public partial class TestSceneArtifactStats : GentrysQuestTestScene
    {
        private int level = 1;
        private int rating = 1;
        private bool isPercent = false;

        private StatDrawableContainer statContainer = new StatDrawableContainer
        {
            RelativeSizeAxes = Axes.Both
        };

        public TestSceneArtifactStats()
        {
            Add(statContainer);
            AddStep("ok!", () =>
            {
                Logger.Log("I'm ready!");
            });
            AddStep("Reset", resetDisplay);
            AddToggleStep("Is percent", b => toggleBool());
            AddSliderStep("Star Rating", 1, 5, 1, setStarRating);
            AddSliderStep("Level", 1, 20, 1, setLevel);

            statContainer.AddStat(new StatDrawable(getBuff(StatType.Health)));
            statContainer.AddStat(new StatDrawable(getBuff(StatType.Attack)));
            statContainer.AddStat(new StatDrawable(getBuff(StatType.Defense)));
            statContainer.AddStat(new StatDrawable(getBuff(StatType.CritRate)));
            statContainer.AddStat(new StatDrawable(getBuff(StatType.CritDamage)));
            statContainer.AddStat(new StatDrawable(getBuff(StatType.Speed)));
            statContainer.AddStat(new StatDrawable(getBuff(StatType.AttackSpeed)));
            statContainer.AddStat(new StatDrawable(getBuff(StatType.RegenSpeed)));
            statContainer.AddStat(new StatDrawable(getBuff(StatType.RegenStrength)));
            statContainer.AddStat(new StatDrawable(getBuff(StatType.Tenacity)));
        }

        private void resetDisplay()
        {
            foreach (StatDrawable statDrawable in statContainer.GetStatDrawables())
            {
                statContainer.GetStatDrawable(statDrawable.Identifier).Value.Value = 0;
            }
        }

        private Buff getBuff(StatType buffType)
        {
            Artifact artifact = new Artifact();
            artifact.StarRating.Value = rating;
            artifact.Experience.Level.Current.Value = level;
            Buff buff = new Buff(artifact, buffType, isPercent);

            for (int i = 0; i < level; i++)
            {
                buff.Improve();
            }

            return buff;
        }

        private void toggleBool()
        {
            isPercent = !isPercent;
            resetDisplay();
        }

        private void setLevel(int level)
        {
            this.level = level;
            resetDisplay();
        }

        private void setStarRating(int rating)
        {
            this.rating = rating;
            resetDisplay();
        }
    }
}
