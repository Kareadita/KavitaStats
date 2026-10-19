using System.ComponentModel;

namespace KavitaStats.Entities.Enum;

public enum FolderWatchMode
{
    [Description("Off")]
    Off = 0,
    [Description("Automatic")]
    Automatic = 1,
    [Description("File Watcher")]
    FileWatcher = 2,
    [Description("Polling")]
    Polling = 3
}
