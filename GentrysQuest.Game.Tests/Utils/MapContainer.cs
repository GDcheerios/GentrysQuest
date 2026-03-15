using GentrysQuest.Game.Location;
using GentrysQuest.Game.Utils;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace GentrysQuest.Game.Tests.Utils
{
    public partial class MapContainer : Container
    {
        private MapScene mapScene;

        private bool up;
        private bool down;
        private bool left;
        private bool right;

        public MapContainer(MapScene mapScene)
        {
            this.mapScene = mapScene;
            Add(mapScene);
        }

        [BackgroundDependencyLoader]
        private void load() => RelativeSizeAxes = Axes.Both;

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.A:
                    left = true;
                    break;

                case Key.D:
                    right = true;
                    break;

                case Key.W:
                    up = true;
                    break;

                case Key.S:
                    down = true;
                    break;

                case Key.Down:
                    mapScene.GetMap().Scale *= 0.5f;
                    mapScene.GetMap().Position *= 0.5f;
                    break;

                case Key.Up:
                    mapScene.GetMap().Scale *= 2f;
                    mapScene.GetMap().Position *= 2f;
                    break;
            }

            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            switch (e.Key)
            {
                case Key.A:
                    left = false;
                    break;

                case Key.D:
                    right = false;
                    break;

                case Key.W:
                    up = false;
                    break;

                case Key.S:
                    down = false;
                    break;
            }

            base.OnKeyUp(e);
        }

        protected override void Update()
        {
            Vector2 direction = Vector2.Zero;

            if (left) direction -= MathBase.GetAngleToVector(180);
            if (right) direction -= MathBase.GetAngleToVector(0);
            if (up) direction -= MathBase.GetAngleToVector(270);
            if (down) direction -= MathBase.GetAngleToVector(90);

            mapScene.GetMap().Position += direction * (float)Clock.ElapsedFrameTime * 1;
        }
    }
}
