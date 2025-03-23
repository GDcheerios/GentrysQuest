// * Name              : GentrysQuest.Game
//  * Author           : Brayden J Messerschmidt
//  * Created          : 03/23/2025
//  * Course           : CIS 169 C#
//  * Version          : 1.0
//  * OS               : Windows 11 22H2
//  * IDE              : Jet Brains Rider 2023
//  * Copyright        : This is my work.
//  * Description      : desc.
//  * Academic Honesty : I attest that this is my original work.
//  * I have not used unauthorized source code, either modified or
//  * unmodified. I have not given other fellow student(s) access
//  * to my program.

using GentrysQuest.Game.Audio.Music;

namespace GentrysQuest.Game.Content.Music;

public class AMBI : ISong
{
    public string Name { get; } = "...";
    public string ArtistName { get; } = "...";
    public string FileName { get; } = "AMBI.mp3";
    public TimingPointsHandler TimingPoints { get; } = new();
}
