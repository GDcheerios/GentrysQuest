using System.Linq;
using GentrysQuest.Game.Quests;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace GentrysQuest.Game.Overlays
{
    public partial class QuestOverlay : CompositeDrawable
    {
        private FillFlowContainer<VisualQuestItem> questFlow;

        public QuestOverlay()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChild = questFlow = new FillFlowContainer<VisualQuestItem>
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(10),
                Padding = new MarginPadding(20)
            };
        }

        public void Load()
        {
            QuestManager.OnQuestStarted += addQuestToUI;
            QuestManager.OnQuestCompleted += removeQuestFromUI;

            foreach (var quest in QuestManager.GetActiveQuests())
                addQuestToUI(quest);
        }

        private void addQuestToUI(Quest quest)
        {
            if (questFlow.Any(i => i.Quest == quest)) return;

            var visualItem = new VisualQuestItem(quest);
            questFlow.Add(visualItem);

            visualItem.MoveToX(100).MoveToX(0, 500, Easing.OutQuint)
                      .FadeInFromZero(500);
        }

        private void removeQuestFromUI(Quest quest)
        {
            var item = questFlow.FirstOrDefault(i => i.Quest == quest);
            if (item == null) return;

            item.FadeOut(500, Easing.InQuint).Expire();
        }

        private void refreshUI(Quest quest)
        {
            var item = questFlow.FirstOrDefault(i => i.Quest == quest);
            item?.UpdateDisplay();
        }

        private partial class VisualQuestItem : Container
        {
            public readonly Quest Quest;
            private SpriteText titleText;
            private FillFlowContainer objectivesFlow;

            public VisualQuestItem(Quest quest)
            {
                Quest = quest;
                AutoSizeAxes = Axes.Both;

                Children =
                [
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black,
                        Alpha = 0.4f,
                    },
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Padding = new MarginPadding(10),
                        Children =
                        [
                            titleText = new SpriteText
                            {
                                Text = quest.Title,
                                Font = FontUsage.Default.With(size: 24, weight: "Bold"),
                                Colour = Colour4.Gold
                            },
                            objectivesFlow = new FillFlowContainer
                            {
                                AutoSizeAxes = Axes.Both,
                                Direction = FillDirection.Vertical,
                                Margin = new MarginPadding { Top = 5 }
                            }
                        ]
                    }
                ];

                Quest.QuestUpdated += _ => UpdateDisplay();
                UpdateDisplay();
            }

            public void UpdateDisplay()
            {
                objectivesFlow.Clear();

                foreach (var objective in Quest.Objectives.Where(o => !o.Hidden))
                {
                    objectivesFlow.Add(new SpriteText
                    {
                        Text = $"• {objective.Name}: {(objective.Completed ? "Done" : $"{objective.CurrentValue}/{objective.TargetValue}")}",
                        Font = FontUsage.Default.With(size: 18),
                        Colour = objective.Completed ? Colour4.LightGreen : Colour4.White
                    });
                }
            }
        }
    }
}
