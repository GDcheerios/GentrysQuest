using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GentrysQuest.Game.Graphics
{
    public partial class GqCursor : Container
    {
        private readonly Triangle head;
        private readonly Box tail;
        private const float CURSOR_SIZE = 20;

        public GqCursor()
        {
            AutoSizeAxes = Axes.Both;
            Anchor = Anchor.TopLeft;
            Origin = Anchor.TopCentre;
            Rotation = -45;
            Children =
            [
                new Triangle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.BottomCentre,
                    Size = new Vector2((float)(CURSOR_SIZE * 1.10), (float)(CURSOR_SIZE * 0.85)),
                    Colour = Colour4.White,
                },
                new Box
                {
                    Size = new Vector2((float)(CURSOR_SIZE * 0.40), (float)(CURSOR_SIZE * 1.05)),
                    Y = (float)(0.5 + (float)(CURSOR_SIZE * 0.525)),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.BottomCentre,
                    Colour = Colour4.White,
                },
                head = new Triangle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.BottomCentre,
                    Size = new Vector2(CURSOR_SIZE, (float)(CURSOR_SIZE * 0.75)),
                    Colour = Colour4.Black,
                    Y = (float)-0.5
                },
                tail = new Box
                {
                    Size = new Vector2((float)(CURSOR_SIZE * 0.35), CURSOR_SIZE),
                    Y = (float)(CURSOR_SIZE * 0.5),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.BottomCentre,
                    Colour = Colour4.Black
                }
            ];
        }
    }
}
