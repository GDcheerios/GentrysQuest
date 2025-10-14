using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GentrysQuest.Game.Online.API;

public class APIKeyResponse
{
    [JsonProperty("id")]
    private string id;

    [JsonProperty("user_id")]
    private int uid;

    [JsonProperty("key_id")]
    private string keyId;

    [JsonProperty("secret")]
    private byte[] secret;

    [CanBeNull]
    [JsonProperty("name")]
    private string name;

    [JsonProperty("scopes")]
    private string[] scopes;

    [JsonProperty("created_at")]
    private string createdAt;

    [JsonProperty("last_used_at")]
    private string lastUsedAt;

    [CanBeNull]
    [JsonProperty("expires_at")]
    private string expiresAt;

    [JsonProperty("status")]
    private string status;

    [JsonProperty("created_by")]
    private string createdBy;

    [CanBeNull]
    [JsonProperty("metadata")]
    private JObject metadata;
}
