using System;
using System.Collections.Generic;

namespace KavitaStats.DTOs.V1
{
    /// <summary>
    /// This is for v1 version of the API for versions of Kavita < v0.4.9
    /// </summary>
    public class StatRecordV1Dto
    {
        public string InstallId { get; set; }
        public DateTime LastUpdate { get; set; }
        public ServerInfo ServerInfo { get; set; }
        public List<ClientInfo> ClientsInfo { get; set; } = new List<ClientInfo>();
        public UsageInfo UsageInfo { get; set; }
        public Guid Id { get; set; }
    }


    public class UsageInfo
    {
        public UsageInfo()
        {
            FileTypes = new HashSet<string>();
            LibraryTypesCreated = new HashSet<LibraryInfo>();
        }

        public int UsersCount { get; set; }
        public IEnumerable<string> FileTypes { get; set; }
        public IEnumerable<LibraryInfo> LibraryTypesCreated { get; set; }

    }

    public class LibraryInfo
    {
        public int Type { get; set; }
        public int Count { get; set; }
    }
    
    
    
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
    
    public class ServerInfo
    {
        public string Os { get; set; }
        public string DotNetVersion { get; set; }
        /// <summary>
        /// NOTE: This is the same thing as DotNetVersion
        /// </summary>
        public string RunTimeVersion { get; set; }
        public string KavitaVersion { get; set; }
        public string BuildBranch { get; set; }
        /// <summary>
        /// Culture tag of the machine.
        /// <remarks>Culture will be empty if used within a Docker image. This is a limitation of alpine linux.
        /// https://github.com/dotnet/dotnet-docker/issues/533</remarks>
        /// </summary>
        public string Culture { get; set; }
        public bool IsDocker { get; set; }
        public int NumOfCores { get; set; }
    }
    
    
}