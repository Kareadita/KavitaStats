namespace KavitaStats.Entities;

public class Color
{
    public int Id { get; set; }
    public string Value { get; set; }
    
    public virtual int StatRecordId { get; set; }
    public virtual StatRecord StatRecord { get; set; }
}