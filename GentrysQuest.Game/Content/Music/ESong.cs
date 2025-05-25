using GentrysQuest.Game.Audio.Music;

namespace GentrysQuest.Game.Content.Music
{
    public class ESong : ISong
    {
        public string Name { get; }= "E";
        public string ArtistName { get; } = "JTheWonton";
        public string FileName { get; } = "e.wav";
        public TimingPointsHandler TimingPoints { get; }
    }
}
