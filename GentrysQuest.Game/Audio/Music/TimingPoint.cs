namespace GentrysQuest.Game.Audio.Music
{
    public class TimingPoint(string name = "", int timeMs = 0)
    {
        public string Name { get; private set; } = name;
        public int TimeMs { get; private set; } = timeMs;
    }
}
