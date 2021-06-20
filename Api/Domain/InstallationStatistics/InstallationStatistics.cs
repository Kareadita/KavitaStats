using System;
using System.Collections.Generic;
using Api.Domain.Shared;

namespace Api.Domain.InstallationStatistics
{
    public class InstallationStatistics : IEntity
    {
        public InstallationStatistics()
        {
            ClientsInfo = new List<ClientInfo>();
        }

        public string InstallId { get; set; }
        public DateTime LastUpdate { get; set; }
        public ServerInfo ServerInfo { get; set; }
        public List<ClientInfo> ClientsInfo { get; set; }

        public UsageInfo UsageInfo { get; set; }

        public Guid Id { get; set; }
    }
}