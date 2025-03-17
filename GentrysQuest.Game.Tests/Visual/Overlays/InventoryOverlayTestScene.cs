using GentrysQuest.Game.Content;
using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Families;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Overlays.Inventory;
using GentrysQuest.Game.Users;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Tests.Visual.Overlays
{
    [TestFixture]
    public partial class InventoryOverlayTestScene : GentrysQuestTestScene
    {
        private InventoryOverlay inventoryOverlay;
        private GuestUser user;

        public InventoryOverlayTestScene()
        {
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = ColourInfo.GradientVertical(Colour4.Black, Colour4.White)
            });
        }

        [Test]
        public void Initialize()
        {
            AddStep("Load Content", ContentManager.LoadContent);
            AddStep("Create user", () => { user = GuestUser.Create("test"); });
            AddStep("Give infinite money", () =>
            {
                user.MoneyHandler.InfiniteMoney = true;
            });
            AddStep("Create Inventory", () => { Add(inventoryOverlay = new InventoryOverlay(user)); });
        }

        [Test]
        public void Display()
        {
            AddStep("Show", () => inventoryOverlay.Show());
            AddStep("Hide", () => inventoryOverlay.Hide());
        }

        [Test]
        public void Collection()
        {
            AddStep("Add Artifact", () => user.AddItem(new TestArtifact()));
            AddStep("Add Weapon", () => user.AddItem(new Sword()));
            AddStep("Add Character", () => user.AddItem(new TestCharacter(1)));
        }
    }
}
