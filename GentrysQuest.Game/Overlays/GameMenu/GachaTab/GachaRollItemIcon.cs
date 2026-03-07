using GentrysQuest.Game.Entity;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GentrysQuest.Game.Overlays.GameMenu.GachaTab;

public partial class GachaRollItemIcon(string texture, StarRating rating) : Container
{
    private readonly string texture = texture;
    private readonly StarRating rating = rating;

    [BackgroundDependencyLoader]
    private void load(TextureStore store)
    {
        Size = new Vector2(100);
        Masking = true;
        BorderColour = rating.GetColor();
        BorderThickness = 10;
        Children =
        [
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Gray,
            },
            new Sprite
            {
                Texture = store.Get(texture),
                RelativeSizeAxes = Axes.Both,
                FillAspectRatio = (float)FillMode.Fit,
                Size = new Vector2(0.6f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            }
        ];
    }
}
