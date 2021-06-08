using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Api.Domain.UsageStatistics
{
    public class UsageStatistics
    {
        public Guid Id { get; set; }
        public int UsersCount { get; set; }
        public ServerInfo ServerInfo { get; set; }
        public IEnumerable<ClientInfo> ClientsInfo { get; set; }
        public IEnumerable<string> FileTypes { get; set; }
        public IEnumerable<LibraryType> LibraryTypesCreated { get; set; }
    }

    public class ClientInfo
    {
        public string Os { get; set; }
        public string Device { get; set; }
        public DeviceType DeviceType { get; set; }
        public string ScreenSize { get; set; }
        public string ScreenResolution { get; set; }
        public string KavitaUiVersion { get; set; }
        public string BuildBranch { get; set; }
    }

    public class ServerInfo
    {
        public string Os { get; set; }
        public string DotNetVersion { get; set; }
        public string KavitaVersion { get; set; }
        public string BuildBranch { get; set; }
        public string Locale { get; set; }
    }

    public enum DeviceType
    {
        Desktop = 0,
        Laptop = 1,
        Tablet = 2,
        Mobile = 3
    }

    public enum LibraryType
    {
        [Description("Manga")] Manga = 0,
        [Description("Comic")] Comic = 1,
        [Description("Book")] Book = 2
    }
}