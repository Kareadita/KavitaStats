using System.Threading;
using System.Threading.Tasks;
using Application.Common.Dtos;
using Application.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.InstallationStats
{
    public static class Create
    {
        public record Command (Domain.InstallationStatistics.InstallationStatistics Statistics) : IRequest<Result>;

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
                    return Result.Fail("InstallId is required");
                }

                await _dbContext.Installations.InsertOneAsync(request.Statistics, null, cancellationToken);

                //Transactions are not working with standalone DB/local docker
                // _dbContext.AddCommand(() => _dbContext.Installations.InsertOneAsync(request.Statistics));
                // await _dbContext.SaveChangesAsync(cancellationToken);

                return Result.Ok();
            }
        }
    }
}