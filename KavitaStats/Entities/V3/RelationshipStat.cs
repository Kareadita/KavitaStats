using KavitaStats.DTOs.V1;
using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities.V3;

public class RelationshipStat
{
    public int Id { get; set; }
    public int Count { get; set; }
    public RelationKind Relationship { get; set; } 
    
    public int ServerStatId { get; set; }
    public virtual ServerStat ServerStat { get; set; }
}