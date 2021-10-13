namespace KavitaStats.DTOs
{
    /// <summary>
    /// Represents information about a Kavita Installation
    /// </summary>
    public class StatRecordDto
    {
        /// <summary>
        /// Unique Id that represents a unique install
        /// </summary>
        public string InstallId { get; set; }
        /// <summary>
        /// If the Kavita install is using Docker
        /// </summary>
        public bool IsDocker { get; set; }
        /// <summary>
        /// Version of .NET instance is running
        /// </summary>
        public string DotnetVersion { get; set; }
        /// <summary>
        /// Version of Kavita
        /// </summary>
        public string KavitaVersion { get; set; }
    }
}