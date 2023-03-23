using System;

namespace KavitaStats.Entities.Interfaces;

public interface IHasDate
{
    DateTime Created { get; set; }
    DateTime LastModified { get; set; }
}