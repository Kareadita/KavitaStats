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
    public static class CreateOrUpdate
    {
        public record Command (InstallationStatistics Statistics) : IRequest<Result>;

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ILogger<Handler> _logger;
            private readonly StatsDbContext _dbContext;

            public Handler(ILogger<Handler> logger, StatsDbContext dbContext)
            {
                _logger = logger;
                _dbContext = dbContext;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Statistics is null || string.IsNullOrEmpty(request.Statistics.InstallId))
                {
                    _logger.LogError("InstallId was not supplied");
                    return Result.Fail("InstallId is required");
                }

                var filter = Builders<InstallationStatistics>.Filter
                    .Eq(nameof(InstallationStatistics.InstallId), request.Statistics.InstallId);

                var query = await _dbContext.Installations
                    .FindAsync(filter, null, cancellationToken);

                var install = await query.SingleOrDefaultAsync(cancellationToken);

                if (install is not null)
                {
                    request.Statistics.Id = install.Id;

                    _logger.LogDebug("Handling DB Update");
                    await _dbContext.Installations.ReplaceOneAsync(filter, request.Statistics,
                        cancellationToken: cancellationToken);

                    return Result.Ok();
                }

                _logger.LogDebug("Handling DB Insert");
                await _dbContext.Installations.InsertOneAsync(request.Statistics, null, cancellationToken);

                //Transactions are not working with standalone DB/local docker
                // _dbContext.AddCommand(() => _dbContext.Installations.InsertOneAsync(request.Statistics));
                // await _dbContext.SaveChangesAsync(cancellationToken);

                return Result.Ok();
            }
        }
    }
}