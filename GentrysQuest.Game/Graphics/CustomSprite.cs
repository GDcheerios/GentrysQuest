using JetBrains.Annotations;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Game.Graphics
{
    public partial class CustomSprite : Container
    {
        /// <summary>
        /// If using a custom image rather than designing your own.
        /// </summary>
        [CanBeNull]
        private string customImageFile;

        /// <summary>
        /// The sprite to display custom texture
        /// </summary>
        [CanBeNull]
        private Sprite sprite;

        public CustomSprite(string imageFile = null)
        {
            customImageFile = imageFile;
            RelativeSizeAxes = Axes.Both;
        }

        // [BackgroundDependencyLoader]
        // private void load(ITextureStore textureStore)
        // {
        //     if (customImageFile == null) return;
        //
        //     Add(sprite = new Sprite
        //     {
        //         Texture = textureStore.Get(customImageFile)
        //     });
        // }
    }
}
