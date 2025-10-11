using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GentrysQuest.Game.Entity;
using GentrysQuest.Game.Entity.Weapon;
using GentrysQuest.Game.Online.API.Requests.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.Online.API.Requests.Account
{
    public class AddItemRequest : APIRequest<RankingItemResponse>
    {
        private readonly int owner;
        private readonly EntityBase entity;
        private readonly string itemType;
        private readonly object itemPayload;

        public AddItemRequest(int owner, EntityBase entity)
        {
            this.owner = owner;
            this.entity = entity ?? throw new ArgumentNullException(nameof(entity));
            (itemType, itemPayload) = buildItemPayload(entity);
        }

        public override string Target { get; } = "api/gq/add-item/";

        protected override HttpMethod Method => HttpMethod.Post;

        protected override HttpContent CreateContent()
        {
            var payload = new
            {
                type = itemType,
                item = itemPayload,
                owner
            };

            var json = JsonConvert.SerializeObject(payload);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public new async Task PerformAsync()
        {
            var apiKey = APIAccess.GetApiKey();
            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("API key missing. Call EnsureApiKeyAsync first.");

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            try
            {
                await base.PerformAsync();
                applyReturnedIdIfAvailable();
            }
            finally
            {
                Client.DefaultRequestHeaders.Authorization = null;
            }
        }

        private static (string type, object payload) buildItemPayload(EntityBase entity)
        {
            switch (entity)
            {
                case Character c:
                    return ("character", c.ToJson());

                case Artifact a:
                    return ("artifact", a.ToJson());

                case Weapon w:
                    return ("weapon", w.ToJson());

                default:
                    return ("unknown", new
                    {
                        name = entity.Name,
                        id = entity.ID,
                        level = entity.Experience?.CurrentLevel(),
                        starRating = entity.StarRating?.Value
                    });
            }
        }

        private void applyReturnedIdIfAvailable()
        {
            if (Response == null || Response.Item == null) return;

            if (!Response.Item.TryGetValue("id", out var idToken)) return;

            var newId = idToken.Value<int>();

            switch (entity)
            {
                case Character c:
                    c.ID = newId;
                    break;

                case Artifact a:
                    a.ID = newId;
                    break;

                case Weapon w:
                    w.ID = newId;
                    break;

                default:
                    entity.ID = newId;
                    break;
            }
        }
    }
}
