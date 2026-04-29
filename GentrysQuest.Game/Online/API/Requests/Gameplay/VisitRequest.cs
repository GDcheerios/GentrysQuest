using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GentrysQuest.Game.Location;

namespace GentrysQuest.Game.Online.API.Requests.Gameplay
{
    public class VisitRequest : APIRequest<Visitation>
    {
        private readonly int userId;
        private readonly int locationId;

        public VisitRequest(int userId, int locationId)
        {
            this.userId = userId;
            this.locationId = locationId;
        }

        protected override HttpMethod Method => HttpMethod.Post;
        public override string Target => "api/gq/visit/";

        protected override HttpContent CreateContent()
        {
            var payload = new
            {
                user_id = userId,
                location = locationId
            };

            return new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");
        }

        public new async Task PerformAsync()
        {
            Client.DefaultRequestHeaders.Authorization = await APIAccess.GetApiAuthorizationHeaderAsync();

            try
            {
                await base.PerformAsync();
            }
            finally
            {
                Client.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}
