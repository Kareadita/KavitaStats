using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.InstallationStatistics;
using Api.Infrastructure.Data;
using Api.Shared;
using MediatR;
using MongoDB.Driver;

namespace Api.Features.InstallationStats
{
    public class GetAll
    {
        public record Query : IRequest<Result<IEnumerable<InstallationStatistics>>>;

        public class Handler : IRequestHandler<Query, Result<IEnumerable<InstallationStatistics>>>
        {
            private readonly StatsDbContext _dbContext;

            public Handler(StatsDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Result<IEnumerable<InstallationStatistics>>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                var installs = await _dbContext.Installations
                    .AsQueryable()
                    .ToListAsync(cancellationToken);

                return Result<IEnumerable<InstallationStatistics>>.Ok(installs);
            }
        }
    }
}