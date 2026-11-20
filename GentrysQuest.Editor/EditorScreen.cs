using GentrysQuest.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Editor
{
    public partial class EditorScreen : CompositeDrawable
    {
        private MainGqButton mapEditorButton;

        private MapEditorScene mapEditor;

        public EditorScreen()
        {
            mapEditorButton = new MainGqButton("Map Editor")
            {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Action = loadMapEditor,
            };

            mapEditor = new MapEditorScene();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren =
            [
                // buttons
                mapEditorButton

                // editors
            ];
        }

        private void loadMapEditor() => LoadComponentAsync(mapEditor = new MapEditorScene(), AddInternal);
    }
}
