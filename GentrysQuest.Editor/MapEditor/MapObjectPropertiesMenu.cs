using GentrysQuest.Game.Location;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace GentrysQuest.Editor.MapEditor
{
    public partial class MapObjectPropertiesMenu : CompositeDrawable
    {
        private readonly SpriteText objectNameDrawable;

        public MapObjectPropertiesMenu()
        {
            InternalChildren =
            [
                new Container
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Child = objectNameDrawable = new SpriteText
                    {
                        Text = "Object Name",
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.Both
                    }
                }
            ];
        }

        /// <summary>
        /// Hide the properties window
        /// </summary>
        public void HideProperties() => this.MoveToX(1);

        /// <summary>
        /// Show the properties window
        /// </summary>
        public void ShowProperties() => this.MoveToX(0);

        public void LoadObjectProperties(MapObject mapObject)
        {
            objectNameDrawable.Text = mapObject.Name;
        }
    }
}
