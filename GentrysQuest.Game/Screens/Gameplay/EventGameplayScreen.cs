using System.Threading.Tasks;
using GentrysQuest.Game.Overlays;
using GentrysQuest.Game.Overlays.GameMenu;
using GentrysQuest.Game.Users;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Screens;

namespace GentrysQuest.Game.Screens.Gameplay
{
    public partial class EventGameplayScreen : GameplayScreen
    {
        public EventGameplayScreen(int? leaderboardId = null) => ID = leaderboardId;

        [Resolved]
        private Bindable<IUser> currentUser { get; set; }

        [Resolved]
        private GameMenuOverlay gameMenuOverlay { get; set; }

        [Resolved]
        private ScreenManager screenManager { get; set; }

        protected override UserSessionMode SessionMode => UserSessionMode.Event;

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            Schedule(() => gameMenuOverlay.Disappear());
        }

        protected override Task OnEnding(GameplayEndReason reason) => base.OnEnding(reason);

        protected override void OnEnded(GameplayEndReason reason)
        {
            gameMenuOverlay.SetState(SelectionState.Event);
            base.OnEnded(reason);
        }

        public override void OnSuspending(ScreenTransitionEvent e)
        {
            if (e.Next is EventGameplayScreen)
            {
                base.OnSuspending(e);
                return;
            }

            IUser modeUser = User ?? currentUser?.Value;
            EventSessionInventoryScope.End(modeUser);

            base.OnSuspending(e);
        }
    }
}
