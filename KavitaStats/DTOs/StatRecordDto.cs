using KavitaStats.Entities.Enum;

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
        /// <summary>
        /// Number of Cores on the Kavita Install
        /// </summary>
        public int NumOfCores { get; set; }

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
        /// Total number of files in the instance
        /// </summary>
        public int TotalFiles { get; set; }

        /// <summary>
        /// How many updates this row has had
        /// </summary>
        public long UpdateCount { get; set; } = 0;
    }
}