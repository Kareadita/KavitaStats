using System;
using KavitaStats.Entities.Enum;
using KavitaStats.Entities.Interfaces;

namespace KavitaStats.Entities
{
    /// <summary>
    /// Represents information about a Kavita Installation
    /// </summary>
    public class StatRecord : IHasDate, IHasUpdateCounter
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
        /// <summary>
        /// If the user has any bookmarks on their install
        /// </summary>
        public bool HasBookmarks { get; set; }
        /// <summary>
        /// The number of libraries
        /// </summary>
        public int NumberOfLibraries { get; set; }
        
        /// <summary>
        /// The site theme the install is using
        /// </summary>
        public string ActiveSiteTheme { get; set; }
        
        /// <summary>
        /// The reading mode the main user has as a preference
        /// </summary>
        public ReaderMode MangaReaderMode { get; set; }
        
        /// <summary>
        /// Number of users on the install
        /// </summary>
        public int NumberOfUsers { get; set; }
        
        /// <summary>
        /// Number of collections on the install
        /// </summary>
        public int NumberOfCollections { get; set; }
        
        /// <summary>
        /// Number of reading lists on the install (Sum of all users)
        /// </summary>
        public int NumberOfReadingLists { get; set; }
        
        /// <summary>
        /// Is OPDS enabled
        /// </summary>
        public bool OPDSEnabled { get; set; }
        
        /// <summary>
        /// The number of files representing a library. This will be max from all libraries.
        /// </summary>
        public int MaxFilesInLibrary { get; set; }

        /// <summary>
        /// How many updates this row has had
        /// </summary>
        public long UpdateCount { get; set; } = 0;

        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
    }
}