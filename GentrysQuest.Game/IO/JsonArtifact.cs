// * Name              : GentrysQuest.Game
//  * Author           : Brayden J Messerschmidt
//  * Created          : 09/27/2024
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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace GentrysQuest.Game.IO;

public class JsonArtifact : IJsonEntity
{
    public string Name { get; set; }
    public int ID { get; set; }
    public int StarRating { get; set; }
    public int Level { get; set; }
    public int CurrentXp { get; set; }
    public int RequiredXp { get; set; }

    [JsonProperty]
    public string FamilyName { get; set; }

    [JsonProperty]
    public JsonBuff MainBuff { get; set; }

    [JsonProperty]
    public List<JsonBuff> Buffs { get; set; }
}
