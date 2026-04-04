using osu.Framework.Allocation;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Screens
{
    public partial class ScreenManager : IDependencyInjectionCandidate
    {
        private ScreenStack stack;

        public ScreenManager(ScreenStack stack) => this.stack = stack;

        public void SetScreen(GqScreen screen)
        {
            if (stack.CurrentScreen != screen) stack.Push(screen);
        }

        public void SetCustomScreen(GqScreen screen) => stack.Push(screen);

        public void ExitCurrentScreen() => stack.Exit();
    }
}
