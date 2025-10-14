using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.IO
{
    public static class ItemSerializer
    {
        private static readonly JsonSerializerSettings Settings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.None
        };

        public static JObject SerializeToObject(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Settings);
            return JObject.Parse(json);
        }

        public static string SerializeToString(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}
