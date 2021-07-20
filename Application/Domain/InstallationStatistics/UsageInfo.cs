using System.Collections.Generic;
using Application.Domain.Shared.Enumerations;

namespace Application.Domain.InstallationStatistics
{
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
        public LibraryType Type { get; set; }
        public int Count { get; set; }
    }
}