using Newtonsoft.Json;

namespace CorpusSearchModule.Core.Services.TextCorpusSystemDriver.Models
{
    public class PairQuery : Query
    {
        public string First{ get; set; }
        public string Second { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }

        [JsonIgnore]
        public PairQueryType Type { get; set; }
    }

    public enum PairQueryType
    {
        LemmaPair,
        TagPair
    }
}