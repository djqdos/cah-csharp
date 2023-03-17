using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace cah.models
{

    public class Cards
    {
        public List<Set> Sets { get; set; }
    }

    public class Set
    {
        public string name { get; set; }

        [JsonPropertyName("white")]
        public List<WhiteCard> White { get; set; }

        [JsonPropertyName("black")]
        public List<BlackCard> Black { get; set; }
        public bool official { get; set; }
    }
    
    public class WhiteCard
    {
        [Parameter]
        public string text { get; set; }

        [Parameter]
        public int pack { get; set; }
    }
    
    public class BlackCard
    {
        [Parameter]
        public string text { get; set; }

        [Parameter]
        public int pick { get; set; }

        [Parameter]
        public int pack { get; set; }
    }

}