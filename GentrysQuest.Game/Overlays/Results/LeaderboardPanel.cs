using System.Globalization;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Scoring;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Overlays.Results
{
    public partial class LeaderboardPanel : Container
    {
        public readonly LeaderboardPlacement leaderboardPlacement;
        private readonly Colour4 rankColour;
        private readonly bool isScore;

        public LeaderboardPanel(LeaderboardPlacement placement, bool isScore = false)
        {
            leaderboardPlacement = placement;
            this.isScore = isScore;

            rankColour = placement.Rank switch
            {
                "unranked" => new Colour4(8, 43, 59, 255),
                "copper" => new Colour4(255, 6, 6, 255),
                "bronze" => new Colour4(166, 45, 45, 255),
                "silver" => new Colour4(131, 131, 131, 255),
                "gold" => new Colour4(255, 216, 5, 255),
                "platinum" => new Colour4(5, 5, 255, 255),
                "diamond" => new Colour4(1, 255, 255, 255),
                "champion" => new Colour4(128, 0, 128, 255),
                "gentry warrior" => new Colour4(0, 255, 0, 255),
                _ => rankColour
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Size = new Vector2(350, 64);

            Children =
            [
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(0.8f, 1),
                    Masking = true,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Colour4(51, 51, 51, 255),
                    },
                    CornerRadius = 10,
                    CornerExponent = 2,
                    BorderThickness = 2,
                    BorderColour = Colour4.Black,
                    Shear = new Vector2(0.5f, 0),
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Type = EdgeEffectType.Shadow,
                        Colour = new Colour4(0, 0, 0, 200),
                        Radius = 5,
                        Roundness = 1
                    }
                },
                new Container
                {
                    Padding = new MarginPadding { Left = 60 },
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(0.8f, 1),
                    RelativeSizeAxes = Axes.Both,
                    Child = new FillFlowContainer
                    {
                        Direction = FillDirection.Horizontal,
                        RelativeSizeAxes = Axes.Y,
                        AutoSizeAxes = Axes.X,
                        Spacing = new Vector2(10, 0),
                        Height = 1,
                        Children =
                        [
                            new Container
                            {
                                RelativeSizeAxes = Axes.Y,
                                Size = new Vector2(60, 1),
                                Child = new Container
                                {
                                    RelativeSizeAxes = Axes.X,
                                    Size = new Vector2(1, 20),
                                    Masking = true,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    CornerRadius = 10,
                                    CornerExponent = 2,
                                    EdgeEffect = new EdgeEffectParameters
                                    {
                                        Type = EdgeEffectType.Glow,
                                        Colour = rankColour,
                                        Radius = 3,
                                        Roundness = 1
                                    },
                                    Children =
                                    [
                                        new Box
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Colour = rankColour
                                        },
                                        new GqText($"#{leaderboardPlacement.Placement}")
                                        {
                                            Colour = Colour4.White,
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre
                                        }
                                    ]
                                }
                            },
                            new FillFlowContainer
                            {
                                Direction = FillDirection.Vertical,
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                AutoSizeAxes = Axes.X,
                                Height = 48,
                                Children =
                                [
                                    new Container
                                    {
                                        Size = new Vector2(150, 24),
                                        Child = new GqText($"{leaderboardPlacement.Username}")
                                    },
                                    new Container
                                    {
                                        Size = new Vector2(150, 24),
                                        Child = new GqText($"{leaderboardPlacement.Score.ToString("N0", CultureInfo.InvariantCulture)}{(isScore ? " score" : "gp")}")
                                    }
                                ]
                            }
                        ]
                    }
                }
            ];
        }
    }
}
