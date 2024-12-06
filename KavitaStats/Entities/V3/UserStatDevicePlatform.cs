using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities.V3;

public class UserStatDevicePlatform
{
    public int Id { get; set; }
    public DevicePlatform DevicePlatform { get; set; }
    
    public int UserStatId { get; set; }
    public virtual UserStat UserStat { get; set; }
}