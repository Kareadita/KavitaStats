using System;

namespace Application.Domain.InstallationStatistics
{
    /// <summary>
    /// Information about the (web) device used to read on Kavita. Only recorded from accessing webui.
    /// </summary>
    public class ClientInfo
    {
        public string KavitaUiVersion { get; set; }
        public string ScreenResolution { get; set; }
        public string PlatformType { get; set; }
        public SoftwareVersion Browser { get; set; }
        public SoftwareVersion Os { get; set; }
        public DateTime? CollectedAt { get; set; }
        public bool UsingDarkTheme { get; set; }
    }
    
    /// <summary>
    /// Represents a version of some software. For example, A browser might be "Microsoft Edge" with version "90.10.1".
    /// </summary>
    public class SoftwareVersion
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }
}