using GentrysQuest.Game.Content.Maps;
using GentrysQuest.Game.Location;
using NUnit.Framework;
using osu.Framework.Testing;

namespace GentrysQuest.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneMap : TestScene
    {
        private readonly MapScene mapScene;
        private readonly Map map = new RaccoonRiver();

        public TestSceneMap() => mapScene = new MapScene();

        [Test]
        public void Test()
        {
            mapScene.LoadMap(map);
            Add(mapScene);
        }
    }
}
