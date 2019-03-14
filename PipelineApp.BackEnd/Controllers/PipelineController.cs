// <copyright file="PipelineController.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Exceptions.Pipeline;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.DomainModels;
    using Models.RequestModels.Pipeline;
    using Models.ViewModels;

    /// <summary>
    /// Controller class for behavior related to persona data.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PipelineController : BaseController
    {
        private readonly ILogger<PipelineController> _logger;
        private readonly IPipelineService _pipelineService;
        private readonly IPipelineRepository _pipelineRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineController"/> class.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="pipelineService">The pipeline service.</param>
        /// <param name="pipelineRepository">The pipeline repository.</param>
        /// <param name="mapper">The mapper.</param>
        public PipelineController(
            ILogger<PipelineController> logger,
            IPipelineService pipelineService,
            IPipelineRepository pipelineRepository,
            IMapper mapper)
        {
            _logger = logger;
            _pipelineService = pipelineService;
            _pipelineRepository = pipelineRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Processes a request to create a new pipeline for the current user.
        /// </summary>
        /// <param name="requestModel">View model containing information about pipeline to be created.</param>
        /// <returns>
        /// HTTP response containing the results of the request and, if successful,
        /// a <see cref="PipelineDto" /> object in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful creation of pipeline</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PipelineDto))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] CreatePipelineRequestModel requestModel)
        {
            try
            {
                _logger.LogInformation($"Received request to create a pipeline belonging to user {UserId}. Request: {requestModel}");
                requestModel.AssertIsValid();
                var pipeline = _mapper.Map<Pipeline>(requestModel);
                var result = await _pipelineService.CreatePipeline(pipeline, UserId, _pipelineRepository, _mapper);
                var dto = _mapper.Map<PipelineDto>(result);
                _logger.LogInformation($"Processed request to create a pipeline belonging to user {UserId}. Created {dto}");
                return Ok(dto);
            }
            catch (InvalidPipelineException e)
            {
                _logger.LogWarning($"User {UserId} attempted to create invalid pipeline: {requestModel}");
                return BadRequest(e.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
