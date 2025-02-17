using Moq;
using Moq.Protected;
using Stuller_Console_App.ClientWrapper;
using System.Net;

namespace StullerConsoleUnitTest
{
    public class PokeApiServiceTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly PokeApiService _pokeApiService;

        public PokeApiServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://pokeapi.co/api/v2/")
            };

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            _pokeApiService = new PokeApiService(_mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task GetPokemonEffectivenessAsync_ShouldReturnEffectiveness_WhenPokemonExists()
        {
            // Arrange
            var pokemonJson = "{\"types\":[{\"type\":{\"name\":\"electric\",\"url\":\"https://pokeapi.co/api/v2/type/13/\"}}]}";
            var typeJson = "{\"damage_relations\":{\"double_damage_to\":[{\"name\":\"water\"}],\"double_damage_from\":[{\"name\":\"ground\"}],\"no_damage_to\":[{\"name\":\"rock\"}]}}";

            _mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(pokemonJson) })
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(typeJson) });

            // Act
            var result = await _pokeApiService.GetPokemonEffectivenessAsync("pikachu");

            // Assert
            Assert.Contains("Strong Against: water", result);
            Assert.Contains("Weak Against: ground", result);
            Assert.Contains("No Effect: rock", result);
        }

        [Fact]
        public async Task GetPokemonEffectivenessAsync_ShouldReturnError_WhenPokemonNotFound()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound });

            // Act
            var result = await _pokeApiService.GetPokemonEffectivenessAsync("unknown");

            // Assert
            Assert.Equal("Pokemon 'unknown' not found.", result);
        }

        [Fact]
        public async Task GetPokemonEffectivenessAsync_ShouldReturnError_WhenApiFails()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("API failure"));

            // Act
            var result = await _pokeApiService.GetPokemonEffectivenessAsync("pikachu");

            // Assert
            Assert.Contains("Error: Unable to fetch data", result);
        }
    }
}
