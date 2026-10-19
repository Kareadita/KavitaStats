using System.ComponentModel;

namespace KavitaStats.Entities.Enum;

public enum KavitaPlusPeriodEndReason
{
    [Description("Expired")]
    Expired = 0,
    [Description("Paused")]
    Paused = 1,
    [Description("License Removed")]
    LicenseRemoved = 2,
    [Description("Went Silent")]
    WentSilent = 3,
    [Description("Opted Out")]
    OptedOut = 4,
    /// <summary>
    /// A Kavita without <see cref="KavitaPlusState"/> reported the flag as false
    /// </summary>
    [Description("Unknown")]
    Unknown = 5,
    /// <summary>
    /// Free or paid changed, a new period starts the same day
    /// </summary>
    [Description("Plan Changed")]
    PlanChanged = 6
}
