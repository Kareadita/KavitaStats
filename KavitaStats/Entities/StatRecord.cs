using System;
using KavitaStats.Entities.Interfaces;

namespace KavitaStats.Entities
{
    /// <summary>
    /// Represents information about a Kavita Installation
    /// </summary>
    public class StatRecord : IHasDate
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }
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
        /// <summary>
        /// Number of Cores on the machine
        /// </summary>
        public int NumOfCores { get; set; }
        /// <summary>
        /// Last time we heard from the Kavita instance
        /// </summary>
        /// <remarks>This is required because if nothing changes on the User instance, then Last Modified wont change.</remarks>
        public DateTime LastUpdated { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
    }
}