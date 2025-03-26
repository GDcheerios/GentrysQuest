using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.Online.API.Requests.Responses
{
    public record LoginResponse
    {
        public bool Success { get; set; }

        [CanBeNull]
        public string Error { get; set; }

        [CanBeNull]
        public JToken Data { get; set; }
    }
}
