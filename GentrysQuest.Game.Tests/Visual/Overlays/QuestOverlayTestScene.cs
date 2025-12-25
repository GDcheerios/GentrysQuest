using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Quests;
using osu.Framework.Allocation;

namespace GentrysQuest.Game.Tests.Visual.Overlays
{
    public partial class QuestOverlayTestScene : GentrysQuestTestScene
    {
        private QuestOverlay overlay;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(overlay = new QuestOverlay());
            overlay.Load();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddStep("Add Simple Quest", () =>
            {
                var quest = new Quest { Title = "Test Quest", Description = "A simple test" };
                quest.AddObjective(new Objective { Name = "Objective 1", TargetValue = 1 });
                QuestManager.RegisterQuest(quest);
                QuestManager.AcceptQuest(quest);
            });

            AddStep("Add Multi-stage Quest", () =>
            {
                var quest = new Quest { Title = "Kill Slimes", Description = "Exterminate them!" };
                quest.AddObjective(new Objective { Name = "Slimes killed", TargetValue = 5 });
                quest.AddObjective(new Objective { Name = "Collect goo", TargetValue = 3 });
                QuestManager.RegisterQuest(quest);
                QuestManager.AcceptQuest(quest);
            });

            AddRepeatStep("Progress Slime Kill", () =>
            {
                QuestManager.SignalProgress("Slimes killed", 1);
            }, 5);

            AddStep("Progress Goo", () =>
            {
                QuestManager.SignalProgress("Collect goo", 3);
            });

            AddStep("Complete Simple Quest", () =>
            {
                QuestManager.SignalProgress("Objective 1", 1);
            });

            AddStep("Add Hidden Objective Quest", () =>
            {
                var quest = new Quest { Title = "Secret Business" };
                quest.AddObjective(new Objective { Name = "Visible Task", TargetValue = 1 });
                quest.AddObjective(new Objective { Name = "Secret Task", TargetValue = 1, Hidden = true });
                QuestManager.RegisterQuest(quest);
                QuestManager.AcceptQuest(quest);
            });
        }
    }
}
