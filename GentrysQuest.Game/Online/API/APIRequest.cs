using System;
using System.Net;
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
        public HttpStatusCode? LastStatusCode { get; private set; }

        public async Task PerformAsync()
        {
            if (APIAccess.IsSessionExpired)
                throw new SessionExpiredException("Session expired. Please log in again.");

            var endpoint = $@"{APIAccess.Endpoint.ServerUrl}/{Target}";
            Logger.Log($"Trying request @ {endpoint}", LoggingTarget.Network);

            try
            {
                bool retriedWithRefreshedApiKey = false;
                LastStatusCode = null;

                while (true)
                {
                    HttpRequestMessage requestMessage = new HttpRequestMessage(Method, endpoint);
                    if (Method == HttpMethod.Post)
                        requestMessage.Content = CreateContent();

                    using (var response = await Client.SendAsync(requestMessage))
                    {
                        LastStatusCode = response.StatusCode;

                        if (shouldRetryWithRefreshedApiKey(response, retriedWithRefreshedApiKey))
                        {
                            await APIAccess.EnsureApiKeyAsync(true);

                            var refreshedApiKey = APIAccess.GetApiKey();
                            if (refreshedApiKey != null)
                            {
                                Client.DefaultRequestHeaders.Authorization =
                                    new System.Net.Http.Headers.AuthenticationHeaderValue(refreshedApiKey.GetHeader());

                                retriedWithRefreshedApiKey = true;
                                continue;
                            }
                        }

                        response.EnsureSuccessStatusCode();
                        var data = await response.Content.ReadAsStringAsync();
                        if (typeof(T) == typeof(string)) Response = data as T;
                        else Response = JsonConvert.DeserializeObject<T>(data);
                    }

                    break;
                }

                Logger.Log($"successful with {endpoint}", LoggingTarget.Network);
            }
            catch (SessionExpiredException e)
            {
                Logger.Log(e.Message, LoggingTarget.Network, LogLevel.Important);
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

        private static bool shouldRetryWithRefreshedApiKey(HttpResponseMessage response, bool alreadyRetried)
        {
            if (alreadyRetried || response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                return false;

            var currentAuth = Client.DefaultRequestHeaders.Authorization;
            if (currentAuth == null)
                return false;

            if ("Bearer".Equals(currentAuth.Scheme, StringComparison.OrdinalIgnoreCase))
                return false;

            return !string.IsNullOrWhiteSpace(APIAccess.GetUserToken());
        }
    }
}
