using System.Collections.Generic;
using GentrysQuest.Game.Database;
using GentrysQuest.Game.Scoring;
using GentrysQuest.Game.Screens.Gameplay.Results;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Overlays.Results
{
    public partial class ResultsLeaderboard : CompositeDrawable
    {
        private FillFlowContainer<LeaderboardPanel> leaderboardPanels = new();
        public List<LeaderboardPlacement> Placements = new();

        public ResultsLeaderboard()
        {
            InternalChildren = new Drawable[]
            {
                new FillFlowContainer
                {
                    Spacing = new Vector2(0, 20),
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            RelativeSizeAxes = Axes.X,
                            Height = 80,
                            Child = new SpriteText
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Text = $"{(int)GameData.CurrentStats.GetStat(StatTypes.Score).Value} Score",
                                Colour = Colour4.Black,
                                Font = FontUsage.Default.With(size: 42)
                            }
                        },
                        new BasicScrollContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 1000,
                            Child = leaderboardPanels = new FillFlowContainer<LeaderboardPanel>
                            {
                                Direction = FillDirection.Vertical,
                                AutoSizeAxes = Axes.Y,
                                RelativeSizeAxes = Axes.X
                            }
                        }
                    }
                }
            };
        }

        public void AddListing(LeaderboardPlacement placement)
        {
            LeaderboardPanel panel = new LeaderboardPanel(placement);
            Placements.Add(placement);
            leaderboardPanels.Add(panel);
            panel.Scale = new Vector2(0, 1);
            panel.ScaleTo(new Vector2(1, 1), 100);
        }
    }
}
