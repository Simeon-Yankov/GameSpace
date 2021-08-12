using System.Net;

using Newtonsoft.Json;

namespace GameSpace.Services.Sumonners.Models
{
    public class ImportSummonerStatusService
    {
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; init; }

        public string Message { get; init; }
    }
}