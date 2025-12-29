using System;

namespace KavitaStats.Entities;

/// <summary>
/// A weekly snapshot of the data to help show growth over time
/// </summary>
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
    
}