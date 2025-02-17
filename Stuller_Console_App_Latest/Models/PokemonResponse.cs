using System.Text.Json.Serialization;

namespace Stuller_Console_App.Models
{
    public class PokemonResponse
    {
        [JsonPropertyName("types")]
        public PokemonType[] Types { get; set; }
    }
}