using System.Collections.Generic;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Overlays.Inventory;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Screens.MainMenu
{
    public partial class GuestSelectionContainer : Container
    {
        private List<GuestSelectionButton> guestSelectionButtons = new();
        private BasicScrollContainer selectionScroll;
        private GQTextBox guestNameInput;
        private InventoryButton createButton;

        public GuestSelectionContainer()
        {
            guestNameInput = new GQTextBox
            {
                Text = "Guest Name",
                RelativeSizeAxes = Axes.X,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Y = -100,
                Size = new Vector2(0.5f, 100f)
            };

            createButton = new InventoryButton("Create Guest")
            {
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Position = new Vector2(-100, -100),
            };

            selectionScroll = new BasicScrollContainer
            {
                RelativeSizeAxes = Axes.Both,

            };

            createButton.SetAction(delegate
            {
                guestSelectionButtons.Clear();
                GuestFileManager.CreateUser(guestNameInput.Text);
                reloadGuests();
            });

            reloadGuests();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children =
            [
                new Container
                {
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(0.8f, 0.75f),
                    Child = selectionScroll
                },
                guestNameInput,
                createButton
            ];
        }

        private void reloadGuests()
        {
            foreach (string guestName in GuestFileManager.GetGuestNames()) guestSelectionButtons.Add(new GuestSelectionButton(guestName));
            selectionScroll.Clear();

            int counter = 0;

            foreach (GuestSelectionButton button in guestSelectionButtons)
            {
                button.Y = counter;
                counter += 105;
                selectionScroll.Add(button);
            }
        }
    }
}
