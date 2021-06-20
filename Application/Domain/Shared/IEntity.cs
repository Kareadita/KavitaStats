using System;

namespace Application.Domain.Shared
{
    public interface IEntity
    {
        public Guid Id { get; set; }
    }
}