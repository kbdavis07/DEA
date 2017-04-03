#pragma warning disable CS1591
using Newtonsoft.Json;

namespace Discord.API.Rpc
{
    internal class SetLocalVolumeResponse
    {
        [JsonProperty("user_id")]
        public ulong userid { get; set; }
        [JsonProperty("volume")]
        public int Volume { get; set; }
    }
}
