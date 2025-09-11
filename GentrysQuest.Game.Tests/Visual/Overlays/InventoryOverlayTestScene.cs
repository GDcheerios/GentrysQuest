using GentrysQuest.Game.Content;
using GentrysQuest.Game.Content.Characters;
using GentrysQuest.Game.Content.Families;
using GentrysQuest.Game.Content.Families.BraydenMesserschmidt;
using GentrysQuest.Game.Content.Families.Intro;
using GentrysQuest.Game.Content.Families.JVee;
using GentrysQuest.Game.Content.Weapons;
using GentrysQuest.Game.Overlays.Inventory;
using GentrysQuest.Game.Users;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;

namespace GentrysQuest.Game.Tests.Visual.Overlays
{
    [TestFixture]
    public partial class InventoryOverlayTestScene : GentrysQuestTestScene
    {
        private InventoryOverlay inventoryOverlay;

        [Cached]
        private Bindable<IUser> user = new();

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
            AddStep("Create user", () => { user.Value = GuestUser.Create("test", true); });
            AddStep("Give infinite money", () =>
            {
                user.Value.MoneyHandler.InfiniteMoney = true;
            });
            AddStep("Create Inventory", () => { Add(inventoryOverlay = new InventoryOverlay()); });
            AddStep("Link User", () => inventoryOverlay.ProvideUser(user.Value));
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
            AddStep("Add Artifacts", () =>
            {
                user.Value.AddItem(new TestArtifact());
                user.Value.AddItem(new TestArtifact());
                user.Value.AddItem(new TestArtifact());
                user.Value.AddItem(new TestArtifact());
                user.Value.AddItem(new TestArtifact());
                user.Value.AddItem(new ElHefe());
                user.Value.AddItem(new Keyboard());
                user.Value.AddItem(new MadokaChibiPlush());
                user.Value.AddItem(new OsuTablet());
            });
            AddStep("Add Weapons", () =>
            {
                user.Value.AddItem(new Sword());
                user.Value.AddItem(new Bow());
                user.Value.AddItem(new BraydensOsuPen());
                user.Value.AddItem(new BrodysBroadsword());
                user.Value.AddItem(new Spear());
                user.Value.AddItem(new Hammer());
            });
            AddStep("Add Characters", () =>
            {
                user.Value.AddItem(new TestCharacter(1));
                user.Value.AddItem(new TestCharacter(2));
                user.Value.AddItem(new TestCharacter(3));
                user.Value.AddItem(new TestCharacter(4));
                user.Value.AddItem(new TestCharacter(5));
                user.Value.AddItem(new BraydenMesserschmidt());
                user.Value.AddItem(new MekhiElliot());
                user.Value.AddItem(new GMoney());
                user.Value.AddItem(new PhilipMcClure());
            });
        }
    }
}
