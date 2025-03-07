using System.Collections.Generic;
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
        private BasicScrollContainer scrollContainer;

        public readonly FillFlowContainer<LeaderboardPanel> LeaderboardPanels = new();
        public List<LeaderboardPlacement> Placements = new();

        protected SpriteText ScoreText;

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
                            Child = ScoreText = new SpriteText
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Text = $"0 Score", //TODO: rewrite
                                Colour = Colour4.Black,
                                Font = FontUsage.Default.With(size: 42)
                            },
                        },
                        scrollContainer = new BasicScrollContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 500,
                            Child = LeaderboardPanels = new FillFlowContainer<LeaderboardPanel>
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
            LeaderboardPanels.Add(panel);
            panel.Scale = new Vector2(0, 1);
            panel.ScaleTo(new Vector2(1, 1), 100);
        }
    }
}
