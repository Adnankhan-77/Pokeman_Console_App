using System.Text.Json.Serialization;

namespace Stuller_Console_App.Models
{
    public class TypeResponse
    {
        [JsonPropertyName("damage_relations")]
        public DamageRelations DamageRelations { get; set; }
    }
}