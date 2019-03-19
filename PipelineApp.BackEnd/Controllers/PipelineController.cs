// <copyright file="PipelineController.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        /// Processes a request for all of a user's pipelines.
        /// </summary>
        /// <returns>
        /// HTTP response containing the results of the request and, if successful,
        /// a list of <see cref="PipelineDto" /> objects in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful retrieval of pipeline information</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<PipelineDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation($"Received request to get list of available pipelines for user {UserId}.");
                var personas = await _pipelineService.GetAllPipelines(UserId, _pipelineRepository, _mapper);
                var result = personas.ToList().Select(_mapper.Map<PipelineDto>).ToList();
                _logger.LogInformation(
                    $"Processed request to get list of available pipelines for user {UserId}. Found {result.Count} pipelines.");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "An unexpected error occurred.");
            }
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

        /// <summary>
        /// Processes a request to add a tracked persona to the given pipeline.
        /// </summary>
        /// <param name="pipelineId">The unique identifier of the pipeline which should track the persona.</param>
        /// <param name="personaId">The unique identifier of the persona to be tracked.</param>
        /// <returns>
        /// HTTP response containing the results of the request.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful creation of relationship</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPost]
        [Route("{pipelineId}/Persona/{personaId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddPersona(Guid pipelineId, Guid personaId)
        {
            try
            {
                _logger.LogInformation($"Received request for pipeline {pipelineId} to track persona {personaId}");
                await _pipelineService.AssertUserOwnsPipeline(pipelineId, UserId, _pipelineRepository);
                await _pipelineService.AddTrackedPersona(pipelineId, personaId, _pipelineRepository, _mapper);
                _logger.LogInformation($"Processed request for pipeline {pipelineId} to track persona {personaId}");
                return Ok();
            }
            catch (PipelineNotFoundException)
            {
                _logger.LogWarning($"User {UserId} attempted to add persona to invalid pipeline {pipelineId}");
                return BadRequest("A pipeline with this ID does not exist for this user.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Processes a request to add a tracked fandom to the given pipeline.
        /// </summary>
        /// <param name="pipelineId">The unique identifier of the pipeline which should track the persona.</param>
        /// <param name="fandomId">The unique identifier of the fandom to be tracked.</param>
        /// <returns>
        /// HTTP response containing the results of the request.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful creation of relationship</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpPost]
        [Route("{pipelineId}/Fandom/{fandomId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddFandom(Guid pipelineId, Guid fandomId)
        {
            try
            {
                _logger.LogInformation($"Received request for pipeline {pipelineId} to track fandom {fandomId}");
                await _pipelineService.AssertUserOwnsPipeline(pipelineId, UserId, _pipelineRepository);
                await _pipelineService.AddTrackedFandom(pipelineId, fandomId, _pipelineRepository, _mapper);
                _logger.LogInformation($"Processed request for pipeline {pipelineId} to track fandom {fandomId}");
                return Ok();
            }
            catch (PipelineNotFoundException)
            {
                _logger.LogWarning($"User {UserId} attempted to add persona to invalid pipeline {pipelineId}");
                return BadRequest("A pipeline with this ID does not exist for this user.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
