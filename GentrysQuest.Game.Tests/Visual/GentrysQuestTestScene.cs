using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Graphics;
using GentrysQuest.Game.Online;
using osu.Framework.Allocation;
using osu.Framework.Testing;

namespace GentrysQuest.Game.Tests.Visual
{
    public partial class GentrysQuestTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new GentrysQuestTestSceneTestRunner();

        [Resolved]
        private DiscordRpc discordRpc { get; set; }

        protected virtual string TestName { get; init; }

        private partial class GentrysQuestTestSceneTestRunner : GentrysQuestGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new GqBackground());
            discordRpc.UpdatePresence(TestName, "Testing");
        }

        public GentrysQuestTestScene()
        {
            HitBoxScene.Clear();
        }
    }
}
