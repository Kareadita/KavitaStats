using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Common.Constants;
using Application.Common.Dtos;
using Application.Domain.InstallationStatistics;
using FluentAssertions;
using Tests.Integration.Setup;
using Xunit;

namespace Tests.Integration.Tests
{
    [Collection("MongoDB")]
    public class InstallationStatsControllerTests : IClassFixture<KavitaStatsAppFactory>
    {
        private readonly string _endPoint;
        private readonly HttpClient _client;
        private readonly KavitaStatsAppFactory _factory;

        public InstallationStatsControllerTests(KavitaStatsAppFactory factory)
        {
            _factory = factory;

            _client = factory.CreateAppClient();

            _endPoint = "/api/installationstats";
        }

        [Fact(DisplayName = "Add usage data with success")]
        public async Task Post_SaveUsageData_ReturnSuccess()
        {
            //Given
            var stats = new InstallationStatistics
            {
                InstallId = "my precious id",
                ServerInfo = new ServerInfo {DotNetVersion = "5", Os = "Windows Server"}
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

        [Fact(DisplayName = "Get all data with success")]
        public async Task Get_UsageData_ReturnSuccess()
        {
            //Given
            const string expectedInstallId = "install123";

            var context = _factory.GetDbContext();

            await context.Installations.InsertOneAsync(new InstallationStatistics
            {
                InstallId = expectedInstallId
            });

            //When
            var response = await _client
                .GetAsync($"{_endPoint}")
                .ConfigureAwait(false);

            //Then
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content
                .ReadFromJsonAsync<ApiResponse<InstallationStatistics[]>>()
                .ConfigureAwait(false);

            responseContent.Should().NotBeNull();

            responseContent?.Success.Should().BeTrue();
            responseContent?.Error.Should().BeNullOrEmpty();

            responseContent?.Data.Should().NotBeNullOrEmpty();
            responseContent?.Data.Any(x => x.InstallId.Equals(expectedInstallId))
                .Should().BeTrue();
        }

        [Fact(DisplayName = "Can not get data because there's no api key header")]
        public async Task Get_UsageData_MissingHeader_ReturnUnauthorized()
        {
            //Given

            var unauthorizedClient = _factory.CreateAppClient();

            unauthorizedClient.DefaultRequestHeaders.Remove(AppConstants.AuthHeaderKey);

            //When
            var response = await unauthorizedClient
                .GetAsync($"{_endPoint}")
                .ConfigureAwait(false);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var responseContent = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            responseContent.Should().NotBeNull();
            responseContent.Should().Contain("Api Key was not provided");
        }


        [Fact(DisplayName = "Can not get data because the api key header is invalid")]
        public async Task Get_UsageData_InvalidHeader_ReturnUnauthorized()
        {
            //Given
            var unauthorizedClient = _factory.CreateAppClient();

            unauthorizedClient.DefaultRequestHeaders.Remove(AppConstants.AuthHeaderKey);
            unauthorizedClient.DefaultRequestHeaders.Add(AppConstants.AuthHeaderKey, "just-an-invalid-key");

            //When
            var response = await unauthorizedClient
                .GetAsync($"{_endPoint}")
                .ConfigureAwait(false);

            //Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var responseContent = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            responseContent.Should().NotBeNull();
            responseContent.Should().Contain("Api Key is not valid");
        }
    }
}