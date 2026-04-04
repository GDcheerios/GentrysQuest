namespace GentrysQuest.Game.Audio.Music
{
    public interface ISong
    {
        /// <summary>
        /// The song name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The artist name
        /// </summary>
        string ArtistName { get; }

        /// <summary>
        /// The file name
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// How we manage song events
        /// </summary>
        TimingPointsHandler TimingPoints { get; }
    }
}

// These are quotes my fellow co-workers made on this file.

// "benis benis bines" - Payton Schutz
// "my meat is super big= 1v2" - Vitalijs Cameron
// "dont listen to vitsljis, hes got a 3.5 incher" - Payton Schutz
// "Elijah like CBT maximum pain" - Ellijah F (Secret last name)
