namespace Application.Domain.InstallationStatistics
{
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