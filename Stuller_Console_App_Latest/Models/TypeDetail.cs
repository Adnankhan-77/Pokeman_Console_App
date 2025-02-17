using System.Text.Json.Serialization;

namespace Stuller_Console_App.Models
{
    public class TypeDetail
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}