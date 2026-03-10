using GentrysQuest.Game.Content.Artifacts;
using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Drawables;
using GentrysQuest.Game.Entity.Weapon;
using NUnit.Framework;
using osuTK;

namespace GentrysQuest.Game.Tests.Visual.Entity
{
    [TestFixture]
    public partial class TestSceneEntityInfoDrawable : GentrysQuestTestScene
    {
        private EntityInfoDrawable entityInfoDrawable;
        private ArtifactInfoDrawable artifactInfoDrawable;
        private WeaponInfoDrawable weaponInfoDrawable;
        private Game.Entity.Entity testEntity;
        private Weapon testWeapon;
        private Artifact testArtifact;

        public TestSceneEntityInfoDrawable()
        {
            Add(entityInfoDrawable = new EntityInfoDrawable(testEntity = new TestCharacter(1))
            {
                Position = new Vector2(0, 150)
            });
            Add(artifactInfoDrawable = new ArtifactInfoDrawable(testArtifact = new TestArtifact())
            {
                Position = new Vector2(0, 300)
            });
            Add(weaponInfoDrawable = new WeaponInfoDrawable(testWeapon = new Knife()));
            AddSliderStep("Change the star rating", 1, 5, 1, i =>
            {
                entityInfoDrawable.StarRatingContainer.starRating.Value = i;
            });
        }
    }
}
