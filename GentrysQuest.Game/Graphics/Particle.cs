using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osuTK;
using osuTK.Graphics;

namespace GentrysQuest.Game.Graphics
{
    public partial class Particle : Drawable
    {
        private Color4 color = Color4.White;
        private Vector2 size = new Vector2(20, 20);

        public Particle(Color4 color, Vector2 size)
        {
            this.color = color;
            this.size = size;
        }

        protected override DrawNode CreateDrawNode() => new ParticleDrawNode(this);

        [BackgroundDependencyLoader]
        private void load()
        {
        }

        protected override void Update()
        {
            base.Update();
        }

        private class ParticleDrawNode : DrawNode
        {
            private Particle source;
            private Quad squareQuad;
            private Color4 drawColor;

            public ParticleDrawNode(Particle source)
                : base(source)
            {
                this.source = source;
            }

            public override void ApplyState()
            {
                base.ApplyState();
                drawColor = source.color;
                squareQuad = new Quad();
            }

            protected override void Draw(IRenderer renderer)
            {
                base.Draw(renderer);

                var drawQuad = new Quad(
                    Vector2Extensions.Transform(squareQuad.TopLeft * source.DrawSize, DrawInfo.Matrix),
                    Vector2Extensions.Transform(squareQuad.TopRight * source.DrawSize, DrawInfo.Matrix),
                    Vector2Extensions.Transform(squareQuad.BottomLeft * source.DrawSize, DrawInfo.Matrix),
                    Vector2Extensions.Transform(squareQuad.BottomRight * source.DrawSize, DrawInfo.Matrix)
                );

                renderer.DrawQuad(renderer.WhitePixel, drawQuad, drawColor);
            }
        }
    }
}
