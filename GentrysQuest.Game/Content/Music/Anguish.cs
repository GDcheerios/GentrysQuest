using GentrysQuest.Game.Audio.Music;

namespace GentrysQuest.Game.Content.Music
{
    public class Anguish : ISong
    {
        public string Name => "Anguish";
        public string ArtistName => "Bandito";
        public string FileName => "Anguish.mp3";
        public TimingPointsHandler TimingPoints { get; }
    }
}
