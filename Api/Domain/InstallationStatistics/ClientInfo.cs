using System;

namespace Api.Domain.InstallationStatistics
{
    public class ClientInfo
    {
        public string KavitaUiVersion { get; set; }
        public string ScreenResolution { get; set; }
        public string PlatformType { get; set; }
        public DetailsVersion Browser { get; set; }
        public DetailsVersion Os { get; set; }

        public DateTime? CollectedAt { get; set; }
    }

    public class DetailsVersion
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }
}