using Stuller_Console_App.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Stuller_Console_App.ClientWrapper
{
    public class PokeApiService : IPokeApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PokeApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetPokemonEffectivenessAsync(string pokemonName)
        {
            try
            {
                var pokemonData = await FetchPokemonDataAsync(pokemonName);
                if (pokemonData == null)
                    return $"Pokemon '{pokemonName}' not found.";

                if (pokemonData.Types == null || pokemonData.Types.Length == 0)
                    return "Pokemon type not found.";

                var typeTasks = pokemonData.Types.Select(t => FetchTypeDataAsync(t.Type.Url));
                var typeResponses = await Task.WhenAll(typeTasks);

                return FormatEffectiveness(typeResponses);
            }
            catch (HttpRequestException ex)
            {
                return $"Error: Unable to fetch data - {ex.Message}";
            }
            catch (JsonException)
            {
                return "Error: Failed to parse response from API.";
            }
            catch (Exception ex)
            {
                return $"Unexpected error: {ex.Message}";
            }
        }

        private async Task<PokemonResponse> FetchPokemonDataAsync(string pokemonName)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{pokemonName}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"API returned {response.StatusCode}");

            return await response.Content.ReadFromJsonAsync<PokemonResponse>();
        }

        private async Task<TypeResponse> FetchTypeDataAsync(string typeUrl)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(typeUrl);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"API returned {response.StatusCode}");

            return await response.Content.ReadFromJsonAsync<TypeResponse>();
        }

        private static string FormatEffectiveness(IEnumerable<TypeResponse> typeResponses)
        {
            var strongAgainst = typeResponses.SelectMany(tr => tr.DamageRelations.DoubleDamageTo).Select(t => t.Name).Distinct();
            var weakAgainst = typeResponses.SelectMany(tr => tr.DamageRelations.DoubleDamageFrom).Select(t => t.Name).Distinct();
            var noEffect = typeResponses.SelectMany(tr => tr.DamageRelations.NoDamageTo).Select(t => t.Name).Distinct();

            return strongAgainst.Any() || weakAgainst.Any() || noEffect.Any()
                ? $"Strong Against: {string.Join(", ", strongAgainst)}\n" +
                  $"Weak Against: {string.Join(", ", weakAgainst)}\n" +
                  $"No Effect: {string.Join(", ", noEffect)}"
                : "No type effectiveness data available.";
        }
    }
}