using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GentrysQuest.Game.Overlays.Inventory
{
    public partial class RemoveButton : GqButton
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Size = new Vector2(24);
            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                CornerRadius = 5,
                CornerExponent = 2,
                Children =
                [
                    new Box
                    {
                        Colour = new Colour4(17, 17, 17, 180),
                        RelativeSizeAxes = Axes.Both,
                    },
                    new SpriteIcon
                    {
                        Icon = FontAwesome.Solid.Times,
                        Colour = Colour4.White,
                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(0.65f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    }
                ]
            };
        }
    }
}
