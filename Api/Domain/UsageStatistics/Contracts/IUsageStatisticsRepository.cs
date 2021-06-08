using System.Threading.Tasks;

namespace Api.Domain.UsageStatistics.Contracts
{
    public interface IUsageStatisticsRepository
    {
        Task Add(UsageStatistics usageStatistics);
        Task Add(ServerInfo serverInfo);
        Task Add(ClientInfo clientInfo);
    }
}