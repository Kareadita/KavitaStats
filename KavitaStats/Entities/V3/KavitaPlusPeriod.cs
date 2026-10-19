using System;
using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities.V3;

public class KavitaPlusPeriod
{
    public int Id { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime? CancellingOn { get; set; }
    public DateTime? EndedOn { get; set; }
    public KavitaPlusPeriodEndReason? EndReason { get; set; }
    public bool? IsFree { get; set; }
    public bool Seeded { get; set; }

    public int InstallLifecycleId { get; set; }
    public InstallLifecycle InstallLifecycle { get; set; }
}
