using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Common.Dtos;
using FluentAssertions;
using Tests.Integration.Setup;
using Xunit;

namespace Tests.Integration.Tests
{
    public class HealthControllerTests : IClassFixture<KavitaStatsAppFactory>
    {
        private readonly string _endPoint;
        private readonly HttpClient _client;
        private readonly KavitaStatsAppFactory _factory;

        public HealthControllerTests(KavitaStatsAppFactory factory)
        {
            _factory = factory;

            _client = factory.CreateAppClient();

            _endPoint = "/api/health";
        }

        [Fact(DisplayName = "health check with success")]
        public async Task Get_HealthCheck_ReturnSuccess()
        {
            //When
            var response = await _client
                .GetAsync($"{_endPoint}")
                .ConfigureAwait(false);

            //Then
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content
                .ReadFromJsonAsync<ApiResponse>()
                .ConfigureAwait(false);

            responseContent?.Success.Should().BeTrue();
            responseContent?.Error.Should().BeNullOrEmpty();
        }
    }
}