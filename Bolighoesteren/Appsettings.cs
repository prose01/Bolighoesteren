using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bolighoesteren
{
    [JsonObject("appsettings")]
    internal class Appsettings
    {
        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("Console")]
        public bool Console { get; set; } = true;

        [JsonProperty("ThreadSleep")]
        public int ThreadSleep { get; set; } = 10000;

        [JsonProperty("DataDumpLocation")]
        public string DataDumpLocation { get; set; }

        [JsonProperty("Postnumre")]
        public List<string> Postnumre { get; set; }
    }
}