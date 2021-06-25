using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Dtos;
using Application.Domain.InstallationStatistics;
using Application.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Application.Features.InstallationStats
{
    public class GetAll
    {
        public record Query : IRequest<Result<IEnumerable<InstallationStatistics>>>;

        public class Handler : IRequestHandler<Query, Result<IEnumerable<InstallationStatistics>>>
        {
            private readonly StatsDbContext _dbContext;
            private readonly ILogger<Handler> _logger;

            public Handler(StatsDbContext dbContext, ILogger<Handler> logger)
            {
                _dbContext = dbContext;
                _logger = logger;
            }

            public async Task<Result<IEnumerable<InstallationStatistics>>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                _logger.LogDebug("Handling GetAll");
                var installs = await _dbContext.Installations
                    .AsQueryable()
                    .ToListAsync(cancellationToken);

                return Result<IEnumerable<InstallationStatistics>>.Ok(installs);
            }
        }
    }
}