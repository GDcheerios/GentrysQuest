using GentrysQuest.Game.Content.Music;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using AudioManager = GentrysQuest.Game.Audio.AudioManager;

namespace GentrysQuest.Game.Screens
{
    public partial class InventoryTutorial : GqScreen
    {
        private SceneScript cinematicScene = new();

        private Box flashOverlay = new Box
        {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.White,
        };

        public InventoryTutorial()
        {
            cinematicScene.AddEvent("fade flash", new SceneEvent
            {
                Event = () =>
                {
                    flashOverlay.FadeOut(3000, Easing.OutQuint);
                    AudioManager.Instance.ChangeMusic(new ESong());
                },
                Delay = 2000,
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(flashOverlay);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            cinematicScene.Start(Overlay, Scheduler);
        }
    }
}
