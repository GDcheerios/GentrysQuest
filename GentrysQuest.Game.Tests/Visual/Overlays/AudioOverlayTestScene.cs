using GentrysQuest.Game.Audio.Music;
using GentrysQuest.Game.Content.Music;
using GentrysQuest.Game.Overlays;
using NUnit.Framework;

namespace GentrysQuest.Game.Tests.Visual.Overlays
{
    [TestFixture]
    public partial class AudioOverlayTestScene : GentrysQuestTestScene
    {
        private AudioOverlay audioOverlay;
        private readonly ISong song = new GentrysQuestAmbient();

        public AudioOverlayTestScene()
        {
            Add(audioOverlay = new AudioOverlay());
        }

        [Test]
        public void Test()
        {
            AddStep("Test", () => { audioOverlay.DisplaySong(song); });
        }
    }
}
