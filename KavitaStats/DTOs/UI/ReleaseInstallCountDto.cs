namespace KavitaStats.DTOs.UI;

/// <summary>
/// The number of installs for a given release version
/// </summary>
public class ReleaseInstallCountDto
{
    public string ReleaseVersion { get; set; }
    public int InstallCount { get; set; }
}