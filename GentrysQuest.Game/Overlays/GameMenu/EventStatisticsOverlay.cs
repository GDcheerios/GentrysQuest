using System;
using System.Collections.Generic;
using System.Globalization;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Online.API.Requests.Leaderboard;
using GentrysQuest.Game.Online.API.Requests.Responses;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;
using osuTK;

namespace GentrysQuest.Game.Overlays.GameMenu
{
    public partial class EventStatisticsOverlay : CompositeDrawable
    {
        private enum StatisticsView
        {
            Overview,
            MyStats,
            LastRun
        }

        private static readonly Colour4 panelBackground = new Colour4(28, 28, 28, 255);
        private static readonly Colour4 cardBackground = new Colour4(42, 42, 42, 255);
        private static readonly Colour4 outlineColour = new Colour4(157, 157, 157, 255);
        private static readonly Colour4 valueColour = new Colour4(230, 230, 230, 255);

        private readonly MainGqButton overviewButton;
        private readonly MainGqButton myStatsButton;
        private readonly MainGqButton lastRunButton;
        private readonly FillFlowContainer<Drawable> contentFlow;
        private readonly LoadingIndicator loadingIndicator;

        private int loadVersion;
        private StatisticsView activeView = StatisticsView.Overview;
        private EventStatisticsResponse statisticsResponse;

