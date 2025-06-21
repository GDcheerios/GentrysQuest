using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Location;
using GentrysQuest.Game.Tests.Utils;
using NUnit.Framework;
using osu.Framework.Graphics;

namespace GentrysQuest.Game.Tests.Visual.Utils
{
    [TestFixture]
    public partial class MapViewer : GentrysQuestTestScene
    {
        private readonly MapScene mapScene;
        private readonly MapContainer mapContainer;
        private readonly Map map = new Jvee();

        public MapViewer()
        {
            mapScene = new MapScene();
            mapContainer = new MapContainer(mapScene);
            RelativeSizeAxes = Axes.Both;
        }

        [Test]
        public void Test()
        {
            mapScene.LoadMap(map);
            Add(mapContainer);
        }
    }
}
