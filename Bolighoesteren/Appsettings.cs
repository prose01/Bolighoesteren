using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bolighoesteren
{
    [JsonObject("appsettings")]
    internal class Appsettings
    {
        [JsonProperty("FilterName")]
        public FilterName FilterName { get; set; } = FilterName.edc;

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