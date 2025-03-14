using GentrysQuest.Game.Audio.Music;

namespace GentrysQuest.Game.Content.Music
{
    public class GentrysClassroom : ISong
    {
        public string Name { get; } = "Gentrys Classroom";
        public string ArtistName { get; } = "JJ";
        public string FileName { get; } = "gentrys_classroom.wav";
        public TimingPointsHandler TimingPoints { get; } = new();
    }
}
