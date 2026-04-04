using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Graphics.TextStyles
{
    public partial class LocationText : TitleText
    {
        private const int FADE_TIME = 1500;

        public LocationText(string location)
            : base(location)
        {
            // man what?
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Alpha = 0;
            Colour = Colour4.White;
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            this.Delay(FADE_TIME).Then()
                .FadeIn(FADE_TIME).Then()
                .Delay(FADE_TIME).Then()
                .FadeOut(FADE_TIME).Then();
        }
    }
}
