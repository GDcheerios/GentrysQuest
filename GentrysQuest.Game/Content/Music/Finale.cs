using GentrysQuest.Game.Audio.Music;

namespace GentrysQuest.Game.Content.Music
{
    public class Finale : ISong
    {
        public string Name => "Finale";
        public string ArtistName => "JJ";
        public string FileName => "Finale.wav";
        public TimingPointsHandler TimingPoints { get; }
    }
}
