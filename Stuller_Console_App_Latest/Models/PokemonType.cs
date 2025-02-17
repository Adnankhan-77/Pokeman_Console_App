using System.Text.Json.Serialization;

namespace Stuller_Console_App.Models
{
    public class PokemonType
    {
        [JsonPropertyName("type")]
        public TypeDetail Type { get; set; }
    }
}