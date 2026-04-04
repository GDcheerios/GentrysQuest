using GentrysQuest.Game.Audio.Music;

namespace GentrysQuest.Game.Content.Music
{
    public class Overwhelm : ISong
    {
        public string Name => "Overwhelm";
        public string ArtistName => "JTheWonton";
        public string FileName => "O.V.E.R.W.H.E.L.M..wav";
        public TimingPointsHandler TimingPoints { get; }
    }
}
