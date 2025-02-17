using Microsoft.Extensions.DependencyInjection;
using Stuller_Console_App.ClientWrapper;

namespace Stuller_Console_App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddTransient<IPokeApiService, PokeApiService>();
            var serviceProvider = services.BuildServiceProvider();

            var pokeApiService = serviceProvider.GetRequiredService<IPokeApiService>();

            while (true)
            {
                Console.Write("Enter Pokémon name (or type 'exit' to quit): ");
                string pokemonName = Console.ReadLine()?.ToLower();

                if (string.IsNullOrEmpty(pokemonName))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Pokémon name.");
                    continue;
                }

                if (pokemonName == "exit")
                {
                    Console.WriteLine("Exiting application...");
                    break;
                }

                var result = await pokeApiService.GetPokemonEffectivenessAsync(pokemonName);
                Console.WriteLine(result);
            }
        }
    }
}