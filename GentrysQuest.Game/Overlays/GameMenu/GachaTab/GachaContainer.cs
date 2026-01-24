using System.Collections.Generic;
using GentrysQuest.Game.Content.Gachas;
using GentrysQuest.Game.Gachas;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Overlays.GameMenu.GachaTab
{
    public partial class GachaContainer : Container
    {
        [Resolved]
        private Bindable<IUser> user { get; set; }

        private Container leftContainer;
        private Container rightContainer;

        /// <summary>
        /// The button used to start the roll process
        /// </summary>
        private GachaRollButton rollButton;

        private readonly BasicScrollContainer<GachaButton> gachaButtonList = new()
        {
            RelativeSizeAxes = Axes.Both
        };

        private readonly List<Gacha> gachas =
        [
            new StarterGacha()
        ];

        public GachaContainer()
        {
            foreach (Gacha gacha in gachas)
            {
                gachaButtonList.Add(new GachaButton(gacha, () => LoadGacha(gacha))
                {
                    RelativeSizeAxes = Axes.X,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Width = 0.95f,
                    Height = 50
                });
            }
        }

        public void LoadGacha(Gacha gacha)
        {
        }

        public void AnimateShow()
        {
            leftContainer.ResizeHeightTo(0.8f, 200, Easing.OutQuint);
            rightContainer.ResizeHeightTo(0.8f, 200, Easing.OutQuint);
        }

        public void AnimateHide()
        {
            leftContainer.ResizeHeightTo(0, 200, Easing.OutQuint);
            rightContainer.ResizeHeightTo(0, 200, Easing.OutQuint);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Masking = true;
            RelativeSizeAxes = Axes.Both;
            Children =
            [
                leftContainer = new Container
                {
                    Masking = true,
                    CornerRadius = 10,
                    CornerExponent = 2,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.3f, 0.8f),
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Gray,
                        },
                        gachaButtonList
                    ]
                },
                rightContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 10,
                    CornerExponent = 2,
                    Size = new Vector2(0.6f, 0.8f),
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Gray,
                        },
                        rollButton = new GachaRollButton
                        {
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            Size = new Vector2(200, 50),
                            Margin = new MarginPadding { Bottom = 25 }
                        }
                    ]
                }
            ];
        }
    }
}
