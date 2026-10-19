using System.ComponentModel;

namespace KavitaStats.Entities.Enum;

public enum KavitaPlusState
{
    [Description("None")]
    None = 0,
    [Description("Active")]
    Active = 1,
    [Description("Cancelling")]
    Cancelling = 2,
    [Description("Expired")]
    Expired = 3,
    [Description("Paused")]
    Paused = 4,
    [Description("Unreachable")]
    Unreachable = 5
}
