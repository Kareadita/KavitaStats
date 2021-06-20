using System.Collections.Generic;
using Application.Domain.Shared.Enumerations;

namespace Application.Domain.InstallationStatistics
{
    public class UsageInfo
    {
        public UsageInfo()
        {
            FileTypes = new HashSet<string>();
            LibraryTypesCreated = new HashSet<LibInfo>();
        }

        public int UsersCount { get; set; }
        public IEnumerable<string> FileTypes { get; set; }
        public IEnumerable<LibInfo> LibraryTypesCreated { get; set; }
    }

    public class LibInfo
    {
        public LibraryType Type { get; set; }
        public int Count { get; set; }
    }
}