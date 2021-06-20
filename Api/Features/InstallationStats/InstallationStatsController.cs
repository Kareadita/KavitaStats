﻿using System.Threading.Tasks;
using Api.Common.Base;
using Api.Domain.InstallationStatistics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.InstallationStats
{
    public class InstallationStatsController : ApiKeyController
    {
        private readonly IMediator _mediator;

        public InstallationStatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InstallationStatistics statistics) =>
            ReturnResult(await _mediator.Send(new Create.Command(statistics)));

        [HttpGet]
        public async Task<IActionResult> Get() =>
            ReturnResult(await _mediator.Send(new GetAll.Query()));
    }
}