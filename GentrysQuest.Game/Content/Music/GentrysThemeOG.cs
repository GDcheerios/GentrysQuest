using GentrysQuest.Game.Audio.Music;

namespace GentrysQuest.Game.Content.Music
{
    public class GentrysThemeOG : ISong
    {
        public string Name => "Gentrys Quest Theme OG";
        public string ArtistName => "Bandito";
        public string FileName => "GentrysTheme.mp3";
        public TimingPointsHandler TimingPoints { get; }

        public GentrysThemeOG()
        {
            TimingPoints = new TimingPointsHandler();
            TimingPoints.AddPoint(new TimingPoint("LogoStart", 1000));
            TimingPoints.AddPoint(new TimingPoint("FrameworkStart", 3000));
            TimingPoints.AddPoint(new TimingPoint("FrameworkFade", 500));
            TimingPoints.AddPoint(new TimingPoint("OsuStart", 6500));
            TimingPoints.AddPoint(new TimingPoint("FadeOut", 10000));
        }
    }
}
