using System.Text.Json.Serialization;

namespace Stuller_Console_App.Models
{
    public class DamageRelations
    {
        [JsonPropertyName("double_damage_to")]
        public TypeDetail[] DoubleDamageTo { get; set; }

        [JsonPropertyName("double_damage_from")]
        public TypeDetail[] DoubleDamageFrom { get; set; }

        [JsonPropertyName("no_damage_to")]
        public TypeDetail[] NoDamageTo { get; set; }
    }
}