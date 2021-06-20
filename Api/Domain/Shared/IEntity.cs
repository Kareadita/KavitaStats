using System;

namespace Api.Domain.Shared
{
    public interface IEntity
    {
        public Guid Id { get; set; }
    }
}