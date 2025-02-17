namespace Stuller_Console_App.ClientWrapper
{
    public interface IPokeApiService
    {
        Task<string> GetPokemonEffectivenessAsync(string pokemonName);
    }
}