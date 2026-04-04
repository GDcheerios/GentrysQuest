using System.Collections.Generic;
using GentrysQuest.Game.Scoring;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Overlays.Results
{
    public partial class ResultsLeaderboard : CompositeDrawable
    {
        private BasicScrollContainer scrollContainer;

        public readonly FillFlowContainer<LeaderboardPanel> LeaderboardPanels = new();
        public List<LeaderboardPlacement> Placements = new();

        protected SpriteText ScoreText;
        public bool ScoreLeaderboard { get; set; } = true;

        public ResultsLeaderboard()
        {
            InternalChildren =
            [
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children =
                    [
                        scrollContainer = new BasicScrollContainer
                        {
                            ScrollbarVisible = false,
                            RelativeSizeAxes = Axes.Both,
                            Child = LeaderboardPanels = new FillFlowContainer<LeaderboardPanel>
                            {
                                Direction = FillDirection.Vertical,
                                AutoSizeAxes = Axes.Y,
                                Spacing = new Vector2(0, 10),
                                RelativeSizeAxes = Axes.X
                            }
                        }
                    ]
                }
            ];
        }

        public void AddListing(LeaderboardPlacement placement)
        {
            LeaderboardPanel panel = new LeaderboardPanel(placement, ScoreLeaderboard);
            Placements.Add(placement);
            LeaderboardPanels.Add(panel);
            panel.FadeInFromZero(100);
            panel.RelativeSizeAxes = Axes.X;
            panel.Width = 0.9f;
        }
    }
}
