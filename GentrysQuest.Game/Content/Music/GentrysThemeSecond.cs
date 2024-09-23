using GentrysQuest.Game.Audio.Music;

namespace GentrysQuest.Game.Content.Music
{
    public class GentrysThemeSecond : ISong
    {
        public string Name => "Gentrys Theme";
        public string ArtistName => "Bandito";
        public string FileName => "Gentry_Quest_Intro.mp3";
        public TimingPointsHandler TimingPoints { get; }

        public GentrysThemeSecond()
        {
            TimingPoints = new TimingPointsHandler();
            TimingPoints.AddPoint(new TimingPoint("LogoStart", 1000));
            TimingPoints.AddPoint(new TimingPoint("FrameworkStart", 3000));
            TimingPoints.AddPoint(new TimingPoint("FrameworkFade", 1000));
            TimingPoints.AddPoint(new TimingPoint("OsuStart", 6000));
            TimingPoints.AddPoint(new TimingPoint("FadeOut", 13000));
        }
    }
}
