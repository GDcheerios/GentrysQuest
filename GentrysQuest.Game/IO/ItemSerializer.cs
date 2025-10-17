using GentrysQuest.Game.ContentRegistry;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Online.API.Requests.Account;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.IO
{
    public static class ItemSerializer
    {
        private static readonly JsonSerializerSettings settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.None
        };

        public static JObject SerializeToObject(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, settings);
            return JObject.Parse(json);
        }

        public static string SerializeToString(object obj) => JsonConvert.SerializeObject(obj, settings);

        [CanBeNull]
        public static EntityBase DeserializeItem(string type, string itemJson)
        {
            RemoveItemRequest removeItemRequest;
            EntityBase entity = null;
            IJsonEntity data = null;

            switch (type.ToLowerInvariant())
            {
                case "character":
                {
                    data = JsonConvert.DeserializeObject<JsonCharacter>(itemJson);
                    entity = CharacterRegistry.Create(data.Name);
                    break;
                }

                case "weapon":
                {
                    data = JsonConvert.DeserializeObject<JsonWeapon>(itemJson);
                    entity = WeaponRegistry.Create(data.Name);
                    break;
                }

                case "artifact":
                {
                    data = JsonConvert.DeserializeObject<JsonArtifact>(itemJson);
                    entity = ArtifactRegistry.Create(data.Name);
                    break;
                }
            }

            if (entity == null && data != null)
            {
                removeItemRequest = new RemoveItemRequest(data.ID);
                _ = removeItemRequest.PerformAsync();
            }
            else if (entity != null)
            {
                entity.ID = data.ID;
            }

            return entity;
        }
    }
}
