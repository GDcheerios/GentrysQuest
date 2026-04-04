using System.Collections.Generic;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.IO;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GentrysQuest.Game.Overlays.PlayerSelect
{
    public partial class GuestSelectionContainer : Container
    {
        private readonly List<GuestSelectionButton> guestSelectionButtons = new();
        private readonly BasicScrollContainer selectionScroll;
        private readonly GqTextBox guestNameInput;
        private readonly PlayerSelectButton createButton;

        public GuestSelectionContainer()
        {
            guestNameInput = new GqTextBox
            {
                Text = "Guest Name",
                RelativeSizeAxes = Axes.X,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.TopCentre,
                LengthLimit = 10,
                Y = -250,
                Size = new Vector2(0.9f, 50)
            };

            createButton = new PlayerSelectButton("Create Guest")
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.X,
                Y = -100,
                Size = new Vector2(0.8f, 50),
            };

            selectionScroll = new BasicScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1f, 0.6f),
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
            };

            createButton.SetAction(delegate
            {
                guestSelectionButtons.Clear();
                GuestUser.Create(guestNameInput.Text);
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
                counter += 65;
                selectionScroll.Add(button);
            }
        }
    }
}
