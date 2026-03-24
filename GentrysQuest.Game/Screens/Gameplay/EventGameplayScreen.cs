using GentrysQuest.Game.Overlays;
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

        protected override UserSessionMode SessionMode => UserSessionMode.Event;

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            gameMenuOverlay.Disappear();
        }

        public override void OnSuspending(ScreenTransitionEvent e)
        {
            IUser modeUser = User ?? currentUser?.Value;
            if (modeUser != null)
                modeUser.SessionMode = UserSessionMode.Normal;

            base.OnSuspending(e);
        }

        public void End()
        {
            // GameData.UnStore();
            // base.End();
        }
    }
}
