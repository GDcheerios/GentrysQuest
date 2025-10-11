using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Online.API
{
    public abstract class APIRequest
    {
        protected static readonly HttpClient Client = new();

        public abstract string Target { get; }
        public string Uri => $@"{APIAccess.Endpoint}/{Target}";
        public void Fail(Exception exception) => Logger.Log(exception.ToString(), LoggingTarget.Network);
        protected virtual HttpMethod Method => HttpMethod.Get;
    }

    // Generic derived class
    public abstract class APIRequest<T> : APIRequest where T : class
    {
        public T Response { get; private set; }

        public async Task PerformAsync()
        {
            var endpoint = $@"{APIAccess.Endpoint.ServerUrl}/{Target}";
            Logger.Log($"Trying request @ {endpoint}", LoggingTarget.Network);

            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(Method, endpoint);
                if (Method == HttpMethod.Post) requestMessage.Content = CreateContent();

                using (var response = await Client.SendAsync(requestMessage))
                {
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsStringAsync();
                    Logger.Log(data, LoggingTarget.Network);
                    Logger.Log(data.GetType().ToString(), LoggingTarget.Network);
                    if (typeof(T) == typeof(string)) Response = data as T;
                    else Response = JsonConvert.DeserializeObject<T>(data);
                }

                Logger.Log($"successful with {Response}", LoggingTarget.Network);
            }
            catch (HttpRequestException e)
            {
                Logger.Log($"Request failed: {e.Message}", LoggingTarget.Network);
                Fail(e);
            }
            catch (Exception e)
            {
                Logger.Log($"Unexpected error: {e.Message}", LoggingTarget.Network);
                Fail(e);
            }
        }

        protected virtual HttpContent CreateContent() => null;
    }
}
