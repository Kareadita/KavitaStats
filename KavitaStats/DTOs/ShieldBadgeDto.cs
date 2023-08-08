namespace KavitaStats.DTOs;

public class ShieldBadgeDto
{
    public int SchemaVersion => 1;
    public string Label => "Active Installs";
    public string Message { get; set; }
    public string Color => "green";
}