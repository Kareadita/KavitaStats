using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Domain.UsageStatistics;
using Api.DTO;
using FluentAssertions;
using Tests.Integration.Setup;
using Xunit;

namespace Tests.Integration.Tests
{
    public class UsageSettingsControllerTests : IClassFixture<KavitaStatsAppFactory>
    {
        private readonly string _endPoint;
        private readonly HttpClient _client;
        private readonly KavitaStatsAppFactory _factory;

        public UsageSettingsControllerTests(KavitaStatsAppFactory factory)
        {
            _factory = factory;

            _client = factory.CreateAppClient();

            _endPoint = "/api/usagestatistics";
        }

        [Fact(DisplayName = "Add usage data with success")]
        public async Task Post_SaveUsageData_ReturnSuccess()
        {
            //Given
            var stats = new UsageStatistics
            {
                Id = new Guid(),
                UsersCount = 2,
                FileTypes = new[] {"cbr", "cbz"},
                ServerInfo = new ServerInfo {DotNetVersion = "5"}
            };

            //When
            var response = await _client
                .PostAsJsonAsync($"{_endPoint}", stats)
                .ConfigureAwait(false);

            //Then
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content
                .ReadFromJsonAsync<ApiResponse>()
                .ConfigureAwait(false);

            responseContent.Should().NotBeNull();

            responseContent?.Success.Should().BeTrue();
            responseContent?.Error.Should().BeNullOrEmpty();
        }
        
        [Fact(DisplayName = "Add server info with success")]
        public async Task Post_SaveServerData_ReturnSuccess()
        {
            //Given
            var stats = new ServerInfo {DotNetVersion = "5", Os = "OSX"};

            //When
            var response = await _client
                .PostAsJsonAsync($"{_endPoint}/server-stats", stats)
                .ConfigureAwait(false);

            //Then
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content
                .ReadFromJsonAsync<ApiResponse>()
                .ConfigureAwait(false);

            responseContent.Should().NotBeNull();

            responseContent?.Success.Should().BeTrue();
            responseContent?.Error.Should().BeNullOrEmpty();
        }
    }
}