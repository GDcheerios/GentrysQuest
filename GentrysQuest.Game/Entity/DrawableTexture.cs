using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Game.Entity
{
    /// <summary>
    /// Easy mapping for textures.
    /// Makes setting and getting textures easy.
    /// This allows for drawable textures rather than file paths.
    /// </summary>
    public partial class DrawableTexture : Container
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
        }
    }
}
