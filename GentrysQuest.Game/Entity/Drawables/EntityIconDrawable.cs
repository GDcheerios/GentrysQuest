using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Entity.Drawables
{
    public partial class EntityIconDrawable : Sprite
    {
        public EntityIconDrawable()
        {
            RelativeSizeAxes = Axes.Both;
            FillMode = FillMode.Fit;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }
    }
}
