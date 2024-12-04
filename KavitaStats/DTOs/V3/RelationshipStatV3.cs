using KavitaStats.Entities.Enum;

namespace KavitaStats.DTOs.V3;

/// <summary>
/// KavitaStats - Information about Series Relationships
/// </summary>
public class RelationshipStatV3
{
    public int Count { get; set; }
    public RelationKind Relationship { get; set; }
}
