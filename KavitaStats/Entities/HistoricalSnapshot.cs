using System;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Entities;

/// <summary>
/// A weekly snapshot of the data to help show growth over time
/// </summary>
[Index("Date", IsUnique = true)]
public class HistoricalSnapshot
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    /// <summary>
    /// SUM of all Installs
    /// </summary>
    public int UniqueInstalls { get; set; }
    /// <summary>
    /// SUM of all users across Installs
    /// </summary>
    public int UniqueUsers { get; set; }

    public int DeltaInstalls { get; set; }
    public int DeltaUsers { get; set; }

    /// <summary>
    /// The kavitareader.com number: v2 and v3 installs that reported in the last 10 days, opted out or not
    /// </summary>
    public int? ActiveInstalls { get; set; }
    public int? NewInstalls { get; set; }
    /// <summary>
    /// Installs whose last report was 30 to 37 days ago, so each is counted once, in the week it crosses 30 days
    /// </summary>
    public int? ChurnedInstalls { get; set; }
    public int? OptedOutInstalls { get; set; }
    public int? ActiveUsers { get; set; }
    public int? InstallsWithContent { get; set; }

}