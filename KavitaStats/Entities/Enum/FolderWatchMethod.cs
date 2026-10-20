using System.ComponentModel;

namespace KavitaStats.Entities.Enum;

public enum FolderWatchMethod
{
    [Description("Off")]
    Off = 0,
    [Description("File Watcher")]
    FileWatcher = 1,
    [Description("Polling")]
    Polling = 2,
    [Description("Trial")]
    Trial = 3,
    [Description("Failed")]
    Failed = 4
}