        public EventStatisticsOverlay()
        {
            const float tabsY = 55;
            const float tabsHeight = 50;
            const float contentTopPadding = tabsY + tabsHeight + 12;

            RelativeSizeAxes = Axes.Both;

            InternalChildren =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(0, 0, 0, 185)
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 12,
                    CornerExponent = 2,
                    BorderThickness = 2,
                    BorderColour = Colour4.Black,
                    Children =
                    [
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = panelBackground
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(16),
                            Children =
                            [
                                new GqText("Event Statistics")
                                {
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    Font = FontUsage.Default.With(size: 30, weight: "Bold")
                                },
                                new FillFlowContainer
                                {
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    Direction = FillDirection.Horizontal,
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Y = tabsY,
                                    Spacing = new Vector2(8, 0),
                                    Children =
                                    [
                                        overviewButton = createTabButton("Overview", StatisticsView.Overview),
                                        myStatsButton = createTabButton("My Stats", StatisticsView.MyStats),
                                        lastRunButton = createTabButton("Last Run", StatisticsView.LastRun)
                                    ]
                                },
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Padding = new MarginPadding { Top = contentTopPadding },
                                    Child = new BasicScrollContainer
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        ScrollbarVisible = true,
                                        Child = contentFlow = new FillFlowContainer<Drawable>
                                        {
                                            Direction = FillDirection.Vertical,
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Spacing = new Vector2(0, 10)
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                loadingIndicator = new LoadingIndicator("Loading statistics...")
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            ];

            loadingIndicator.Hide();
            setActiveView(StatisticsView.Overview);
        }

        private MainGqButton createTabButton(string text, StatisticsView view)
        {
            var button = new MainGqButton(text)
            {
                Size = new Vector2(120, 50)
            };

            button.SetAction(() => setActiveView(view));
            return button;
        }

        public async void Load(int leaderboardId)
        {
            int requestVersion = ++loadVersion;
            loadingIndicator.FadeIn(100);

            try
            {
                var request = new GetStatisticsRequest(leaderboardId: leaderboardId);
                await request.PerformAsync();

                if (requestVersion != loadVersion)
                    return;

                statisticsResponse = request.Response;
                renderCurrentView();
            }
            catch (Exception ex)
            {
                Logger.Log($"Statistics fetch failed: {ex.Message}", LoggingTarget.Network, LogLevel.Important);
            }
            finally
            {
                if (requestVersion == loadVersion)
                    loadingIndicator.FadeOut(100);
            }
        }

        private void setActiveView(StatisticsView view)
        {
            activeView = view;
            updateButtonStates();
            renderCurrentView();
        }

        private void updateButtonStates()
        {
            overviewButton.Alpha = activeView == StatisticsView.Overview ? 1f : 0.65f;
            myStatsButton.Alpha = activeView == StatisticsView.MyStats ? 1f : 0.65f;
            lastRunButton.Alpha = activeView == StatisticsView.LastRun ? 1f : 0.65f;
        }

        private void renderCurrentView()
        {
            contentFlow.Clear();

            if (statisticsResponse == null)
            {
                contentFlow.Add(createInfoCard("No statistics loaded yet."));
                return;
            }

            switch (activeView)
            {
                case StatisticsView.Overview:
                    renderOverview();
                    break;

                case StatisticsView.MyStats:
                    renderMyStats();
                    break;

                case StatisticsView.LastRun:
                    renderLastRun();
                    break;
            }
        }

        private void renderOverview()
        {
            var leaderboard = statisticsResponse.Leaderboard;

            if (leaderboard == null)
            {
                addMissingDataText();
                return;
            }

            contentFlow.AddRange(
            [
                createKpiCard("Total Players", formatWholeNumber(leaderboard.TotalPlayers)),
                createKpiCard("Total Plays", formatOptionalWholeNumber(resolveTotalPlays(leaderboard))),
                createKpiCard("Total Score", formatWholeNumber(leaderboard.TotalScore)),
                createKpiCard("Average Score", formatOptionalNumber(leaderboard.AverageScore, "N0"))
            ]);

            var leaderboardStats = leaderboard.Statistics;

            if (leaderboardStats == null)
            {
                contentFlow.Add(createInfoCard("Event stat totals are unavailable."));
                return;
            }

            addByTypeTable(leaderboardStats.ByType, "No event stat types recorded.");
        }

        private void renderMyStats()
        {
            var user = statisticsResponse.User;
            var scoreSummary = user?.Scores;
            var stats = user?.Statistics;

            contentFlow.AddRange(
            [
                createKpiCard("My Total Plays", formatOptionalWholeNumber(scoreSummary?.TotalPlays)),
                createKpiCard("My Total Score", formatOptionalWholeNumber(scoreSummary?.TotalScore)),
                createKpiCard("My Avg Score", formatOptionalNumber(scoreSummary?.AverageScore, "N0"))
            ]);

            if (!scoreSummary?.TotalPlays.HasValue ?? true)
                contentFlow.Insert(0, createKpiCard("My Total Plays", formatOptionalWholeNumber(stats?.TotalAmount)));

            if (stats == null)
            {
                contentFlow.Add(createInfoCard("Personal stat totals are unavailable."));
                return;
            }

            addByTypeTable(stats.ByType, "No personal stat types recorded.");
        }

        private void renderLastRun()
        {
            var lastRun = statisticsResponse.User?.LastRun;

            if (lastRun == null)
            {
                contentFlow.Add(createInfoCard("No last run data available."));
                return;
            }

            contentFlow.AddRange(
            [
                createKpiCard("Last Run Score", formatOptionalWholeNumber(lastRun.Score)),
            ]);

            addByTypeTotalOnlyTable(lastRun.Statistics?.ByType, "No last run stat types recorded.");
        }

        private void addByTypeTable(IReadOnlyList<StatisticTypeSummaryResponse> byType, string emptyMessage)
        {
            if (byType == null || byType.Count == 0)
            {
                contentFlow.Add(createInfoCard(emptyMessage));
                return;
            }

            contentFlow.Add(new GqText("By Type")
            {
                Font = FontUsage.Default.With(size: 20, weight: "Bold"),
                Margin = new MarginPadding { Top = 4 }
            });

            contentFlow.Add(createTypeRow("Type", "Total", "Avg", true));

            foreach (var stat in byType)
            {
                if (stat == null)
                    continue;

                contentFlow.Add(createTypeRow(
                    normalizeType(stat.Type),
                    formatWholeNumber(stat.TotalAmount),
                    formatOptionalNumber(stat.AverageAmount, "N2"),
                    false));
            }
        }

        private void addByTypeTotalOnlyTable(IReadOnlyList<StatisticTypeSummaryResponse> byType, string emptyMessage)
        {
            if (byType == null || byType.Count == 0)
            {
                contentFlow.Add(createInfoCard(emptyMessage));
                return;
            }

            contentFlow.Add(new GqText("By Type")
            {
                Font = FontUsage.Default.With(size: 20, weight: "Bold"),
                Margin = new MarginPadding { Top = 4 }
            });

            contentFlow.Add(createTypeTotalRow("Type", "Total", true));

            foreach (var stat in byType)
            {
                if (stat == null)
                    continue;

                contentFlow.Add(createTypeTotalRow(
                    normalizeType(stat.Type),
                    formatWholeNumber(stat.TotalAmount),
                    false));
            }
        }

        private void addMissingDataText() => contentFlow.Add(createInfoCard("Statistics were returned without data."));

        private static Drawable createInfoCard(string message)
        {
            return new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Masking = true,
                CornerRadius = 10,
                CornerExponent = 2,
                BorderThickness = 1,
                BorderColour = Colour4.Black,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = cardBackground
                    },
                    new GqText(message)
                    {
                        Font = FontUsage.Default.With(size: 17),
                        Padding = new MarginPadding(12)
                    }
                ]
            };
        }

