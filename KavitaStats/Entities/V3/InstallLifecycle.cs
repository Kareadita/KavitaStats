using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Entities.V3;

[Index("InstallId", IsUnique = true)]
public class InstallLifecycle
{
    public int Id { get; set; }
    public string InstallId { get; set; }
    public DateTime FirstSeen { get; set; }
    public DateTime LastSeen { get; set; }
    /// <summary>
    /// The Kavita version the install was first set up with, as the install reports it
    /// </summary>
    public string InitialVersion { get; set; }
    public string LastVersion { get; set; }
    public DateTime? InitialInstallDate { get; set; }
    public bool EverHadSeries { get; set; }
    /// <summary>
    /// Created from an existing ServerStat row, so FirstSeen is "no later than"
    /// </summary>
    public bool Seeded { get; set; }

    public ICollection<KavitaPlusPeriod> KavitaPlusPeriods { get; set; } = new List<KavitaPlusPeriod>();
}
