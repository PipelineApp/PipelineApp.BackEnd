// <copyright file="FandomController.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Interfaces;
    using Interfaces.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.ViewModels;

    /// <summary>
    /// Controller class for behavior related to fandom data.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class FandomController : BaseController
    {
        private readonly ILogger<FandomController> _logger;
        private readonly IMapper _mapper;
        private readonly IFandomService _fandomService;
        private readonly IGraphDbClient _graphDbClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FandomController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="fandomService">The fandom service.</param>
        /// <param name="graphDbClient">The graph DB client.</param>
        public FandomController(
            ILogger<FandomController> logger,
            IMapper mapper,
            IFandomService fandomService,
            IGraphDbClient graphDbClient)
        {
            _logger = logger;
            _mapper = mapper;
            _fandomService = fandomService;
            _graphDbClient = graphDbClient;
        }

        /// <summary>
        /// Processes a request for all fandoms.
        /// </summary>
        /// <returns>
        /// HTTP response containing the results of the request and, if successful,
        /// a list of <see cref="FandomDto" /> objects in the response body.<para />
        /// <list type="table">
        /// <item><term>200 OK</term><description>Response code for successful retrieval of fandom information</description></item>
        /// <item><term>500 Internal Server Error</term><description>Response code for unexpected errors</description></item>
        /// </list>
        /// </returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<FandomDto>))]
        [ProducesResponseType(500)]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation(
                    $"Received request to get list of available fandoms for user {UserId}.");
                var fandoms = _fandomService.GetAllFandoms(_graphDbClient, _mapper);
                var result = fandoms.Select(_mapper.Map<FandomDto>).ToList();
                _logger.LogInformation(
                    $"Processed request to get list of available fandoms for user {UserId}. Found {result.Count} fandoms.");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
