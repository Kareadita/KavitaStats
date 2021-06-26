using System.Threading.Tasks;
using Application.Common.Base;
using Application.Domain.InstallationStatistics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.InstallationStats
{
    public class InstallationStatsController : ApiKeyController
    {
        private readonly IMediator _mediator;

        public InstallationStatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate([FromBody] InstallationStatistics statistics) =>
            ReturnResult(await _mediator.Send(new CreateOrUpdate.Command(statistics)));

        // [HttpGet]
        // public async Task<IActionResult> Get() =>
        //     ReturnResult(await _mediator.Send(new GetAll.Query()));
    }
}