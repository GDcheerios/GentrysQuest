using GentrysQuest.Game.Scoring;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Screens.Gameplay.Results
{
    public partial class LeaderboardPanel : Container
    {
        private SpriteText placementSprite;
        private SpriteText usernameSprite;
        private SpriteText scoreSprite;

        public LeaderboardPanel(LeaderboardPlacement placement)
        {
            var colour = ColourInfo.GradientVertical(Colour4.Black, Colour4.Black);
            if (placement.Placement == 1) colour = ColourInfo.GradientVertical(Colour4.White, Colour4.Gold);
            RelativeSizeAxes = Axes.X;
            Margin = new MarginPadding(10);
            Size = new Vector2(0.95f, 100);
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    BorderColour = ColourInfo.GradientVertical(Colour4.Gray, Colour4.Black),
                    Masking = true,
                    CornerExponent = 2,
                    CornerRadius = 12,
                    BorderThickness = 2,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = Colour4.Gray,
                            RelativeSizeAxes = Axes.Both
                        },
                        scoreSprite = new SpriteText
                        {
                            Text = $"{placement.Score} score",
                            Colour = colour,
                            Font = FontUsage.Default.With(size: 36),
                            Margin = new MarginPadding { Right = 4 },
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight
                        },
                        new Box
                        {
                            Colour = ColourInfo.GradientHorizontal(Colour4.Gray, Colour4.Transparent),
                            RelativeSizeAxes = Axes.Both,
                            Width = 1
                        },
                        placementSprite = new SpriteText
                        {
                            Text = $"#{placement.Placement} {placement.Username}",
                            Colour = colour,
                            Font = FontUsage.Default.With(size: 42),
                            Margin = new MarginPadding { Left = 4 },
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft
                        }
                    }
                }
            };
        }
    }
}
