using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Graphics
{
    public partial class LoadingIndicator : CompositeDrawable
    {
        private SpriteIcon icon;
        private SpriteText text;

        public LoadingIndicator(string text = "Loading...")
        {
            Size = new Vector2(84);
            AddInternal(icon = new SpriteIcon
            {
                Icon = FontAwesome.Solid.Grin,
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre
            });
            AddInternal(this.text = new SpriteText
            {
                Text = text,
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.TopCentre,
                Font = FontUsage.Default.With(size: 32),
                AllowMultiline = false
            });
        }

        protected override void Update()
        {
            icon.Rotation += (float)(0.5 * Clock.ElapsedFrameTime);
            base.Update();
        }
    }
}