        private static Drawable createKpiCard(string label, string value)
        {
            return new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Masking = true,
                CornerRadius = 10,
                CornerExponent = 2,
                BorderThickness = 1,
                BorderColour = Colour4.Black,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = cardBackground
                    },
                    new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 2,
                        Anchor = Anchor.TopLeft,
                        Origin = Anchor.TopLeft,
                        Colour = outlineColour
                    },
                    new FillFlowContainer
                    {
                        Direction = FillDirection.Vertical,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Padding = new MarginPadding { Left = 12, Right = 12, Top = 10, Bottom = 10 },
                        Spacing = new Vector2(0, 2),
                        Children =
                        [
                            new GqText(label)
                            {
                                Font = FontUsage.Default.With(size: 15),
                                Alpha = 0.9f
                            },
                            new GqText(value)
                            {
                                Font = FontUsage.Default.With(size: 28, weight: "Bold"),
                                Colour = valueColour
                            }
                        ]
                    }
                ]
            };
        }

        private static Drawable createTypeRow(string type, string total, string avg, bool header)
        {
            return new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Masking = true,
                CornerRadius = 8,
                CornerExponent = 2,
                BorderThickness = 1,
                BorderColour = Colour4.Black,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = header ? new Colour4(60, 60, 60, 255) : cardBackground
                    },
                    new FillFlowContainer
                    {
                        Direction = FillDirection.Horizontal,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Children =
                        [
                            createCellText(type, header, 0.50f, Anchor.CentreLeft),
                            createCellText(total, header, 0.25f, Anchor.CentreRight),
                            createCellText(avg, header, 0.25f, Anchor.CentreRight)
                        ]
                    }
                ]
            };
        }

        private static Drawable createTypeTotalRow(string type, string total, bool header)
        {
            return new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Masking = true,
                CornerRadius = 8,
                CornerExponent = 2,
                BorderThickness = 1,
                BorderColour = Colour4.Black,
                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = header ? new Colour4(60, 60, 60, 255) : cardBackground
                    },
                    new FillFlowContainer
                    {
                        Direction = FillDirection.Horizontal,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Children =
                        [
                            createCellText(type, header, 0.70f, Anchor.CentreLeft),
                            createCellText(total, header, 0.30f, Anchor.CentreRight)
                        ]
                    }
                ]
            };
        }

        private static Drawable createCellText(string text, bool header, float width, Anchor anchor)
        {
            return new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Width = width,
                Padding = new MarginPadding { Left = 10, Right = 10, Top = 8, Bottom = 8 },
                Child = new GqText(text)
                {
                    Anchor = anchor,
                    Origin = anchor,
                    Font = FontUsage.Default.With(size: header ? 16 : 15, weight: header ? "Bold" : "Regular"),
                    Colour = header ? Colour4.White : valueColour
                }
            };
        }

        private static long? resolveTotalPlays(LeaderboardStatisticsResponse leaderboard)
        {
            if (leaderboard.TotalPlays.HasValue)
                return leaderboard.TotalPlays.Value;

            if (leaderboard.Statistics != null)
                return leaderboard.Statistics.TotalAmount;

            if (!leaderboard.AverageScore.HasValue || leaderboard.AverageScore.Value <= 0)
                return null;

            return (long)Math.Round(leaderboard.TotalScore / leaderboard.AverageScore.Value, MidpointRounding.AwayFromZero);
        }

        private static string formatWholeNumber(long value) =>
            value.ToString("N0", CultureInfo.InvariantCulture);

        private static string formatOptionalWholeNumber(long? value) =>
            value.HasValue ? value.Value.ToString("N0", CultureInfo.InvariantCulture) : "N/A";

        private static string formatOptionalNumber(double? value, string format) =>
            value.HasValue ? value.Value.ToString(format, CultureInfo.InvariantCulture) : "N/A";

        private static string normalizeType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return "Unknown";

            return type.Trim().Replace('_', ' ');
        }
    }
}
