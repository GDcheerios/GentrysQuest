namespace GentrysQuest.Game.Utils
{
    public record DialogueEvent
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public double Duration { get; set; }
    }
}
